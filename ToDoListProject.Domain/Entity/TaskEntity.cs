using System.ComponentModel.DataAnnotations;
using ToDoListProject.Domain.Enum;

namespace ToDoListProject.Domain.Entity
{
    public class TaskEntity
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Priority Priority { get; set; }
    }
}
