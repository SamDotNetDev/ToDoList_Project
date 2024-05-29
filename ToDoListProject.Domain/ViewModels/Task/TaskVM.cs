using ToDoListProject.Domain.Enum;

namespace ToDoListProject.Domain.ViewModels.Task
{
    public class TaskVM
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string IsDone { get; set; }

        public string Priority { get; set; }

        public string Description { get; set; }

        public string Created { get; set; }

    }
}
