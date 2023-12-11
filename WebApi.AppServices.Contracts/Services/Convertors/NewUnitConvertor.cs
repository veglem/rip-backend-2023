using WebApi.AppServices.Contracts.Models;
using WebApi.AppServices.Contracts.Models.Request;

namespace WebApi.AppServices.Contracts.Services.Convertors;

public class NewUnitConvertor
{
    public static UnivesityUnit ToDomainModel(NewUnit unit)
    {
        return new UnivesityUnit()
        {
            Id = 0,
            Name = unit.Name ?? "",
            Description = unit.Description,
            IsDeleted = unit.IsDeleted ?? false,
            // todo: insert default ImgUrl
            ImgUrl = unit.ImgUrl ?? "",
        };
    }
}
