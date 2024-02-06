using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.AppServices.Exceptions;

namespace WebApi.Controllers;

[Route("api/orders")]
public class OrdersController : Controller
{
    private IOrdersHandler _ordersHandler;

    private readonly IDistributedCache _cache;

    public OrdersController(IOrdersHandler ordersHandler,
        IDistributedCache cache)
    {
        _ordersHandler = ordersHandler;
        _cache = cache;
    }

    /// <summary>
    /// Возвращает все приказы пользователя
    /// </summary>
    /// <param name="endDate"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="status"></param>
    /// <param name="startDate"></param>
    /// <returns></returns>
    /// <response code="200">Успешное получение приказов</response>
    /// <response code="403">Пользователь не авторизован</response>
    [HttpGet]
    [Authorize(Roles = "user, moderator")]
    public async Task<List<GetOrderResult>> GetAllOrders(
        [FromQuery] int? status,
        [FromQuery] DateOnly? startDate,
        [FromQuery] DateOnly? endDate,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        if (await _cache.GetStringAsync(
                HttpContext.Request.Cookies[".AspNetCore.Cookies"] ??
                string.Empty, cancellationToken) is not null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        List<GetOrderResult> result =
            (await _ordersHandler.GetUserOrders(User.Identity.Name,
                cancellationToken)).Where(order => order.Status != 1).ToList();

        if (status == -1)
        {
            status = null;
        }

        if (status is not null)
        {
            result = result.Where(order => order.Status == status).ToList();
        }

        if (startDate is not null)
        {
            result = result.Where(order =>
                DateOnly.FromDateTime(order.FormationDate ??
                                      DateTime.MinValue) >= startDate).ToList();
        }

        if (endDate is not null)
        {
            result = result.Where(order =>
                DateOnly.FromDateTime(order.FormationDate ??
                                      DateTime.MaxValue) <= endDate).ToList();
        }

        return result;
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

        if (await _cache.GetStringAsync(
                HttpContext.Request.Cookies[".AspNetCore.Cookies"] ??
                string.Empty, cancellationToken) is not null)
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
            await Results.BadRequest(new { Message = ex.Message })
                .ExecuteAsync(HttpContext);
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
            if (await _cache.GetStringAsync(
                    HttpContext.Request.Cookies[".AspNetCore.Cookies"] ??
                    string.Empty, cancellationToken) is not null)
            {
                await Results.Forbid().ExecuteAsync(HttpContext);
                return null;
            }

            GetOrderResult order =
                await _ordersHandler.UpdateOrder(orderId, User.Identity.Name,
                    request,
                    cancellationToken);

            return order;
        }
        catch (ResultException ex)
        {
            await Results.BadRequest(new { Message = ex.Message })
                .ExecuteAsync(HttpContext);
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
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        if (await _cache.GetStringAsync(
                HttpContext.Request.Cookies[".AspNetCore.Cookies"] ??
                string.Empty, cancellationToken) is not null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        try
        {
            HttpClient client = new HttpClient();
            var resp = await client.PostAsJsonAsync(
                "http://192.168.43.44:8080/calc_sig/",
                new
                {
                    orderid = orderId
                }, cancellationToken);
            Console.WriteLine(
                $"{resp.StatusCode} | {await resp.Content.ReadAsStringAsync()}");
        }
        catch
        {
            
        }
        
        try
        {
            GetOrderResult order =
                await _ordersHandler.UpdateStatusUser(orderId,
                    User.Identity.Name, "formed",
                    cancellationToken);

            return order;
        }
        catch (ResultException ex)
        {
            Results.BadRequest(new { Message = ex.Message });
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
        [FromQuery] int status,
        [FromRoute] int orderId,
        CancellationToken cancellationToken)
    {
        if (User.Identity is null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        if (await _cache.GetStringAsync(
                HttpContext.Request.Cookies[".AspNetCore.Cookies"] ??
                string.Empty, cancellationToken) is not null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        string strStatus = "";

        if (status != 3 && status != 4)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
        }

        switch (status)
        {
            case 3:
                strStatus = "completed";
                break;
            case 4:
                strStatus = "rejected";
                break;
        }

        try
        {
            GetOrderResult order =
                await _ordersHandler.UpdateStatusUser(orderId,
                    User.Identity.Name, strStatus,
                    cancellationToken);

            return order;
        }
        catch (ResultException ex)
        {
            Results.BadRequest(new { Message = ex.Message });
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

        if (await _cache.GetStringAsync(
                HttpContext.Request.Cookies[".AspNetCore.Cookies"] ??
                string.Empty, cancellationToken) is not null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        try
        {
            GetOrderResult order =
                await _ordersHandler.UpdateStatusUser(orderId,
                    User.Identity.Name, "deleted",
                    cancellationToken);

            return order;
        }
        catch (ResultException ex)
        {
            Results.BadRequest(new { Message = ex.Message });
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

        if (await _cache.GetStringAsync(
                HttpContext.Request.Cookies[".AspNetCore.Cookies"] ??
                string.Empty, cancellationToken) is not null)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return null;
        }

        try
        {
            GetOrderResult order =
                await _ordersHandler.DeleteUnitFromOrder(orderId, unitId,
                    User.Identity.Name,
                    cancellationToken);

            return order;
        }
        catch (ResultException ex)
        {
            Results.BadRequest(new { Message = ex.Message });
        }

        return null;
    }

    [HttpPut("{orderId}/update_signature/")]
    public async Task AddSignature([FromRoute] int orderId,
    [FromBody] SignatureRequest request)
    {
        if (request.AccessToken != 123)
        {
            await Results.Forbid().ExecuteAsync(HttpContext);
            return;
        }

        await _ordersHandler.UpdateSignature(orderId, request.Signature);
    }
}
