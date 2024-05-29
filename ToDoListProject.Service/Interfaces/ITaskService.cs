﻿using ToDoListProject.Domain.Entity;
using ToDoListProject.Domain.Filters.Task;
using ToDoListProject.Domain.Response;
using ToDoListProject.Domain.ViewModels.Task;

namespace ToDoListProject.Service.Interfaces
{
    public interface ITaskService
    {
        Task<IBaseResponse<TaskEntity>> Create(CreateTaskVM model);

        Task<IBaseResponse<bool>> EndTask(long id);

        Task<IBaseResponse<IEnumerable<TaskVM>>> GetTasks(TaskFilter filter);
    }
}