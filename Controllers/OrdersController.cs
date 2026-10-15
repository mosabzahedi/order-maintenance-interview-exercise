using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderMaintenance.Contracts;
using OrderMaintenance.Services;

namespace OrderMaintenance.Controllers;

[Authorize]
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly OrderService service;

    public OrdersController(OrderService service)
    {
        this.service = service;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderView>> Get(int id)
    {
        var order = await service.GetAsync(id);
        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpPut("{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult<OrderView>> Update(int id, UpdateOrderRequest request)
    {
        // The demo cookie identifies who is saving the order.
        var actorClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var actorId = int.Parse(actorClaim);

        var order = await service.UpdateAsync(id, request, actorId);
        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }
}
