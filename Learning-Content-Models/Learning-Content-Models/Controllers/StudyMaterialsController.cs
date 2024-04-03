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
using System.Drawing.Printing;
using Learning_Content_Models.Service;
using Learning_Content_Models.Service.IService;
using Learning_Content_Models.Models.Enums;

namespace Learning_Content_Models.Controllers
{
	[Authorize]
	public class StudyMaterialsController : Controller
	{
		private readonly ApplicationDbContext context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IFileService _fileService;

		public StudyMaterialsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IFileService fileService)
		{
			this.context = context;
			this._userManager = userManager;
			this._fileService = fileService;
		}
		//Pagination Working
		//public IActionResult Index(int? pageSize, int? pageNumber)
		//{
		//    var materials = context.StudyMaterials.AsQueryable();

		//    // Pagination
		//    pageSize = pageSize ?? 6; // Default page size is 5
		//    pageNumber = pageNumber ?? 1; // Default page number is 1
		//    ViewBag.PageSize = pageSize.Value;
		//    ViewBag.CurrentPage = pageNumber.Value;
		//    ViewBag.TotalPages = (int)Math.Ceiling((double)materials.Count() / pageSize.Value);

		//    var paginatedMaterials = materials.Skip((pageNumber.Value - 1) * pageSize.Value)
		//                                     .Take(pageSize.Value);



		//    // Pass the paginated materials to the view
		//    return View(paginatedMaterials.ToList());
		//}
		public IActionResult Index(string sortOrder, int? pageSize, int? pageNumber, string createdName, string category, string typefile, string subject, string classFilter, string search)
		{
			var materials = context.StudyMaterials.AsQueryable();

			ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";

			// Apply sorting
			switch (sortOrder)
			{
				case "date_desc":
					materials = materials.OrderByDescending(s => s.CreateDate);
					break;
				default:
					materials = materials.OrderBy(s => s.CreateDate);
					break;
			}

			// Apply filters
			if (!string.IsNullOrEmpty(createdName))
			{
				materials = materials.Where(m => m.CreatedByName == createdName);
			}
			if (!string.IsNullOrEmpty(search))
			{
				materials = materials.Where(m => m.Title.Contains(search) || m.Description.Contains(search));
			}

			if (!string.IsNullOrEmpty(category) && Enum.TryParse<Category>(category, out var parsedCategory))
			{
				materials = materials.Where(m => m.Category == parsedCategory);
			}

			if (!string.IsNullOrEmpty(typefile) && Enum.TryParse<TypeFile>(typefile, out var parsedTypeFile))
			{
				materials = materials.Where(m => m.TypeFile == parsedTypeFile);
			}

			if (!string.IsNullOrEmpty(subject))
			{
				materials = materials.Where(m => m.Subject == subject);
			}

			if (!string.IsNullOrEmpty(classFilter))
			{
				materials = materials.Where(m => (int)m.Class == int.Parse(classFilter));
			}

			// Pagination
			//pageSize = pageSize ?? 6; // Default page size is 6
			//pageNumber = pageNumber ?? 1; // Default page number is 1
			//ViewBag.PageSize = pageSize.Value;
			//ViewBag.CurrentPage = pageNumber.Value;
			//ViewBag.TotalPages = (int)Math.Ceiling((double)materials.Count() / pageSize.Value);

			//var paginatedMaterials = materials.Skip((pageNumber.Value - 1) * pageSize.Value)
			//								  .Take(pageSize.Value);

			//// Pass the paginated materials to the view
			//return View(paginatedMaterials.ToList());



			pageSize = pageSize ?? 6; // Default page size is 6
			pageNumber = pageNumber ?? 1; // Default page number is 1
			ViewBag.PageSize = pageSize.Value;
			ViewBag.CurrentPage = pageNumber.Value;
			ViewBag.TotalPages = (int)Math.Ceiling((double)materials.Count() / pageSize.Value);

			// Set the available page size options
			ViewBag.PageSizeOptions = new List<int> { 5, 10, 15, 20 };

			var paginatedMaterials = materials.Skip((pageNumber.Value - 1) * pageSize.Value)
											  .Take(pageSize.Value);
			System.Diagnostics.Debug.WriteLine($"pageSize: {pageSize}");
			//Pass the paginated materials to the view
			return View(paginatedMaterials.ToList());
		}

		[Authorize(Roles = "Teacher")]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(StudyMaterial studyMaterial)
		{

			var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				throw new ArgumentException("Invalid user.");
			}

			var user = await _userManager.FindByIdAsync(userId);

			studyMaterial.CreatedByName = user.Name;

			if (studyMaterial.FileUpload != null)
			{
				var fileResult = _fileService.SaveImage(studyMaterial.FileUpload);
				if (fileResult.Item1 == 1)
				{
					studyMaterial.FileTitle = studyMaterial.Title;
					studyMaterial.FileTitle = fileResult.Item2;
				}
				else
				{
					ModelState.AddModelError(string.Empty, fileResult.Item2);
					return View(studyMaterial);
				}
			}
			if (ModelState.IsValid)
			{
				context.StudyMaterials.Add(studyMaterial);
				context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View("Add", studyMaterial);

			//return View(studyMaterial);
		}
		[Authorize(Roles = "Teacher")]
		public IActionResult Edit(int id)
		{
			var studyMaterial = context.StudyMaterials
				.FirstOrDefault(m => m.Id == id);
			if (studyMaterial == null)
			{
				return NotFound();
			}
			//ViewBag.User = User.Identity.Name.ToList();

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
			if (studyMaterial.FileUpload != null)
			{
				var fileResult = _fileService.SaveImage(studyMaterial.FileUpload);
				if (fileResult.Item1 == 1)
				{
					studyMaterial.FileTitle = studyMaterial.Title;
					studyMaterial.FileTitle = fileResult.Item2;
				}
				else
				{
					ModelState.AddModelError(string.Empty, fileResult.Item2);
					return View(studyMaterial);
				}
			}
			else
			{
				var existingMaterial = context.StudyMaterials.AsNoTracking().FirstOrDefault(m => m.Id == studyMaterial.Id);
				if (existingMaterial != null)
				{
					studyMaterial.FileTitle = existingMaterial.FileTitle;
				}
			}
			if (ModelState.IsValid)
			{

				context.StudyMaterials.Update(studyMaterial);
				context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View("Edit", studyMaterial);
		}

		[HttpPost]
		[Authorize(Roles = "Teacher")]
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
		public IActionResult Details(int id)
		{
			var studyMaterial = context.StudyMaterials.Find(id);

			return View(studyMaterial);
		}
		public IActionResult Ascending()
		{
			var materials = context.StudyMaterials.OrderBy(s => s.CreateDate).ToList();
			return View("Index", materials);
		}

		// Action method to display study materials in descending order of creation date
		public IActionResult Descending()
		{
			var materials = context.StudyMaterials.OrderByDescending(s => s.CreateDate).ToList();
			return View("Index", materials);
		}
	}
}
