using ToDoListProject.Domain.Enum;

namespace ToDoListProject.Domain.Filters.Task
{
    public class TaskFilter : PagingFilter
    {
        public string Name { get; set; }

        public Priority? Priority { get; set; }
    }
}
