using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.AppServices.Exceptions;

namespace WebApi.Controllers;

[Route("api/orders")]
public class OrdersController : Controller
{
    private IOrdersHandler _ordersHandler;

    public OrdersController(IOrdersHandler ordersHandler)
    {
        _ordersHandler = ordersHandler;
    }

    /// <summary>
    /// Возвращает все приказы пользователя
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Успешное получение приказов</response>
    /// <response code="403">Пользователь не авторизован</response>
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

    /// <summary>
    /// Получение приказа по id
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Успешное получение приказа</response>
    /// <response code="400">Ошибка получения приказа</response>
    /// <response code="403">Пользователь не авторизован</response>
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
        catch (ResultException ex)
        {
            await Results.BadRequest(new { Message = ex.Message}).ExecuteAsync(HttpContext);
        }

        return null;
    }

    /// <summary>
    /// Обновление приказа
    /// </summary>
    /// <param name="request"></param>
    /// <param name="orderId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Успешное обновление приказа</response>
    /// <response code="400">Ошибка обновления</response>
    /// <response code="403">Пользователь не авторизован</response>
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
        catch (ResultException ex)
        {
            await Results.BadRequest(new { Message = ex.Message}).ExecuteAsync(HttpContext);
        }

        return null;
    }

    /// <summary>
    /// Обновление статуса пользователем
    /// </summary>
    /// <param name="status"></param>
    /// <param name="orderId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Успешное добавление подразделения</response>
    /// <response code="400">Ошибка обновления</response>
    /// <response code="403">Пользователь не авторизован или пытается установить не верный статус</response>
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
        catch (ResultException ex)
        {
            Results.BadRequest(new { Message = ex.Message});
        }

        return null;
    }
    
    /// <summary>
    /// Обновление статуса приказа модератором
    /// </summary>
    /// <param name="status"></param>
    /// <param name="orderId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Успешное обновление статуса</response>
    /// <response code="400">Ошибка обновления</response>
    /// <response code="403">Пользователь не авторизован или пытается установить не верный статус</response>
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
        catch (ResultException ex)
        {
            Results.BadRequest(new { Message = ex.Message});
        }

        return null;
    }
    
    /// <summary>
    /// Удаление приказа
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Успешное удаление приказа</response>
    /// <response code="400">Ошибка удаления</response>
    /// <response code="403">Пользователь не авторизован</response>
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
        catch (ResultException ex)
        {
            Results.BadRequest(new { Message = ex.Message});
        }

        return null;
    }

    /// <summary>
    /// Удаление подразделения из приказа
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="unitId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <response code="200">Успешное удаление</response>
    /// <response code="400">Ошибка удаления</response>
    /// <response code="403">Пользователь не авторизован</response>
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
        catch (ResultException ex)
        {
            Results.BadRequest(new { Message = ex.Message});
        }

        return null;
    }
}
