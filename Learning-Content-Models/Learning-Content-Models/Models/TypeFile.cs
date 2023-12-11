using System.ComponentModel.DataAnnotations;

namespace Learning_Content_Models.Models
{
	public class TypeFile
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		public List<StudyMaterial> StudyMaterials { get; set; }

		//public List<string> Names = new List<string>()
		//{ "Text Document", "Image", "Type", "Diagram", "Presentation", "Audio", "Video"};

	}
}
