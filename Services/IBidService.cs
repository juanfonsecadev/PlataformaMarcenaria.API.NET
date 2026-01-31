using PlataformaMarcenaria.API.DTOs.Bid;

namespace PlataformaMarcenaria.API.Services;

public interface IBidService
{
    Task<BidResponseDTO> CreateBidAsync(BidCreateDTO createDTO);
    Task<BidResponseDTO> GetBidByIdAsync(long id);
    Task<IEnumerable<BidResponseDTO>> GetBidsByCarpenterIdAsync(long carpenterId);
    Task<IEnumerable<BidResponseDTO>> GetBidsByBudgetRequestIdAsync(long budgetRequestId);
    Task<BidResponseDTO> AcceptBidAsync(long id);
    Task<BidResponseDTO> RejectBidAsync(long id);
    Task DeleteBidAsync(long id);
}

