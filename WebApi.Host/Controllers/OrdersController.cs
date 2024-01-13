using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using ArgumentNullException = System.ArgumentNullException;

namespace WebApi.Controllers;

[Route("api/orders")]
public class OrdersController : Controller
{
    private IOrdersHandler _ordersHandler;

    public OrdersController(IOrdersHandler ordersHandler)
    {
        _ordersHandler = ordersHandler;
    }

    [HttpGet]
    [Authorize(Roles = "user, moderator")]
    public async Task<List<GetOrderResult>> GetAllOrders(CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        return await _ordersHandler.GetUserOrders(User.Identity.Name, cancellationToken);
    }

    [HttpGet("{orderId:int}")]
    public async Task<GetOrderResult> GetOrderById(int orderId,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        try
        {
            var result = await _ordersHandler.GetOrderById(User.Identity.Name,
                orderId, cancellationToken);

            return result;
        }
        catch (ArgumentNullException ex)
        {
            await Results.BadRequest(ex.Message).ExecuteAsync(HttpContext);
        }

        return null;
    }

    [HttpPut("{orderId:int}/update")]
    [Authorize(Roles = "user")]
    public async Task<GetOrderResult> UpdateOrder(
        [FromBody] UpdateOrderRequest request,
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }
        try
        {
            GetOrderResult order =
                await _ordersHandler.UpdateOrder(orderId, User.Identity.Name, request,
                    cancellationToken);

            return order;
        }
        catch (ArgumentNullException ex)
        {
            await Results.BadRequest(ex.Message).ExecuteAsync(HttpContext);
        }

        return null;
    }

    [HttpPut("{orderId:int}/update_status_user")]
    [Authorize(Roles = "user")]
    public async Task<GetOrderResult> UpdateStatusUser(
        [FromQuery] string status,
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }
        if (status != "draft" && status != "formed")
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
        }
        
        try
        {
            GetOrderResult order =
                await _ordersHandler.UpdateStatusUser(orderId, User.Identity.Name, status,
                    cancellationToken);

            return order;
        }
        catch (ArgumentNullException ex)
        {
            Results.BadRequest(ex.Message);
        }

        return null;
    }
    
    [HttpPut("{orderId:int}/update_status_moderator")]
    [Authorize(Roles = "moderator")]
    public async Task<GetOrderResult> UpdateStatusAdmin(
        [FromQuery] string status,
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }
        if (status != "completed" && status != "rejected")
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
        }
        
        try
        {
            GetOrderResult order =
                await _ordersHandler.UpdateStatusUser(orderId, User.Identity.Name, status,
                    cancellationToken);

            return order;
        }
        catch (ArgumentNullException ex)
        {
            Results.BadRequest(ex.Message);
        }

        return null;
    }
    
    [HttpDelete("{orderId:int}/delete")]
    [Authorize(Roles = "user, moderator")]
    public async Task<GetOrderResult> DeleteOder(
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }
        
        try
        {
            GetOrderResult order =
                await _ordersHandler.UpdateStatusUser(orderId, User.Identity.Name, "deleted",
                    cancellationToken);

            return order;
        }
        catch (ArgumentNullException ex)
        {
            Results.BadRequest(ex.Message);
        }

        return null;
    }

    [HttpDelete("{orderId:int}/delete_unit/{unitId}")]
    [Authorize(Roles = "user, moderator")]
    public async Task<GetOrderResult> DeleteUnitFromOrder(
        [FromRoute] int orderId,
        [FromRoute] int unitId,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }
        
        try
        {
            GetOrderResult order =
                await _ordersHandler.DeleteUnitFromOrder(orderId, unitId, User.Identity.Name,
                    cancellationToken);

            return order;
        }
        catch (ArgumentNullException ex)
        {
            Results.BadRequest(ex.Message);
        }

        return null;
    }
}
