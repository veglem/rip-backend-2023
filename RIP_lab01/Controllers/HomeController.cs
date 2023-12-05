using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RIP_lab01.Controllers;

[Route("page")]
public class HomeController : Controller
{
    /// <summary>
    /// Создание продукта
    /// </summary>
    /// <param name="model">Продукт</param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id?}")]
    public IActionResult Index(int? id)
    {
        RectorOrdersDatabaseContext db = new RectorOrdersDatabaseContext();
        
        //нет конкретного id
        if (id is null)
        {
            
            List<UnivesityUnit>? univesityUnits;
            
            // поиск
            if (HttpContext.Request.Query.Keys.Contains("search"))
            {
                univesityUnits = db.UnivesityUnits
                    .Include(u => u.UniversityEmployees)
                    .Where(unit => unit.Name.ToLower()
                        .Contains(HttpContext.Request.Query["search"]
                            .ToString().ToLower()) && !unit.IsDeleted).ToList();
                    
                
            }
            // вся база
            else
            {
                univesityUnits = db.UnivesityUnits
                    .Include(u => u.UniversityEmployees)
                    .Where(unit => !unit.IsDeleted).ToList();
            }
            
            
            return View(univesityUnits);
        }
        
        // поиск по id

        UnivesityUnit? unit = db.UnivesityUnits.Find(id);
        
        if (unit is null)
        {
            return NotFound();
        }

        db.Entry(unit).Collection(u => u.UniversityEmployees).Load();
        
        return View("Division", unit);
        

        
    }
}
