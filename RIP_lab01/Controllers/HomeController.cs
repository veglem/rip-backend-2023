using System.Net;
using Microsoft.AspNetCore.Mvc;
using RIP_lab01.Database;
using RIP_lab01.Models;

namespace RIP_lab01.Controllers;

public class HomeController : Controller
{
    // GET
    
    public IActionResult Index(string? id)
    {
        
        //нет конкретного id
        if (id is null)
        {
            List<UniversityDivisions>? divisionsList;
            
            // поиск
            if (HttpContext.Request.Query.Keys.Contains("search"))
            {
                divisionsList = JsonDB.AsyncRead().Result?.
                    FindAll(divisions => divisions.StructName.ToLower().
                        Contains(HttpContext.Request.Query["search"].ToString().ToLower()));
            }
            // вся база
            else
            {
                divisionsList = JsonDB.AsyncRead().Result;
            }
            
            
            return View(divisionsList);
        }
        
        // поиск по id
        UniversityDivisions div = JsonDB.AsyncRead().Result?.
            Find(divisions => divisions.Id.ToString() == id) 
                                  ?? new UniversityDivisions("null");
        return View("Division", div);
        

        
    }
}
