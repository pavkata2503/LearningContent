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
using Microsoft.IdentityModel.Tokens;

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
		//public IActionResult Index(string searchString, string authorFilter, string categoryFilter, string typeFilter, string subjectFilter, int classFilter, string sortOrder)
		//{
		//	// Get all study materials
		//	var materials = context.StudyMaterials.ToList();

		//	// Apply search filter
		//	if (!string.IsNullOrEmpty(searchString))
		//	{
		//		materials = materials.Where(m => m.Title.Contains(searchString) || m.Description.Contains(searchString)).ToList();
		//	}

		//	// Apply other filters
		//	if (!string.IsNullOrEmpty(authorFilter))
		//	{
		//		materials = materials.Where(m => m.CreatedByName == authorFilter).ToList();
		//	}

		//	if (!string.IsNullOrEmpty(categoryFilter))
		//	{
		//		materials = materials.Where(m => Convert.ToString(m.Category) == categoryFilter).ToList();
		//	}

		//	if (!string.IsNullOrEmpty(typeFilter))
		//	{
		//		materials = materials.Where(m => Convert.ToString(m.Category) == typeFilter).ToList();
		//	}

		//	if (!string.IsNullOrEmpty(subjectFilter))
		//	{
		//		materials = materials.Where(m => m.Subject == subjectFilter).ToList();
		//	}

		//	if (!string.IsNullOrEmpty(Convert.ToString(classFilter)))
		//	{
		//		materials = materials.Where(m => m.Class == classFilter).ToList();
		//	}

		//	// Apply sorting
		//	switch (sortOrder)
		//	{
		//		case "Title":
		//			materials = materials.OrderBy(m => m.Title).ToList();
		//			break;
		//		case "Date":
		//			materials = materials.OrderBy(m => m.CreateDate).ToList();
		//			break;
		//		// Add more cases for other sorting options if needed
		//		default:
		//			break;
		//	}

		//	return View(materials);
		//}

		//public IActionResult Index(string searchString, string authorFilter, string categoryFilter, string subjectFilter, int classFilter, string sortOrder)
		//{
		//	var materials = context.StudyMaterials.AsQueryable();

		//	// Apply filters
		//	if (!string.IsNullOrEmpty(searchString))
		//	{
		//		materials = materials.Where(m =>
		//			m.Title.Contains(searchString) ||
		//			m.Description.Contains(searchString) ||
		//			m.CreatedByName.Contains(searchString));
		//	}

		//	if (!string.IsNullOrEmpty(authorFilter))
		//	{
		//		materials = materials.Where(m => m.CreatedByName == authorFilter);
		//	}

		//	if (!string.IsNullOrEmpty(categoryFilter))
		//	{
		//		materials = materials.Where(m => Convert.ToString(m.Category) == categoryFilter);
		//	}

		//	if (!string.IsNullOrEmpty(subjectFilter))
		//	{
		//		materials = materials.Where(m => m.Subject == subjectFilter);
		//	}
		//	if (!string.IsNullOrEmpty(Convert.ToString(classFilter)))
		//	{
		//		materials = materials.Where(m => m.Class == classFilter);
		//	}

		//	// Apply sorting
		//	switch (sortOrder)
		//	{
		//		case "title_desc":
		//			materials = materials.OrderByDescending(m => m.Title);
		//			break;
		//		case "date_asc":
		//			materials = materials.OrderBy(m => m.CreateDate);
		//			break;
		//		// Add more sorting options as needed
		//		default:
		//			materials = materials.OrderBy(m => m.Title);
		//			break;
		//	}

		//	return View(materials.ToList());
		//}

		//Pagination Working
		public IActionResult Index(int? pageSize, int? pageNumber)
		{
			var materials = context.StudyMaterials.AsQueryable();

			// Pagination
			pageSize = pageSize ?? 6; // Default page size is 5
			pageNumber = pageNumber ?? 1; // Default page number is 1
			ViewBag.PageSize = pageSize.Value;
			ViewBag.CurrentPage = pageNumber.Value;
			ViewBag.TotalPages = (int)Math.Ceiling((double)materials.Count() / pageSize.Value);

			var paginatedMaterials = materials.Skip((pageNumber.Value - 1) * pageSize.Value)
											 .Take(pageSize.Value);

			// Pass the paginated materials to the view
			return View(paginatedMaterials.ToList());
		}



		//public IActionResult Index()
		//      {
		//          var materials = context.StudyMaterials
		//          .ToList();

		//          return View(materials);
		//      }
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
