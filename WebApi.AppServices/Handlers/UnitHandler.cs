using WebApi.AppServices.Contracts.Handlers;
using WebApi.AppServices.Contracts.Models.Request;
using WebApi.AppServices.Contracts.Models.Responce;
using WebApi.AppServices.Contracts.Repositories;
using WebApi.AppServices.Contracts.Services.Validators;

namespace WebApi.AppServices.Handlers;

public class UnitHandler : IUnitHandler
{
    private IUnitRepository _unitRepository;
    private IS3Repository _s3Repository;

    public UnitHandler(IUnitRepository unitRepository, IS3Repository s3Repository)
    {
        _unitRepository = unitRepository;
        _s3Repository = s3Repository;
    }
    
    public async Task<List<GetUnitResult>> GetUnits(
        string filter,
        CancellationToken cancellationToken)
    {
        return await _unitRepository.GetAllUnits(filter, cancellationToken);
    }

    public async Task<GetUnitResult?> GetUnitById(
        CancellationToken cancellationToken, int id)
    {
        return await _unitRepository.GetUnitById(cancellationToken, id);
    }

    public async Task<int> AddNewUnit(CancellationToken cancellationToken, NewUnit unit)
    {
        // todo: Add default ImgUrl
        unit.ImgUrl = "";
        
        NewUnitValidator.ValidateUnitToAdd(unit);

        int id = await _unitRepository.AddUnit(cancellationToken, unit);

        return id;
    }

    public async Task UpdateUnit(CancellationToken cancellationToken, int id, NewUnit unit)
    {
        await _unitRepository.UpdateUnit(cancellationToken, id, unit);
    }

    public async Task<List<GetUnitResult>> GetUnitsWithDeleted(CancellationToken cancellationToken)
    {
        return await _unitRepository.GetAllUnitsWithDeleted(cancellationToken);
    }

    public async Task UnitLogicDelete(CancellationToken cancellationToken, int id)
    {
        await _unitRepository.LogicUnitDelete(cancellationToken, id);
    }

    public async Task AddImage(Stream image, int id, CancellationToken cancellationToken)
    {
        var unit = await _unitRepository.GetUnitById(cancellationToken, id);

        if (unit is null)
        {
            throw new ArgumentNullException("нет подразделения с таким id");
        }
        
        await _s3Repository.AddImage(image, unit.Name, cancellationToken);

        NewUnit newUnit = new NewUnit()
        {
            ImgUrl = "images/" + unit.Name + ".jpg",
            ParrentUnit = unit.ParrentUnit
        };

        await _unitRepository.UpdateUnit(cancellationToken, id, newUnit);
    }

    // public async Task<byte[]> GetImage(int id, CancellationToken cancellationToken)
    // {
    //     var unit = await _unitRepository.GetUnitById(cancellationToken, id);
    //     
    //     
    // }
}
