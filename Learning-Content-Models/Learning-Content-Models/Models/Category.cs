using System.ComponentModel.DataAnnotations;

namespace Learning_Content_Models.Models
{
	public class Category
	{
		[Key] 
		public int Id { get; set; }
		public string Name { get; set; }
		public List<StudyMaterial> StudyMaterials { get; set; }

		//public List<string> Names = new List<string>() 
		//{ "New Material", "Exercise", "Homework", "For testing" };
	}
}
