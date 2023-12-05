using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.AppServices.Models;

namespace WebApi.AppServices.Handlers;

public class UnitHandler : IUnitHandler
{
    private IUnitRepository _unitRepository;

    public UnitHandler(IUnitRepository unitRepository)
    {
        _unitRepository = unitRepository;
    }
    
    public async Task<List<GetUnitResult>> GetUnits(CancellationToken cancellationToken)
    {
        return await _unitRepository.GetAllUnits(cancellationToken);
    }
}
