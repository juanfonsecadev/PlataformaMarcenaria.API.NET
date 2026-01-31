using PlataformaMarcenaria.API.DTOs.Visit;
using PlataformaMarcenaria.API.Entities;
using PlataformaMarcenaria.API.Exceptions;
using PlataformaMarcenaria.API.Repositories;

namespace PlataformaMarcenaria.API.Services;

public class VisitService : IVisitService
{
    private readonly IVisitRepository _visitRepository;
    private readonly IUserService _userService;
    private readonly IBudgetRequestService _budgetRequestService;

    public VisitService(
        IVisitRepository visitRepository,
        IUserService userService,
        IBudgetRequestService budgetRequestService)
    {
        _visitRepository = visitRepository;
        _userService = userService;
        _budgetRequestService = budgetRequestService;
    }

    public async Task<VisitResponseDTO> CreateVisitAsync(VisitCreateDTO createDTO)
    {
        var seller = await _userService.FindUserByIdAsync(createDTO.SellerId);
        if (seller.UserType != UserType.SELLER)
        {
            throw new BusinessException("Apenas vendedores podem agendar visitas");
        }

        var budgetRequest = await _budgetRequestService.FindBudgetRequestByIdAsync(createDTO.BudgetRequestId);
        if (budgetRequest.Status != BudgetRequest.BudgetStatus.OPEN)
        {
            throw new BusinessException("Não é possível agendar visita para um orçamento que não está aberto");
        }

        // Verificar se já existe visita agendada para o mesmo horário
        if (await HasConflictingVisitAsync(seller.Id, createDTO.ScheduledDate))
        {
            throw new BusinessException("Já existe uma visita agendada para este horário");
        }

        var visit = new Visit
        {
            SellerId = createDTO.SellerId,
            BudgetRequestId = createDTO.BudgetRequestId,
            ScheduledDate = createDTO.ScheduledDate,
            Notes = createDTO.Notes,
            Status = Visit.VisitStatus.SCHEDULED
        };

        var savedVisit = await _visitRepository.CreateAsync(visit);

        // Atualizar status do orçamento
        await _budgetRequestService.UpdateBudgetRequestStatusAsync(
            budgetRequest.Id,
            BudgetRequest.BudgetStatus.WAITING_VISIT
        );

        return await ConvertToResponseDTOAsync(savedVisit);
    }

    public async Task<VisitResponseDTO> GetVisitByIdAsync(long id)
    {
        var visit = await FindVisitByIdAsync(id);
        return await ConvertToResponseDTOAsync(visit);
    }

    public async Task<IEnumerable<VisitResponseDTO>> GetVisitsBySellerIdAsync(long sellerId)
    {
        var visits = await _visitRepository.GetBySellerIdAsync(sellerId);
        var result = new List<VisitResponseDTO>();
        foreach (var visit in visits)
        {
            result.Add(await ConvertToResponseDTOAsync(visit));
        }
        return result;
    }

    public async Task<IEnumerable<VisitResponseDTO>> GetVisitsByBudgetRequestIdAsync(long budgetRequestId)
    {
        var visits = await _visitRepository.GetByBudgetRequestIdAsync(budgetRequestId);
        var result = new List<VisitResponseDTO>();
        foreach (var visit in visits)
        {
            result.Add(await ConvertToResponseDTOAsync(visit));
        }
        return result;
    }

    public async Task<VisitResponseDTO> UpdateVisitStatusAsync(long id, Visit.VisitStatus newStatus)
    {
        var visit = await FindVisitByIdAsync(id);
        ValidateStatusTransition(visit.Status, newStatus);
        visit.Status = newStatus;

        if (newStatus == Visit.VisitStatus.COMPLETED)
        {
            await _budgetRequestService.UpdateBudgetRequestStatusAsync(
                visit.BudgetRequestId,
                BudgetRequest.BudgetStatus.WAITING_BIDS
            );
        }
        else if (newStatus == Visit.VisitStatus.CANCELLED)
        {
            await _budgetRequestService.UpdateBudgetRequestStatusAsync(
                visit.BudgetRequestId,
                BudgetRequest.BudgetStatus.OPEN
            );
        }

        var updatedVisit = await _visitRepository.UpdateAsync(visit);
        return await ConvertToResponseDTOAsync(updatedVisit);
    }

    public async Task DeleteVisitAsync(long id)
    {
        var visit = await FindVisitByIdAsync(id);
        if (visit.Status != Visit.VisitStatus.SCHEDULED)
        {
            throw new BusinessException("Não é possível excluir uma visita que já foi realizada ou cancelada");
        }
        await _visitRepository.DeleteAsync(visit);
    }

    private async Task<bool> HasConflictingVisitAsync(long sellerId, DateTime scheduledDate)
    {
        var startWindow = scheduledDate.AddHours(-2);
        var endWindow = scheduledDate.AddHours(2);

        var conflictingVisits = await _visitRepository.GetBySellerIdAndScheduledDateBetweenAsync(
            sellerId,
            startWindow,
            endWindow
        );

        return conflictingVisits.Any();
    }

    private void ValidateStatusTransition(Visit.VisitStatus currentStatus, Visit.VisitStatus newStatus)
    {
        if (currentStatus == Visit.VisitStatus.COMPLETED || currentStatus == Visit.VisitStatus.CANCELLED)
        {
            throw new BusinessException("Não é possível alterar o status de uma visita finalizada ou cancelada");
        }

        if (newStatus == Visit.VisitStatus.SCHEDULED && currentStatus != Visit.VisitStatus.SCHEDULED)
        {
            throw new BusinessException("Não é possível voltar o status para agendado");
        }
    }

    private async Task<Visit> FindVisitByIdAsync(long id)
    {
        var visit = await _visitRepository.GetByIdAsync(id);
        if (visit == null)
        {
            throw new ResourceNotFoundException("Visita", "id", id);
        }
        return visit;
    }

    private async Task<VisitResponseDTO> ConvertToResponseDTOAsync(Visit visit)
    {
        return new VisitResponseDTO
        {
            Id = visit.Id,
            Seller = await _userService.GetUserByIdAsync(visit.SellerId),
            BudgetRequest = await _budgetRequestService.GetBudgetRequestByIdAsync(visit.BudgetRequestId),
            ScheduledDate = visit.ScheduledDate,
            Status = visit.Status,
            Notes = visit.Notes,
            Photos = visit.Photos,
            CreatedAt = visit.CreatedAt,
            UpdatedAt = visit.UpdatedAt
        };
    }
}

