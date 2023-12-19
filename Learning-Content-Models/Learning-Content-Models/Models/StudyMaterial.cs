using Humanizer.Localisation;
using Learning_Content_Models.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Learning_Content_Models.Models
{
	public class StudyMaterial
	{
		[Key]
		public int Id { get; set; }
		public string Title { get; set; }
		public string URL { get; set; }	
		public string Description { get; set; }

        [EnumDataType(typeof(Category))]
        public Category Category { get; set; }

        [EnumDataType(typeof(TypeFile))]
        public TypeFile TypeFile { get; set; }
        public string Subject { get; set; }
        [Range(1, 12)]
        public int Class { get; set; }
		public DateTime CreateDate { get; set; }

		// New property to store the name of the user who created the study material
		public string CreatedByName { get; set; }
		//Upload File
		[NotMapped]
		public IFormFile FileUpload { get; set; }

    }
}
