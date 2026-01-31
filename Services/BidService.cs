using PlataformaMarcenaria.API.DTOs.Bid;
using PlataformaMarcenaria.API.Entities;
using PlataformaMarcenaria.API.Exceptions;
using PlataformaMarcenaria.API.Repositories;

namespace PlataformaMarcenaria.API.Services;

public class BidService : IBidService
{
    private readonly IBidRepository _bidRepository;
    private readonly IUserService _userService;
    private readonly IBudgetRequestService _budgetRequestService;

    public BidService(
        IBidRepository bidRepository,
        IUserService userService,
        IBudgetRequestService budgetRequestService)
    {
        _bidRepository = bidRepository;
        _userService = userService;
        _budgetRequestService = budgetRequestService;
    }

    public async Task<BidResponseDTO> CreateBidAsync(BidCreateDTO createDTO)
    {
        var carpenter = await _userService.FindUserByIdAsync(createDTO.CarpenterId);
        if (carpenter.UserType != UserType.CARPENTER)
        {
            throw new BusinessException("Apenas marceneiros podem enviar lances");
        }

        var budgetRequest = await _budgetRequestService.FindBudgetRequestByIdAsync(createDTO.BudgetRequestId);
        if (budgetRequest.Status != BudgetRequest.BudgetStatus.WAITING_BIDS)
        {
            throw new BusinessException("Este orçamento não está aceitando lances no momento");
        }

        // Verificar se o marceneiro já deu um lance para este orçamento
        if (await HasExistingBidAsync(carpenter.Id, budgetRequest.Id))
        {
            throw new BusinessException("Você já enviou um lance para este orçamento");
        }

        var bid = new Bid
        {
            CarpenterId = createDTO.CarpenterId,
            BudgetRequestId = createDTO.BudgetRequestId,
            Price = createDTO.Price,
            ExecutionTimeInDays = createDTO.ExecutionTimeInDays,
            Description = createDTO.Description,
            Status = Bid.BidStatus.PENDING
        };

        var savedBid = await _bidRepository.CreateAsync(bid);
        return await ConvertToResponseDTOAsync(savedBid);
    }

    public async Task<BidResponseDTO> GetBidByIdAsync(long id)
    {
        var bid = await FindBidByIdAsync(id);
        return await ConvertToResponseDTOAsync(bid);
    }

    public async Task<IEnumerable<BidResponseDTO>> GetBidsByCarpenterIdAsync(long carpenterId)
    {
        var bids = await _bidRepository.GetByCarpenterIdAsync(carpenterId);
        var result = new List<BidResponseDTO>();
        foreach (var bid in bids)
        {
            result.Add(await ConvertToResponseDTOAsync(bid));
        }
        return result;
    }

    public async Task<IEnumerable<BidResponseDTO>> GetBidsByBudgetRequestIdAsync(long budgetRequestId)
    {
        var bids = await _bidRepository.GetByBudgetRequestIdAsync(budgetRequestId);
        var result = new List<BidResponseDTO>();
        foreach (var bid in bids)
        {
            result.Add(await ConvertToResponseDTOAsync(bid));
        }
        return result;
    }

    public async Task<BidResponseDTO> AcceptBidAsync(long id)
    {
        var bid = await FindBidByIdAsync(id);
        ValidateBidAcceptance(bid);

        bid.Status = Bid.BidStatus.ACCEPTED;
        var acceptedBid = await _bidRepository.UpdateAsync(bid);

        // Rejeitar outros lances
        await RejectOtherBidsAsync(bid.BudgetRequestId, bid.Id);

        // Atualizar status do orçamento
        await _budgetRequestService.UpdateBudgetRequestStatusAsync(
            bid.BudgetRequestId,
            BudgetRequest.BudgetStatus.CLOSED
        );

        return await ConvertToResponseDTOAsync(acceptedBid);
    }

    public async Task<BidResponseDTO> RejectBidAsync(long id)
    {
        var bid = await FindBidByIdAsync(id);
        ValidateBidRejection(bid);

        bid.Status = Bid.BidStatus.REJECTED;
        var rejectedBid = await _bidRepository.UpdateAsync(bid);
        return await ConvertToResponseDTOAsync(rejectedBid);
    }

    public async Task DeleteBidAsync(long id)
    {
        var bid = await FindBidByIdAsync(id);
        if (bid.Status != Bid.BidStatus.PENDING)
        {
            throw new BusinessException("Não é possível excluir um lance que já foi aceito ou rejeitado");
        }
        await _bidRepository.DeleteAsync(bid);
    }

    private async Task<bool> HasExistingBidAsync(long carpenterId, long budgetRequestId)
    {
        var pendingBids = await _bidRepository.GetByBudgetRequestIdAndStatusAsync(budgetRequestId, Bid.BidStatus.PENDING);
        return pendingBids.Any(b => b.CarpenterId == carpenterId);
    }

    private void ValidateBidAcceptance(Bid bid)
    {
        if (bid.Status != Bid.BidStatus.PENDING)
        {
            throw new BusinessException("Este lance não está mais pendente");
        }

        var budgetRequest = _budgetRequestService.FindBudgetRequestByIdAsync(bid.BudgetRequestId).Result;
        if (budgetRequest.Status != BudgetRequest.BudgetStatus.WAITING_BIDS)
        {
            throw new BusinessException("Este orçamento não está mais aceitando lances");
        }
    }

    private void ValidateBidRejection(Bid bid)
    {
        if (bid.Status != Bid.BidStatus.PENDING)
        {
            throw new BusinessException("Este lance não está mais pendente");
        }
    }

    private async Task RejectOtherBidsAsync(long budgetRequestId, long acceptedBidId)
    {
        var pendingBids = await _bidRepository.GetByBudgetRequestIdAndStatusAsync(budgetRequestId, Bid.BidStatus.PENDING);
        foreach (var bid in pendingBids.Where(b => b.Id != acceptedBidId))
        {
            bid.Status = Bid.BidStatus.REJECTED;
            await _bidRepository.UpdateAsync(bid);
        }
    }

    private async Task<Bid> FindBidByIdAsync(long id)
    {
        var bid = await _bidRepository.GetByIdAsync(id);
        if (bid == null)
        {
            throw new ResourceNotFoundException("Lance", "id", id);
        }
        return bid;
    }

    private async Task<BidResponseDTO> ConvertToResponseDTOAsync(Bid bid)
    {
        return new BidResponseDTO
        {
            Id = bid.Id,
            Carpenter = await _userService.GetUserByIdAsync(bid.CarpenterId),
            BudgetRequest = await _budgetRequestService.GetBudgetRequestByIdAsync(bid.BudgetRequestId),
            Price = bid.Price,
            ExecutionTimeInDays = bid.ExecutionTimeInDays,
            Description = bid.Description,
            Status = bid.Status,
            CreatedAt = bid.CreatedAt,
            UpdatedAt = bid.UpdatedAt
        };
    }
}

