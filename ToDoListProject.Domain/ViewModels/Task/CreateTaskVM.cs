using System.ComponentModel.DataAnnotations;
using ToDoListProject.Domain.Enum;

namespace ToDoListProject.Domain.ViewModels.Task
{
    public class CreateTaskVM
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Priority Priority { get; set; }
    }
}
