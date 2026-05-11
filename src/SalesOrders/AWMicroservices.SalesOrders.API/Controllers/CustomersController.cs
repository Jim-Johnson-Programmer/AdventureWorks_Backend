using MediatR;
using Microsoft.AspNetCore.Mvc;
using AWMicroservices.SalesOrders.Application.SalesOrders.DTOs;
using AWMicroservices.SalesOrders.Application.SalesOrders.Queries;
using AWMicroservices.SalesOrders.Application.SalesOrders.Commands;



namespace AWMicroservices.SalesOrders.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CustomersController : ControllerBase
{
  private readonly IMediator _mediator;
  private readonly ILogger<CustomersController> _logger;

  public CustomersController(IMediator mediator, ILogger<CustomersController> logger)
  {
    _mediator = mediator;
    _logger = logger;
  }


  [HttpGet]
  public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
  {
    _logger.LogInformation("Getting all customers");
    var result = await _mediator.Send(new GetAllCustomersQuery());
    return Ok(result);
  }


  [HttpGet("{id}")]
  public async Task<ActionResult<CustomerDto>> Get(int id)
  {
    _logger.LogInformation("Getting customer with id {CustomerId}", id);
    var result = await _mediator.Send(new GetCustomerByIdQuery(id));
    if (result is null)
    {
      _logger.LogWarning("Customer with id {CustomerId} not found", id);
      return NotFound();
    }
    return Ok(result);
  }


  [HttpPost]
  public async Task<ActionResult<int>> Create(CreateCustomerCommand command)
  {
    _logger.LogInformation("Creating new customer");
    var id = await _mediator.Send(command);
    _logger.LogInformation("Customer created with id {CustomerId}", id);
    return CreatedAtAction(nameof(Get), new { id }, id);
  }


  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, UpdateCustomerCommand command)
  {
    if (id != command.CustomerID)
    {
      _logger.LogWarning("Update failed: route id {RouteId} does not match command id {CommandId}", id, command.CustomerID);
      return BadRequest();
    }
    _logger.LogInformation("Updating customer with id {CustomerId}", id);
    await _mediator.Send(command);
    _logger.LogInformation("Customer with id {CustomerId} updated", id);
    return NoContent();
  }


  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    _logger.LogInformation("Deleting customer with id {CustomerId}", id);
    await _mediator.Send(new DeleteCustomerCommand(id));
    _logger.LogInformation("Customer with id {CustomerId} deleted", id);
    return NoContent();
  }
}
