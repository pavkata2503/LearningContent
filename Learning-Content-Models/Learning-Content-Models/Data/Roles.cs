using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Learning_Content_Models.Data
{
    public enum Roles
    {
        [Display(Name ="Админ")]
        Admin,
        [Display(Name = "Учител")]
        Teacher,
        [Display(Name = "Ученик")]
        Student
    }
}
