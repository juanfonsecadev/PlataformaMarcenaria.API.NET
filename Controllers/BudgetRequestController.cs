using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaMarcenaria.API.DTOs.Budget;
using PlataformaMarcenaria.API.Entities;
using PlataformaMarcenaria.API.Services;

namespace PlataformaMarcenaria.API.Controllers;

[ApiController]
[Route("api/budget-requests")]
[Authorize]
public class BudgetRequestController : ControllerBase
{
    private readonly IBudgetRequestService _budgetRequestService;

    public BudgetRequestController(IBudgetRequestService budgetRequestService)
    {
        _budgetRequestService = budgetRequestService;
    }

    [HttpPost]
    public async Task<ActionResult<BudgetRequestResponseDTO>> CreateBudgetRequest([FromBody] BudgetRequestCreateDTO createDTO)
    {
        var budgetRequest = await _budgetRequestService.CreateBudgetRequestAsync(createDTO);
        return CreatedAtAction(nameof(GetBudgetRequestById), new { id = budgetRequest.Id }, budgetRequest);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BudgetRequestResponseDTO>> GetBudgetRequestById(long id)
    {
        var budgetRequest = await _budgetRequestService.GetBudgetRequestByIdAsync(id);
        return Ok(budgetRequest);
    }

    [HttpGet("client/{clientId}")]
    public async Task<ActionResult<IEnumerable<BudgetRequestResponseDTO>>> GetBudgetRequestsByClientId(long clientId)
    {
        var budgetRequests = await _budgetRequestService.GetBudgetRequestsByClientIdAsync(clientId);
        return Ok(budgetRequests);
    }

    [HttpGet("status/{status}")]
    public async Task<ActionResult<IEnumerable<BudgetRequestResponseDTO>>> GetBudgetRequestsByStatus(BudgetRequest.BudgetStatus status)
    {
        var budgetRequests = await _budgetRequestService.GetBudgetRequestsByStatusAsync(status);
        return Ok(budgetRequests);
    }

    [HttpGet("location")]
    public async Task<ActionResult<IEnumerable<BudgetRequestResponseDTO>>> GetBudgetRequestsByLocation(
        [FromQuery] string city,
        [FromQuery] string state)
    {
        var budgetRequests = await _budgetRequestService.GetBudgetRequestsByLocationAsync(city, state);
        return Ok(budgetRequests);
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<BudgetRequestResponseDTO>> UpdateBudgetRequestStatus(
        long id,
        [FromQuery] BudgetRequest.BudgetStatus status)
    {
        var budgetRequest = await _budgetRequestService.UpdateBudgetRequestStatusAsync(id, status);
        return Ok(budgetRequest);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBudgetRequest(long id)
    {
        await _budgetRequestService.DeleteBudgetRequestAsync(id);
        return NoContent();
    }
}

