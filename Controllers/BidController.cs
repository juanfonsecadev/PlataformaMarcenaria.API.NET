using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlataformaMarcenaria.API.DTOs.Bid;
using PlataformaMarcenaria.API.Services;

namespace PlataformaMarcenaria.API.Controllers;

[ApiController]
[Route("api/bids")]
[Authorize]
public class BidController : ControllerBase
{
    private readonly IBidService _bidService;

    public BidController(IBidService bidService)
    {
        _bidService = bidService;
    }

    [HttpPost]
    public async Task<ActionResult<BidResponseDTO>> CreateBid([FromBody] BidCreateDTO createDTO)
    {
        var bid = await _bidService.CreateBidAsync(createDTO);
        return CreatedAtAction(nameof(GetBidById), new { id = bid.Id }, bid);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BidResponseDTO>> GetBidById(long id)
    {
        var bid = await _bidService.GetBidByIdAsync(id);
        return Ok(bid);
    }

    [HttpGet("carpenter/{carpenterId}")]
    public async Task<ActionResult<IEnumerable<BidResponseDTO>>> GetBidsByCarpenterId(long carpenterId)
    {
        var bids = await _bidService.GetBidsByCarpenterIdAsync(carpenterId);
        return Ok(bids);
    }

    [HttpGet("budget-request/{budgetRequestId}")]
    public async Task<ActionResult<IEnumerable<BidResponseDTO>>> GetBidsByBudgetRequestId(long budgetRequestId)
    {
        var bids = await _bidService.GetBidsByBudgetRequestIdAsync(budgetRequestId);
        return Ok(bids);
    }

    [HttpPost("{id}/accept")]
    public async Task<ActionResult<BidResponseDTO>> AcceptBid(long id)
    {
        var bid = await _bidService.AcceptBidAsync(id);
        return Ok(bid);
    }

    [HttpPost("{id}/reject")]
    public async Task<ActionResult<BidResponseDTO>> RejectBid(long id)
    {
        var bid = await _bidService.RejectBidAsync(id);
        return Ok(bid);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBid(long id)
    {
        await _bidService.DeleteBidAsync(id);
        return NoContent();
    }
}

