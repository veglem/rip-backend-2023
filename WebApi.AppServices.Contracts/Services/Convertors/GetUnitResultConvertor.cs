using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Models;

namespace WebApi.AppServices.Contracts.Services.Convertors;

public class GetUnitResultConvertor
{
    public static GetUnitResult FromDomaiModel(UnivesityUnit unit)
    {
        return new GetUnitResult()
        {
            Id = unit.Id,
            ImgUrl = unit.ImgUrl,
            Name = unit.Name,
            IsDeleted = unit.IsDeleted,
            ParrentUnitNavigation = unit.ParrentUnitNavigation is null
                ? null
                : FromDomaiModel(unit.ParrentUnitNavigation),
            ParrentUnit = unit.ParrentUnit,
            InverseParrentUnitNavigation = unit.InverseParrentUnitNavigation
                .Select(FromDomaiModel).ToList()
        };
    }
}
