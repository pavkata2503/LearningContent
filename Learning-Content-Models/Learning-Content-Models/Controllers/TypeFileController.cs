using Humanizer.Localisation;
using Learning_Content_Models.Data;
using Learning_Content_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Learning_Content_Models.Controllers
{
    public class TypeFileController : Controller
    {
        private readonly ApplicationDbContext context;

        public TypeFileController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var typeFiles = context.TypeFiles
            .Include(m => m.StudyMaterials).ToList();

            return View(typeFiles);
        }
        public IActionResult Add()
        {
            ViewBag.StudyMaterials = context.StudyMaterials.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Add(TypeFile typeFiles)
        {
            context.TypeFiles.Add(typeFiles);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Update Genre
        public IActionResult Edit(int id)
        {
            var typeFiles = context.TypeFiles
                .Include(m => m.StudyMaterials)
                .FirstOrDefault(m => m.Id == id);
            if (typeFiles == null)
            {
                return NotFound();
            }

            ViewBag.StudyMaterials = context.StudyMaterials.ToList();

            return View(typeFiles);
        }

        [HttpPost]
        public IActionResult Edit(TypeFile typeFiles)
        {
            if (ModelState.IsValid)
            {
                context.TypeFiles.Update(typeFiles);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeFiles);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var typeFile = context.TypeFiles.Find(id);

            if (typeFile == null)
            {
                return NotFound();
            }

            context.TypeFiles.Remove(typeFile);
            context.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
