using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RIP_lab01.Database;
using RIP_lab01.Models;

namespace RIP_lab01.Controllers;


/// <summary>
/// 
/// </summary>
[Route("[controller]")]
public class ApiController : Controller
{
    // [HttpGet("dbMigrate")]
    // public IActionResult AddDataToDb()
    // {
    //     List<UniversityDivisions>? divisionsList = JsonDB.AsyncRead().Result;
    //
    //     RectorOrdersDatabaseContext db = new RectorOrdersDatabaseContext();
    //
    //     if (divisionsList != null)
    //     {
    //         db.UnivesityUnits.AddRange(
    //                         divisionsList.Select(divs => divs.ToDbModel()));
    //         db.SaveChanges();
    //     }
    //     else
    //     {
    //         return BadRequest();
    //     }
    //     
    //
    //     return Ok();
    // }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet("checkDb")]
    public async Task<ICollection<UnivesityUnit>> CheckDb()
    {
        
        RectorOrdersDatabaseContext db = new RectorOrdersDatabaseContext();


        List<UnivesityUnit> units = db.UnivesityUnits.ToList();

        List<UniversityEmployee> employees = await db.UniversityEmployees.ToListAsync();

        // foreach (var unit in units)
        // {
        //     unit.UniversityEmployees =
        //         employees.Where(employee => employee.UnitId == unit.Id).ToList();
        // }
        

        return units;
    }

    [HttpPost("page/{id:int}/delete")]
    public async Task<IActionResult> SetDeletedStatusToUnit([FromRoute] int id)
    {
        RectorOrdersDatabaseContext db = new RectorOrdersDatabaseContext();

        UnivesityUnit? unit = await db.UnivesityUnits.FindAsync(id);

        if (unit is null)
        {
            return NotFound();
        }

        unit.IsDeleted = true;

        await db.SaveChangesAsync();

        return Redirect("/page");
    }
}
