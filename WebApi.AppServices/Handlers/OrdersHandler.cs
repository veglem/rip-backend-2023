using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Repositories;

namespace WebApi.AppServices.Handlers;

public class OrdersHandler : IOrdersHandler
{
    private IOrdersRepository _ordersRepository;

    public OrdersHandler(IOrdersRepository repository)
    {
        _ordersRepository = repository;
    }
}
