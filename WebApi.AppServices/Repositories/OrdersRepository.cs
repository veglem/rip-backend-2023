using Microsoft.EntityFrameworkCore;
using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.AppServices.Contracts.Services.Convertors;
using WebApi.DataAccess;

namespace WebApi.AppServices.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private RectorOrdersDatabaseContext _context;

    public OrdersRepository(RectorOrdersDatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<RectorOrder>> GetOrdersByUser(string username,
        CancellationToken cancellationToken)
    {
        User? user = _context.Users.AsNoTracking().FirstOrDefault(u => u.Username == username);
        
        if (user is not null && user.IsModerator)
        {
            var moderRes = await _context.RectorOrders
                .Where(order => order.Moderator != null && order.Moderator.Username == username)
                .Include(order => order.Requests)
                .Include(order => order.Status)
                .ToListAsync(cancellationToken);
        
            return moderRes;
        }
        
        var result = await _context.RectorOrders
            .Where(order => order.Creator.Username == username)
            .Include(order => order.Requests)
            .Include(order => order.Status)
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<RectorOrder> AddNewOrder(RectorOrder order,
        CancellationToken cancellationToken)
    {
        var e = await _context.RectorOrders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return e.Entity;
    }

    public async Task<List<GetUnitResult>> AddUnitToOrder(string username,
        int unitId,
        CancellationToken cancellationToken)
    {
        List<RectorOrder> orders = await _context.RectorOrders
            .Where(order => order.Creator.Username == username)
            .Include(rectorOrder => rectorOrder.Status)
            .Include(rectorOrder => rectorOrder.Requests)
            .ThenInclude(request => request.Unit)
            .ToListAsync(cancellationToken);

        RectorOrder? order =
            orders.FirstOrDefault(order => order.Status.Name == "draft");

        if (order is null)
        {
            var o = await _context.RectorOrders.AddAsync(new RectorOrder()
            {
                Creator =
                    _context.Users.FirstOrDefault(user =>
                        user.Username == username) ?? new User(),
                Name = "",
                OrderBody = "",
                Status =
                    _context.Statuses.FirstOrDefault(status =>
                        status.Name == "draft") ?? new Status(),
                CreationDate = DateTime.Now
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            order = o.Entity;
        }

        if (order.Requests.All(request => request.Unit.Id != unitId))
        {
            order.Requests.Add(new Request()
            {
                OrderId = order.Id,
                UnitId = unitId
            });

            UnivesityUnit? unit =
                await _context.UnivesityUnits.FirstOrDefaultAsync(
                    unit => unit.Id == unitId, cancellationToken);

            if (unit is null)
            {
                throw new ArgumentNullException(
                    $"Нет подразделения с id {unitId}");
            }

            order.Requests
                .FirstOrDefault(request => request.UnitId == unitId)
                .Unit = unit;
            
            _context.RectorOrders.Update(order);

            await _context.SaveChangesAsync(cancellationToken);
        }
        
        foreach (var request in order.Requests)
        {
            request.Unit.InverseParrentUnitNavigation = new List<UnivesityUnit>();
        }

        return order.Requests.Select(request =>
            GetUnitResultConvertor.FromDomainModel(request.Unit)).ToList();
    }

    public async Task<GetOrderResult> GetOrderById(string username, int orderId,
        CancellationToken cancellationToken)
    {
        var order =
            await _context.RectorOrders
                .Include(order => order.Requests)
                .Include(order => order.Status)
                .FirstOrDefaultAsync(order =>
                        order.Id == orderId &&
                        order.Creator.Username == username &&
                        order.Status.Name != "deleted",
                    cancellationToken);

        if (order is null)
        {
            throw new ArgumentNullException($"Нет приказа с id {orderId}");
        }
        
        return GetOrderResultConvertor.FromDomainModel(order);
    }

    public async Task<GetOrderResult> UpdateOrder(int orderId, string username, UpdateOrderRequest request,
        CancellationToken cancellationToken)
    {
        RectorOrder? oreder = await _context.RectorOrders
            .Include(o => o.Creator)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (oreder.Creator.Username != username && !oreder.Creator.IsModerator)
        {
            throw new ArgumentNullException($"Нет приказа с id {orderId}");
        }

        if (oreder is null)
        {
            throw new ArgumentNullException($"Нет приказа с id {orderId}");
        }

        if (request.Name is not null)
        {
            oreder.Name = request.Name;
        }

        if (request.Description is not null)
        {
            oreder.OrderBody = request.Description;
        }
        
        _context.RectorOrders.Update(oreder);
        await _context.SaveChangesAsync(cancellationToken);

        return GetOrderResultConvertor.FromDomainModel(oreder);
    }

    public async Task<GetOrderResult> UpdateStatusUser(int orderId, string username, string status,
        CancellationToken cancellationToken)
    {
        Status? statusToSet = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == status, cancellationToken);
        
        if (statusToSet is null)
        {
            throw new ArgumentNullException($"Нет статуса с именем {status}");
        }

        RectorOrder? order =
            await _context.RectorOrders.Include(o => o.Creator)
                .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order.Creator.Username != username && !order.Creator.IsModerator)
        {
            throw new ArgumentNullException($"Нет приказа с id {orderId}");
        }

        if (order is null)
        {
            throw new ArgumentNullException($"Нет приказа с id {orderId}");
        }

        order.Status = statusToSet;
        
        _context.RectorOrders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);

        return GetOrderResultConvertor.FromDomainModel(order);
    }

    public async Task<GetOrderResult> DeleteUnitFromOrder(int orderId, int unitId, string username,
        CancellationToken cancellationToken)
    {
        
        
        RectorOrder? order = await _context.RectorOrders
            .Where(o => o.Creator.Username == username || )
            .Include(o => o.Creator)
            .Include(o => o.Requests)
            .Include(o => o.Status)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order.Creator.Username != username && !order.Creator.IsModerator)
        {
            throw new ArgumentNullException($"Нет приказа с id {orderId}");
        }

        if (order is null)
        {
            throw new ArgumentNullException($"Нет приказа с id {orderId}");
        }

        Request? request = await _context.Requests.FirstOrDefaultAsync(r =>
            r.OrderId == orderId && r.UnitId == unitId, cancellationToken: cancellationToken);
        
        if (request is null)
        {
            throw new ArgumentNullException($"Нет приказа с id {orderId}");
        }

        _context.Requests.Remove(request);
        
        return GetOrderResultConvertor.FromDomainModel(order);
    }
}
