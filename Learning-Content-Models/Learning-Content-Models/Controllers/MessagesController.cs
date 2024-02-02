using Learning_Content_Models.Data;
using Learning_Content_Models.Models;
using Learning_Content_Models.Service;
using Learning_Content_Models.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Learning_Content_Models.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext context;
        public readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this._userManager = userManager;
        }

        public IActionResult Index(string TextMessage)
        {
			var messages = context.Messages
			.ToList();
			// Retrieve messages from the database
			//var messages = context.Messages.AsQueryable();
			if (!string.IsNullOrEmpty(TextMessage))
			{
				// Filter messages by the search term
				messages = messages.Where(m => m.Text.Contains(TextMessage)).ToList();
			}
			// Order messages by send date in descending order (newest to oldest)
			messages = messages.OrderBy(o => o.SendDate).ToList();
			//messages.First().SearchQuery = TextMessage;

			return View(messages);
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
            message.SendDate= DateTime.Now;
            message.SenderEmail= User.Identity.Name;
			message.IsRead = false; // Задаване на начален статус "непрочетено"

			context.Messages.Add(message);
            context.SaveChanges();
            return RedirectToAction("Index");
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
