using PlataformaMarcenaria.API.DTOs.Visit;
using PlataformaMarcenaria.API.Entities;

namespace PlataformaMarcenaria.API.Services;

public interface IVisitService
{
    Task<VisitResponseDTO> CreateVisitAsync(VisitCreateDTO createDTO);
    Task<VisitResponseDTO> GetVisitByIdAsync(long id);
    Task<IEnumerable<VisitResponseDTO>> GetVisitsBySellerIdAsync(long sellerId);
    Task<IEnumerable<VisitResponseDTO>> GetVisitsByBudgetRequestIdAsync(long budgetRequestId);
    Task<VisitResponseDTO> UpdateVisitStatusAsync(long id, Visit.VisitStatus status);
    Task DeleteVisitAsync(long id);
}

