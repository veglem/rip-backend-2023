namespace WebApi.AppServices.Contracts.Models.Responce;

public class GetAllUnitsResult
{
    public List<GetUnitResult> Units { get; set; }
    
    public int? Draft { get; set; }
}
