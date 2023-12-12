using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.AppServices.Models;
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
}
