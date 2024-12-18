using Microsoft.AspNetCore.Mvc;
using NewsBlogAdmin.Filter;
using NewsBlogAdmin.Models;

namespace NewsBlogAdmin.Controllers;

[UserFilter]
public class AuthorController : Controller
{
    private readonly BlogContext _context;

    public AuthorController(BlogContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> AddAuthor(Author author)
    {
        if (author.Id == 0)
        {
            await _context.AddAsync(author);    
        }
        else
        {
            _context.Update(author);
        }
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> AuthorDetails(int id)
    {
        var author = await _context.Author.FindAsync(id);
        return Json(author);
    }
    
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        Author author = await _context.Author.FindAsync(id);
        _context.Remove(author);
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
    
    public IActionResult Index()
    {
        List<Author> list = _context.Author.ToList();
        return View(list);
    }
}