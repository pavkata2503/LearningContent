using Learning_Content_Models.Data;
using Learning_Content_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Learning_Content_Models.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }   
        public IActionResult Index()
        {
            List<ApplicationUser> users = _userManager.Users.ToList();

            return View(users);
        }

        // Action to show a form to create a new user
        public IActionResult CreateUser()
        {
            // Populate a dropdown with roles (e.g., Cashier, User)
            ViewBag.Roles = _roleManager.Roles.Where(r => r.Name != Roles.Admin.ToString()).ToList();
            return View();
        }

        // Action to handle form submission to create a new user
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, Name = model.Name };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Add the selected role to the user
                    var role = await _roleManager.FindByIdAsync(model.RoleId);
                    if (role != null)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }

                    return RedirectToAction("Index", "Home"); // Redirect to a suitable location
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If ModelState is not valid, redisplay the form
            ViewBag.Roles = _roleManager.Roles.Where(r => r.Name != Roles.Admin.ToString()).ToList();
            return View(model);
        }


        [HttpPost]
        public IActionResult Delete(string id)
        {
            var user = _dbContext.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
