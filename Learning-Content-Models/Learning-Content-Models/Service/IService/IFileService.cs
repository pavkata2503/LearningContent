namespace Learning_Content_Models.Service.IService
{
	public interface IFileService
	{
		Tuple<int, string> SaveImage(IFormFile imageFile);
		public bool DeleteImage(string imageFileName);
	}
}
