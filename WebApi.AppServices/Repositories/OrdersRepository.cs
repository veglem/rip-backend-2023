using Microsoft.EntityFrameworkCore;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.AppServices.Contracts.Services.Convertors;
using WebApi.AppServices.Models;
using WebApi.DataAccess;

namespace WebApi.AppServices.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private RectorOrdersDatabaseContext _context;

    public OrdersRepository(RectorOrdersDatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<RectorOrder>> GetOrdersByUser(string username, CancellationToken cancellationToken)
    {
        return await _context.RectorOrders.Where(order => order.Creator.Username == username)
            .ToListAsync(cancellationToken);
    }

    public async Task<RectorOrder> AddNewOrder(RectorOrder order, CancellationToken cancellationToken)
    {
        var e = await _context.RectorOrders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return e.Entity;
    }

    public async Task<List<GetUnitResult>> AddUnitToOrder(string username, int unitId,
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

            _context.RectorOrders.Update(order);

            await _context.SaveChangesAsync(cancellationToken);
        }
        
        _context.RectorOrders.Entry(order).Collections

        return order.Requests.Select(request => GetUnitResultConvertor.FromDomaiModel(request.Unit)).ToList();
    }
}
