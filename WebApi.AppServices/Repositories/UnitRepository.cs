using Microsoft.EntityFrameworkCore;
using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.AppServices.Contracts.Services.Convertors;
using WebApi.DataAccess;

namespace WebApi.AppServices.Repositories;

public class UnitRepository : IUnitRepository
{
    private RectorOrdersDatabaseContext _context;

    public UnitRepository(RectorOrdersDatabaseContext context)
    {
        _context = context;
    }
    
    public async Task<List<GetUnitResult>> GetAllUnits(
        string filter,
        CancellationToken cancellationToken)
    {
        List<UnivesityUnit> fromBase = await _context.UnivesityUnits
            .Where(unit => !unit.IsDeleted && 
                unit.Name.Contains(filter) || 
                unit.Description != null && unit.Description.Contains(filter)
            )
            .ToListAsync(cancellationToken);
        
        
        
        return fromBase
            .Select(u => GetUnitResultConvertor.FromDomainModel(u))
            .Where(u => u.ParrentUnit is null || !fromBase.Exists(un => un.Id == u.ParrentUnit))
            .ToList();
    }

    public async Task<GetUnitResult?> GetUnitById(
        CancellationToken cancellationToken, int id)
    {
        UnivesityUnit? unit = await _context.UnivesityUnits.FindAsync(id);

        if (unit is null)
        {
            return null;
        }

        return GetUnitResultConvertor.FromDomainModel(unit);
    }

    public async Task<int> AddUnit(CancellationToken cancellationToken, NewUnit unit)
    {
        var newUnit = await
            _context.UnivesityUnits.AddAsync(
                NewUnitConvertor.ToDomainModel(unit),
                cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return newUnit.Entity.Id;
    }

    public async Task UpdateUnit(CancellationToken cancellationToken, int id,
        NewUnit unit)
    {
        UnivesityUnit? unitToUpdate =
            await _context.UnivesityUnits.FindAsync(id);

        if (unitToUpdate is null)
        {
            throw new ArgumentNullException(
                "подразделения с таким id не существует");
        }

        unitToUpdate.Id = id;

        if (unit.Name is not null && unit.Name != String.Empty)
        {
            unitToUpdate.Name = unit.Name;
        }

        if (unit.Description is not null && unit.Description != String.Empty)
        {
            unitToUpdate.Description = unit.Description;
        }

        if (unit.IsDeleted is not null)
        {
            unitToUpdate.IsDeleted = unit.IsDeleted.Value;
        }

        if (unit.ImgUrl is not null)
        {
            unitToUpdate.ImgUrl = unit.ImgUrl;
        }

        unitToUpdate.ParrentUnit = unit.ParrentUnit;

        List<int?> parrentKeys = await _context.UnivesityUnits
            .Select(u => u.ParrentUnit).ToListAsync(cancellationToken);

        if (!parrentKeys.Contains(unit.ParrentUnit))
        {
            throw new ArgumentNullException(
                "родительского подразделения с таким id не существует");
        }
        
        _context.Update(unitToUpdate);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<GetUnitResult>> GetAllUnitsWithDeleted(CancellationToken cancellationToken)
    {
        List<UnivesityUnit> fromBase = await _context.UnivesityUnits
            .ToListAsync(cancellationToken);
        
        
        
        return fromBase
            .Select(u => GetUnitResultConvertor.FromDomainModel(u))
            .Where(u => u.ParrentUnit is null || !fromBase.Exists(un => un.Id == u.ParrentUnit))
            .ToList();
    }

    public async Task LogicUnitDelete(CancellationToken cancellationToken, int id)
    {
        UnivesityUnit unit = await _context.UnivesityUnits.FindAsync(id);

        if (unit is null)
        {
            throw new ArgumentNullException(
                "подразделения с таким id не существует");
        }

        unit.IsDeleted = true;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
