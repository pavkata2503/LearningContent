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

		//     public IActionResult Index(string TextMessage)
		//     {
		//var messages = context.Messages
		//.ToList();
		//// Retrieve messages from the database
		////var messages = context.Messages.AsQueryable();
		////if (!string.IsNullOrEmpty(TextMessage))
		////{
		////	// Filter messages by the search term
		////	messages = messages.Where(m => m.Text.Contains(TextMessage)).ToList();
		////}
		//// Order messages by send date in descending order (newest to oldest)
		//messages = messages.OrderByDescending(o => o.SendDate).ToList();
		////messages.First().SearchQuery = TextMessage;

		//return View(messages);
		//     }




		public async Task<IActionResult> Index(string TextMessage, bool? isRead,string receiver, int? pageSize, int? pageNumber)
		{
			//var messages = context.Messages.AsQueryable();
			var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				throw new ArgumentException("Invalid user.");
			}
			// Fetch user from UserManager
			var user = await _userManager.FindByIdAsync(userId);

			// Fetch messages for the current user
			var messages = context.Messages.Where(m => m.Receiver == user.Email);
			// Apply filters
			if (!string.IsNullOrEmpty(receiver))
			{
				messages = messages.Where(m => m.Sender == receiver);
			}
			// Filter messages by status (read or unread) if provided
			if (isRead.HasValue)
			{
				messages = messages.Where(m => m.IsRead == isRead.Value);
			}

			// Order messages by send date in descending order (newest to oldest)
			messages = messages.OrderByDescending(o => o.SendDate);
			pageSize = pageSize ?? 5; // Default page size is 5
			pageNumber = pageNumber ?? 1; // Default page number is 1
			ViewBag.PageSize = pageSize.Value;
			ViewBag.CurrentPage = pageNumber.Value;
			ViewBag.TotalPages = (int)Math.Ceiling((double)messages.Count() / pageSize.Value);

			// Set the available page size options
			ViewBag.PageSizeOptions = new List<int> { 5, 10, 15, 20 };

			var paginatedMassages = messages.Skip((pageNumber.Value - 1) * pageSize.Value)
											  .Take(pageSize.Value);
			System.Diagnostics.Debug.WriteLine($"pageSize: {pageSize}");
			//Pass the paginated materials to the view
			return View(paginatedMassages.ToList());
			//var messages2 = messages.Select(m => m.Receiver == user.Email);
			//return View(messages2.ToList());
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(Message message)
		{
			//This give the email of the user
			//ApplicationUser currentUser = await _userManager.GetUserAsync(User);
			//studyMaterial.CreatedByName = currentUser?.UserName;


			var userId = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				throw new ArgumentException("Invalid user.");
			}
			var user = await _userManager.FindByIdAsync(userId);
			message.Sender = user.Name;
			message.SendDate = DateTime.Now;
			message.SenderEmail = User.Identity.Name;
			message.IsRead = false; // Задаване на начален статус "непрочетено"
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
