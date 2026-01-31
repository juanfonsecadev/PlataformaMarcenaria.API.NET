using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaMarcenaria.API.DTOs.Visit;
using PlataformaMarcenaria.API.Entities;
using PlataformaMarcenaria.API.Services;

namespace PlataformaMarcenaria.API.Controllers;

[ApiController]
[Route("api/visits")]
[Authorize]
public class VisitController : ControllerBase
{
    private readonly IVisitService _visitService;

    public VisitController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpPost]
    public async Task<ActionResult<VisitResponseDTO>> CreateVisit([FromBody] VisitCreateDTO createDTO)
    {
        var visit = await _visitService.CreateVisitAsync(createDTO);
        return CreatedAtAction(nameof(GetVisitById), new { id = visit.Id }, visit);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VisitResponseDTO>> GetVisitById(long id)
    {
        var visit = await _visitService.GetVisitByIdAsync(id);
        return Ok(visit);
    }

    [HttpGet("seller/{sellerId}")]
    public async Task<ActionResult<IEnumerable<VisitResponseDTO>>> GetVisitsBySellerId(long sellerId)
    {
        var visits = await _visitService.GetVisitsBySellerIdAsync(sellerId);
        return Ok(visits);
    }

    [HttpGet("budget-request/{budgetRequestId}")]
    public async Task<ActionResult<IEnumerable<VisitResponseDTO>>> GetVisitsByBudgetRequestId(long budgetRequestId)
    {
        var visits = await _visitService.GetVisitsByBudgetRequestIdAsync(budgetRequestId);
        return Ok(visits);
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<VisitResponseDTO>> UpdateVisitStatus(
        long id,
        [FromQuery] Visit.VisitStatus status)
    {
        var visit = await _visitService.UpdateVisitStatusAsync(id, status);
        return Ok(visit);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVisit(long id)
    {
        await _visitService.DeleteVisitAsync(id);
        return NoContent();
    }
}

