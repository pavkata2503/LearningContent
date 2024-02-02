using Microsoft.AspNetCore.Identity;

namespace Learning_Content_Models.Models
{
	public class ApplicationUser: IdentityUser
	{
		public string? Name { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Description { get; set; }
		public List<Message> Messages { get; set; }
	}
}
