using System.ComponentModel.DataAnnotations;

namespace Learning_Content_Models.Models.Enums
{
    public enum TypeFile
    {
        [Display(Name = "Текстов документ")]
        TextDocument,
        [Display(Name = "Снимка")]
        Image,
        [Display(Name = "Таблица")]
        Table,
        [Display(Name = "Диаграма")]
        Diagram,
        [Display(Name = "Презентация")]
        Presentation,
        [Display(Name = "Аудио")]
        Audio,
        [Display(Name = "Видео")]
        Video
    }
}
