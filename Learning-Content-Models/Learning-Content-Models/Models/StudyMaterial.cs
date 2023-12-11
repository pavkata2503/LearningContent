using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;

namespace Learning_Content_Models.Models
{
	public class StudyMaterial
	{
		[Key]
		public int Id { get; set; }
		public string Title { get; set; }
		public string URL { get; set; }	
		public string Description { get; set; }

		//public string Category {  get; set; }

		public int CategoryId { get; set; }
		public Category Category { get; set; }

		//public string Type { get; set; }
		public int TypeFileId { get; set; }
		public TypeFile TypeFile { get; set; }
		public string Subject { get; set; }
		public int Class { get; set; }
		public DateTime CreateDate { get; set; }

		// New property to store the name of the user who created the study material
		public string CreatedByName { get; set; }

	}
}
