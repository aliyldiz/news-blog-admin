using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsBlogAdmin.Filter;
using NewsBlogAdmin.Models;

namespace NewsBlogAdmin.Controllers;

[UserFilter]
public class BlogController : Controller
{
    private readonly BlogContext _context;
    
    public BlogController(BlogContext context)
    {
        _context = context;
    }
    
    public IActionResult Index()
    {
        var list = _context.Blog.ToList();
        return View(list);
    }

    public IActionResult Blog()
    {
        ViewBag.Categories = _context.Category.Select( w => new SelectListItem
        {
            Text = w.Name,
            Value = w.Id.ToString()
        }).ToList();
        
        return View();
    }
    
    public IActionResult Publish(int id)
    {
        var blog = _context.Blog.Find(id);
        blog.IsPublish = true;
        _context.Update(blog);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Save(Blog model)
    {
        if (model != null)
        {
            var file = Request.Form.Files.First();
            string savePath = Path.Combine("your path");

            var fileName = $"{DateTime.Now:MMddHHmmss}.{file.FileName.Split(".").Last()}";
            var fileUrl = Path.Combine(savePath, fileName);
            using (var fileStream = new FileStream(fileUrl, FileMode.Create)) 
            {
                await file.CopyToAsync(fileStream);
            }

            model.ImagePath = fileName;
            model.AuthorId = (int)HttpContext.Session.GetInt32("id");
            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
            return Json(true);
        } 

        return Json(false);
    }
}