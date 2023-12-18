using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.AppServices.Contracts.Services.Convertors;
using WebApi.DataAccess;

namespace WebApi.AppServices.Handlers;

public class OrdersHandler : IOrdersHandler
{
    private IOrdersRepository _ordersRepository;
    private IUnitRepository _unitRepository;

    public OrdersHandler(IOrdersRepository repository, IUnitRepository unitRepository)
    {
        _ordersRepository = repository;
        _unitRepository = unitRepository;
    }

    public async Task<List<GetUnitResult>> AddUnitToOrder(
        int unitId,
        string username,
        CancellationToken cancellationToken)
    {
        return await _ordersRepository.AddUnitToOrder(username, unitId, cancellationToken);
    }

    public async Task<List<GetOrderResult>> GetUserOrders(string username, CancellationToken cancellationToken)
    {
        return (await _ordersRepository.GetOrdersByUser(username,
                cancellationToken))
            .Select(GetOrderResultConvertor.FromDomainModel)
            .ToList();

    }

    public async Task<GetOrderResult> GetOrderById(string username, int orderId,
        CancellationToken cancellationToken)
    {
        return await _ordersRepository.GetOrderById(username, orderId,
            cancellationToken);
    }

    public async Task<GetOrderResult> UpdateOrder(int orderId, string username, UpdateOrderRequest request,
        CancellationToken cancellationToken)
    {
        return await _ordersRepository.UpdateOrder(orderId, username, request, cancellationToken);
    }

    public async Task<GetOrderResult> UpdateStatusUser(int orderId, string username, string status,
        CancellationToken cancellationToken)
    {
        return await _ordersRepository.UpdateStatusUser(orderId, username, status,
            cancellationToken);
    }

    public async Task<GetOrderResult> DeleteUnitFromOrder(int orderId, int unitId, string username,
        CancellationToken cancellationToken)
    {
        return await _ordersRepository.DeleteUnitFromOrder(orderId, unitId, username, cancellationToken);
    }
}
