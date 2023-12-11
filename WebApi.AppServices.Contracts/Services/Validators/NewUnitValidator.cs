using WebApi.AppServices.Contracts.Models.Request;

namespace WebApi.AppServices.Contracts.Services.Validators;

public class NewUnitValidator
{
    public static void ValidateUnitToAdd(NewUnit unit)
    {
        if (unit.Name is null ||
            unit.IsDeleted is null ||
            unit.ImgUrl is null ||
            unit.Name == "")
        {
            throw new ValidationException(
                "Заполните все поля для добавления нового подразделения");
        }
    }
    
}
