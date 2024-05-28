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

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ArgumentNullException(Name, "Write name of the task");
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                throw new ArgumentNullException(Description, "Write description of the task");
            }
        }
    }
}
