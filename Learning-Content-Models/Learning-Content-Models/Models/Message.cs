using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Content_Models.Models
{
	public class Message
	{
		[Key] 
		public int Id { get; set; }
		public string Text {  get; set; }


		// Други свойства...
		//public string SenderId { get; set; }
		////Външен ключ
	 //  [ForeignKey("SenderId")]

		//public ApplicationUser ApplicationUser { get; set; }
		//public int SenderId { get; set; }
		//public ApplicationUser ApplicationUser { get; set; }

		//public string Sender { get; set; }

		//public int SenderId { get; set; }
		//public Sender Sender { get; set; }
		public string Receiver {  get; set; }
		public string Sender {  get; set; }
		public string SenderEmail {  get; set; }

		//public int ReceiverId { get; set; }

		//public Receiver Receiver { get; set; }
		public DateTime SendDate { get; set; }
		//public string Status { get ; set; }
		public bool IsRead { get; set; } // Добавено поле за статус на прочитане
	}
}
