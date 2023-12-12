using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Learning_Content_Models.Data;
using Learning_Content_Models.Models;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Learning_Content_Models.Controllers
{
    public class StudyMaterialsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudyMaterialsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this._userManager = userManager;
        }

        public IActionResult Index()
        {
            var materials = context.StudyMaterials
            .ToList();

            return View(materials);
        }
        //Add Movie
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(StudyMaterial studyMaterial)
        {
            //ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            //studyMaterial.CreatedByName = currentUser?.UserName;


            //string currentUser = _userManager.GetUserName(User);
            //studyMaterial.CreatedByName = currentUser; 


            //string currentUser = await _userManager.GetUserAsync(Name);
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ArgumentException("Invalid user.");
            }
           var user= await _userManager.FindByIdAsync(userId);
            studyMaterial.CreatedByName=user.Name;


            context.StudyMaterials.Add(studyMaterial);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        //Update Movie
        public IActionResult Edit(int id)
        {
            var studyMaterial = context.StudyMaterials
                .FirstOrDefault(m => m.Id == id);
            if (studyMaterial == null)
            {
                return NotFound();
            }
            ViewBag.User = User.Identity.Name.ToList();

            return View(studyMaterial);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudyMaterial studyMaterial)
        {
            var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ArgumentException("Invalid user.");
            }
            var user = await _userManager.FindByIdAsync(userId);
            studyMaterial.CreatedByName = user.Name;


            context.StudyMaterials.Update(studyMaterial);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var studyMaterial = context.StudyMaterials.Find(id);

            if (studyMaterial == null)
            {
                return NotFound();
            }

            context.StudyMaterials.Remove(studyMaterial);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
