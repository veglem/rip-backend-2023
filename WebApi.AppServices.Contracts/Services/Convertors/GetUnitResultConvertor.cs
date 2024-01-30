using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Responce;

namespace WebApi.AppServices.Contracts.Services.Convertors;

public class GetUnitResultConvertor
{
    public static GetUnitResult FromDomainModel(UnivesityUnit unit)
    {
        return new GetUnitResult()
        {
            Id = unit.Id,
            ImgUrl = unit.ImgUrl,
            Name = unit.Name,
            IsDeleted = unit.IsDeleted,
            // ParrentUnit = unit.ParrentUnit,
            Description = unit.Description,
            // InverseParrentUnitNavigation = unit.InverseParrentUnitNavigation.Select(FromDomainModel).ToList()
        };
    }
}
