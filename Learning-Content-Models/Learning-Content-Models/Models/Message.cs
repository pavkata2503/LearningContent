using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_Content_Models.Models
{
	public class Message
	{
		[Key] 
		public int Id { get; set; }
		[Required(ErrorMessage = "Текста е задължителен")]
		public string Text {  get; set; }
		[Required(ErrorMessage = "Получателят е задължителен")]
		public string Receiver {  get; set; }
		public string Sender {  get; set; }
		public string SenderEmail {  get; set; }
		public int AppliationUserId {  get; set; }
		public ApplicationUser ApplicationUser {  get; set; }
		public DateTime SendDate { get; set; }
		public bool IsRead { get; set; } // Поле за статус на прочитане
	}
}
