using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;

namespace Learning_Content_Models.Models
{
	public class Message
	{
		[Key] 
		public int Id { get; set; }
		public string Text {  get; set; }

		public int ApplicationUserId { get; set; }

		public ApplicationUser ApplicationUser { get; set; }

		//public string Sender { get; set; }

		//public int SenderId { get; set; }
		//public Sender Sender { get; set; }
		public string Receiver {  get; set; }

		//public int ReceiverId { get; set; }

		//public Receiver Receiver { get; set; }
		public DateTime SendDate { get; set; }
		public string Status { get ; set; }
	}
}
