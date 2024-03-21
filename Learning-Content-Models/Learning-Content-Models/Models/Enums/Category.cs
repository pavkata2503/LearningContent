using System.ComponentModel.DataAnnotations;

namespace Learning_Content_Models.Models.Enums
{
    public enum Category
    {
        [Display(Name ="Нов Материал")]
        NewMaterial,
		[Display(Name = "Упражнение")]
		Exercise,
		[Display(Name = "Домашна работа")]
		Homework,
		[Display(Name = "За тест")]
		ForTesting
    }
}
