using Humanizer;
using Learning_Content_Models.Data;
using Learning_Content_Models.Models;
using Learning_Content_Models.Service;
using Learning_Content_Models.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System;
using System.Drawing.Printing;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Learning_Content_Models.Controllers
{
	[Authorize]
	public class MessagesController : Controller
	{
		private readonly ApplicationDbContext context;
		public readonly UserManager<ApplicationUser> _userManager;

		public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			this.context = context;
			this._userManager = userManager;
		}

		public async Task<IActionResult> Index(string TextMessage, bool? isRead,string receiver, int? pageSize, int? pageNumber)
		{
			var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				throw new ArgumentException("Invalid user.");
			}
			var user = await _userManager.FindByIdAsync(userId);

			var messages = context.Messages.Where(m => m.Receiver == user.Email);
			
			if (!string.IsNullOrEmpty(receiver))
			{
				messages = messages.Where(m => m.Sender == receiver);
			}
			
			if (isRead.HasValue)
			{
				messages = messages.Where(m => m.IsRead == isRead.Value);
			}

			
			messages = messages.OrderByDescending(o => o.SendDate);
			pageSize = pageSize ?? 5; // Default page size is 5
			pageNumber = pageNumber ?? 1; // Default page number is 1
			ViewBag.PageSize = pageSize.Value;
			ViewBag.CurrentPage = pageNumber.Value;
			ViewBag.TotalPages = (int)Math.Ceiling((double)messages.Count() / pageSize.Value);

			
			ViewBag.PageSizeOptions = new List<int> { 5, 10, 15, 20 };

			var paginatedMassages = messages.Skip((pageNumber.Value - 1) * pageSize.Value)
											  .Take(pageSize.Value);
			System.Diagnostics.Debug.WriteLine($"pageSize: {pageSize}");
			
			return View(paginatedMassages.ToList());
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(Message message)
		{
			var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				throw new ArgumentException("Invalid user.");
			}
			var user = await _userManager.FindByIdAsync(userId);
			message.Sender = user.Name;
			message.SendDate = DateTime.Now;
			message.SenderEmail = User.Identity.Name;
			message.IsRead = false; 
			ModelState.Remove("SenderEmail");
			ModelState.Remove("Sender");
			ModelState.Remove("ApplicationUser");
			if (ModelState.IsValid) {

				context.Messages.Add(message);
				context.SaveChanges();
				return RedirectToAction("Index");
			}
			return View("Add", message);
		}

		[HttpPost]
		public IActionResult Delete(int id)
		{
			var message = context.Messages.Find(id);

			if (message == null)
			{
				return NotFound();
			}

			context.Messages.Remove(message);
			context.SaveChanges();
			return RedirectToAction("Index");
		}

		[HttpPost]
		public IActionResult MarkAsRead(int id)
		{
			var message = context.Messages.Find(id);

			if (message == null)
			{
				return NotFound();
			}

			message.IsRead = true; // Маркиране на съобщението като прочетено
			context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}
