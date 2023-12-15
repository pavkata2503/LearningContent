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
		public IActionResult Index(int? pageSize, int? pageNumber, string createdName, string category, string typefile, string subject, string classFilter)
		{
			var materials = context.StudyMaterials.AsQueryable();

			// Apply filters
			if (!string.IsNullOrEmpty(createdName))
			{
				materials = materials.Where(m => m.CreatedByName == createdName);
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
			pageSize = pageSize ?? 6; // Default page size is 6
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

			//if (Input.ImageFile != null)
			//{
			//	var result = _fileService.SaveImage(Input.ImageFile);
			//	if (result.Item1 == 1)
			//	{
			//		var oldImage = user.ProfilePicture;
			//		user.ProfilePicture = result.Item2;
			//		await _userManager.UpdateAsync(user);
			//		var deleteUser = _fileService.DeleteImage(oldImage);
			//	}
			//}

			//await _signInManager.RefreshSignInAsync(user);
			//StatusMessage = "Your profile has been updated";
			if (studyMaterial.FileUpload!=null)
			{
				var fileResult = _fileService.SaveImage(studyMaterial.FileUpload);
				//if (fileResult.Item1 == 1)
				//{
				//	studyMaterial.Title = fileResult.Item2;
				//}
				//else
				//{
				//	ModelState.AddModelError(string.Empty, fileResult.Item2);
				//	return View(studyMaterial);
				//}
			}

			context.StudyMaterials.Add(studyMaterial);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
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
