using System;

namespace ToDoListProject.Domain.ViewModels.Task
{
    public class TaskCompletedVM
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
