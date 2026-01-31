using PlataformaMarcenaria.API.DTOs.Budget;
using PlataformaMarcenaria.API.Entities;
using PlataformaMarcenaria.API.Exceptions;
using PlataformaMarcenaria.API.Repositories;

namespace PlataformaMarcenaria.API.Services;

public class BudgetRequestService : IBudgetRequestService
{
    private readonly IBudgetRequestRepository _budgetRequestRepository;
    private readonly IUserService _userService;
    private readonly IAddressService _addressService;

    public BudgetRequestService(
        IBudgetRequestRepository budgetRequestRepository,
        IUserService userService,
        IAddressService addressService)
    {
        _budgetRequestRepository = budgetRequestRepository;
        _userService = userService;
        _addressService = addressService;
    }

    public async Task<BudgetRequestResponseDTO> CreateBudgetRequestAsync(BudgetRequestCreateDTO createDTO)
    {
        var client = await _userService.FindUserByIdAsync(createDTO.ClientId);
        if (client.UserType != UserType.CLIENT)
        {
            throw new BusinessException("Apenas clientes podem criar orçamentos");
        }

        var location = await _addressService.FindAddressByIdAsync(createDTO.LocationId);
        if (location.UserId != client.Id)
        {
            throw new BusinessException("O endereço informado não pertence ao cliente");
        }

        var budgetRequest = new BudgetRequest
        {
            ClientId = createDTO.ClientId,
            Description = createDTO.Description,
            ReferenceImages = createDTO.ReferenceImages ?? new List<string>(),
            LocationId = createDTO.LocationId,
            EstimatedBudget = createDTO.EstimatedBudget,
            DesiredDeadline = createDTO.DesiredDeadline,
            Status = BudgetRequest.BudgetStatus.OPEN
        };

        var savedBudgetRequest = await _budgetRequestRepository.CreateAsync(budgetRequest);
        return await ConvertToResponseDTOAsync(savedBudgetRequest);
    }

    public async Task<BudgetRequestResponseDTO> GetBudgetRequestByIdAsync(long id)
    {
        var budgetRequest = await FindBudgetRequestByIdAsync(id);
        return await ConvertToResponseDTOAsync(budgetRequest);
    }

    public async Task<BudgetRequest> FindBudgetRequestByIdAsync(long id)
    {
        var budgetRequest = await _budgetRequestRepository.GetByIdAsync(id);
        if (budgetRequest == null)
        {
            throw new ResourceNotFoundException("Orçamento", "id", id);
        }
        return budgetRequest;
    }

    public async Task<IEnumerable<BudgetRequestResponseDTO>> GetBudgetRequestsByClientIdAsync(long clientId)
    {
        var budgetRequests = await _budgetRequestRepository.GetByClientIdAsync(clientId);
        var result = new List<BudgetRequestResponseDTO>();
        foreach (var br in budgetRequests)
        {
            result.Add(await ConvertToResponseDTOAsync(br));
        }
        return result;
    }

    public async Task<IEnumerable<BudgetRequestResponseDTO>> GetBudgetRequestsByStatusAsync(BudgetRequest.BudgetStatus status)
    {
        var budgetRequests = await _budgetRequestRepository.GetByStatusAsync(status);
        var result = new List<BudgetRequestResponseDTO>();
        foreach (var br in budgetRequests)
        {
            result.Add(await ConvertToResponseDTOAsync(br));
        }
        return result;
    }

    public async Task<IEnumerable<BudgetRequestResponseDTO>> GetBudgetRequestsByLocationAsync(string city, string state)
    {
        var budgetRequests = await _budgetRequestRepository.GetByLocationAsync(city, state);
        var result = new List<BudgetRequestResponseDTO>();
        foreach (var br in budgetRequests)
        {
            result.Add(await ConvertToResponseDTOAsync(br));
        }
        return result;
    }

    public async Task<BudgetRequestResponseDTO> UpdateBudgetRequestStatusAsync(long id, BudgetRequest.BudgetStatus newStatus)
    {
        var budgetRequest = await FindBudgetRequestByIdAsync(id);
        ValidateStatusTransition(budgetRequest.Status, newStatus);
        budgetRequest.Status = newStatus;
        var updated = await _budgetRequestRepository.UpdateAsync(budgetRequest);
        return await ConvertToResponseDTOAsync(updated);
    }

    public async Task DeleteBudgetRequestAsync(long id)
    {
        var budgetRequest = await FindBudgetRequestByIdAsync(id);
        if (budgetRequest.Status != BudgetRequest.BudgetStatus.OPEN)
        {
            throw new BusinessException("Não é possível excluir um orçamento que já está em andamento");
        }
        await _budgetRequestRepository.DeleteAsync(budgetRequest);
    }

    private void ValidateStatusTransition(BudgetRequest.BudgetStatus currentStatus, BudgetRequest.BudgetStatus newStatus)
    {
        if (currentStatus == BudgetRequest.BudgetStatus.CLOSED || currentStatus == BudgetRequest.BudgetStatus.CANCELLED)
        {
            throw new BusinessException("Não é possível alterar o status de um orçamento finalizado ou cancelado");
        }

        if (newStatus == BudgetRequest.BudgetStatus.OPEN && currentStatus != BudgetRequest.BudgetStatus.OPEN)
        {
            throw new BusinessException("Não é possível voltar o status para aberto");
        }
    }

    private async Task<BudgetRequestResponseDTO> ConvertToResponseDTOAsync(BudgetRequest budgetRequest)
    {
        var dto = new BudgetRequestResponseDTO
        {
            Id = budgetRequest.Id,
            Client = await _userService.GetUserByIdAsync(budgetRequest.ClientId),
            Description = budgetRequest.Description,
            ReferenceImages = budgetRequest.ReferenceImages,
            Status = budgetRequest.Status,
            Location = await _addressService.GetAddressByIdAsync(budgetRequest.LocationId),
            EstimatedBudget = budgetRequest.EstimatedBudget,
            DesiredDeadline = budgetRequest.DesiredDeadline,
            CreatedAt = budgetRequest.CreatedAt,
            UpdatedAt = budgetRequest.UpdatedAt
        };

        // Calculando estatísticas dos lances
        var bids = budgetRequest.Bids?.ToList() ?? new List<Bid>();
        dto.TotalBids = bids.Count;
        if (bids.Any())
        {
            dto.LowestBid = bids.Min(b => b.Price);
            dto.HighestBid = bids.Max(b => b.Price);
        }

        return dto;
    }
}

