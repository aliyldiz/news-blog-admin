using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsBlogAdmin.Models;

namespace NewsBlogAdmin.Controllers;

public class HomeController : Controller
{
    private readonly BlogContext _context;
    
    public HomeController(BlogContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Login(string Email, string Password)
    {
        var author = _context.Author.FirstOrDefault(w => w.Email == Email && w.Password == Password);
        if (author == null)
        {
            return RedirectToAction("Index");    
        }
        HttpContext.Session.SetInt32("id", author.Id);
        
        return RedirectToAction("Index", controllerName:"Category");
    }
    
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
