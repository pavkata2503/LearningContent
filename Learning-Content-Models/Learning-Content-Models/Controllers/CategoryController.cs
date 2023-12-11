using Humanizer.Localisation;
using Learning_Content_Models.Data;
using Learning_Content_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learning_Content_Models.Controllers
{
    public class CategoryController : Controller
    {
            private readonly ApplicationDbContext context;

            public CategoryController(ApplicationDbContext context)
            {
                this.context = context;
            }

            public IActionResult Index()
            {
                var categories = context.Categories
                .Include(m => m.StudyMaterials).ToList();

                return View(categories);
            }
            public IActionResult Add()
            {
                ViewBag.StudyMaterials = context.StudyMaterials.ToList();

                return View();
            }

            [HttpPost]
            public IActionResult Add(Category category)
            {
                context.Categories.Add(category);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            //Update Genre
            public IActionResult Edit(int id)
            {
                var categories = context.Categories
                    .Include(m => m.StudyMaterials)
                    .FirstOrDefault(m => m.Id == id);
                if (categories == null)
                {
                    return NotFound();
                }

                ViewBag.StudyMaterials = context.StudyMaterials.ToList();

                return View(categories);
            }

            [HttpPost]
            public IActionResult Edit(Category categories)
            {
                if (ModelState.IsValid)
                {
                    context.Categories.Update(categories);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(categories);
            }

            [HttpPost]
            public IActionResult Delete(int id)
            {
                var categories = context.Categories.Find(id);

                if (categories == null)
                {
                    return NotFound();
                }

                context.Categories.Remove(categories);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
}
