using Microsoft.AspNetCore.Mvc;
using NewsBlogAdmin.Filter;
using NewsBlogAdmin.Models;

namespace NewsBlogAdmin.Controllers;

[UserFilter]
public class CategoryController : Controller
{
    private readonly BlogContext _context;
    
    public CategoryController(BlogContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> AddCategory(Category category)
    {
        if (category.Id == 0)
        {
            await _context.AddAsync(category);    
        }
        else
        {
            _context.Update(category);
        }
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> DeleteCategory(int id)
    {
        Category category = await _context.Category.FindAsync(id);
        _context.Remove(category);
        await _context.SaveChangesAsync();
        
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> CategoryDetails(int id)
    {
        var category = await _context.Category.FindAsync(id);
        return Json(category);
    }
    
    public IActionResult Index()
    {
        List<Category> list = _context.Category.ToList();
        return View(list);
    }
}