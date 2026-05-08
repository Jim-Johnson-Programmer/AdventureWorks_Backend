using MediatR;
using Microsoft.AspNetCore.Mvc;
using AWMicroservices.SalesOrders.Application.SalesOrders.Queries;
using AWMicroservices.SalesOrders.Application.SalesOrders.Commands;

namespace AWMicroservices.SalesOrders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesOrdersController(IMediator mediator) : ControllerBase
{
  [HttpGet("{id:int}")]
  public async Task<IActionResult> GetById(int id, CancellationToken ct)
  {
    var salesOrder = await mediator.Send(new GetSalesOrderByIdQuery(id), ct);
    return salesOrder is null ? NotFound() : Ok(salesOrder);
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] CreateSalesOrderCommand command, CancellationToken ct)
  {
    var id = await mediator.Send(command, ct);
    return CreatedAtAction(nameof(GetById), new { id }, null);
  }
}