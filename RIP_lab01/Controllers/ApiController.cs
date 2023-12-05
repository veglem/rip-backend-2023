using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RIP_lab01.Controllers;

/// <summary>
/// 
/// </summary>
[Route("[controller]")]
public class ApiController : Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("checkDb")]
    public async Task<ICollection<UnivesityUnit>> CheckDb()
    {
        RectorOrdersDatabaseContext db = new RectorOrdersDatabaseContext();


        List<UnivesityUnit> units = db.UnivesityUnits.ToList();

        List<UniversityEmployee> employees =
            await db.UniversityEmployees.ToListAsync();

        return units;
    }

    [HttpPost("page/{id:int}/delete")]
    public async Task<IActionResult> SetDeletedStatusToUnit([FromRoute] int id)
    {
        RectorOrdersDatabaseContext db = new RectorOrdersDatabaseContext();

        int res = await db.Database.ExecuteSqlRawAsync(
            "UPDATE \"UnivesityUnit\" SET \"IsDeleted\" = true WHERE \"Id\" = {0};", id);

        if (res == 0)
        {
            return NotFound();
        }

        // await db.SaveChangesAsync();

        return Redirect("/page");
    }
}
