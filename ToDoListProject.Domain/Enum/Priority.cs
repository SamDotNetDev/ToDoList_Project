using System.ComponentModel.DataAnnotations;

namespace ToDoListProject.Domain.Enum
{
    public enum Priority
    {
        [Display(Name = "Easy")]
        Easy = 1,
        [Display(Name = "Medium")]
        Medium = 2,
        [Display(Name = "Hard")]
        Hard = 3
    }
}