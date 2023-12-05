using Microsoft.EntityFrameworkCore;
using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.AppServices.Contracts.Services.Convertors;
using WebApi.AppServices.Models;
using WebApi.DataAccess;

namespace WebApi.AppServices.Repositories;

public class UnitRepository : IUnitRepository
{
    private RectorOrdersDatabaseContext _context;

    public UnitRepository(RectorOrdersDatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<List<GetUnitResult>> GetAllUnits(CancellationToken cancellationToken)
    {
        List<GetUnitResult> fromBase = await _context.UnivesityUnits
            .AsNoTracking()
            .Where(unit => !unit.IsDeleted)
            
            .Select(unit => GetUnitResultConvertor.FromDomaiModel(unit))
            .ToListAsync(cancellationToken);

        return fromBase;
    }
}
