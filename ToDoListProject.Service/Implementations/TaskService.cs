using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoListProject.DAL.Interfaces;
using ToDoListProject.Domain.Entity;
using ToDoListProject.Domain.Response;
using ToDoListProject.Domain.ViewModels.Task;
using ToDoListProject.Service.Interfaces;
using ToDoListProject.Domain.Enum;

namespace ToDoListProject.Service.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly IBaseRepository<TaskEntity> _repository;
        private readonly ILogger<TaskEntity> _logger;

        public TaskService(IBaseRepository<TaskEntity> repository, ILogger<TaskEntity> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IBaseResponse<TaskEntity>> Create(CreateTaskVM model)
        {
            try
            {
                _logger.LogInformation($"Request to create tasks - {model.Name}");

                var task = await _repository.GetAll()
                    .Where(x => x.Created.Date == DateTime.Today)
                    .FirstOrDefaultAsync(x => x.Name == model.Name);

                if (task is not null)
                {
                    return new BaseResponse<TaskEntity>()
                    {
                        Description = "Task Exists",
                        StatusCode = StatusCode.TaskIsHasAlready
                    };
                }

                task = new TaskEntity()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Priority = model.Priority,
                    IsDone = false,
                    Created = DateTime.Now
                };

                await _repository.Create(task);

                return new BaseResponse<TaskEntity>()
                {
                    Description = $"Task created: {task.Name} {task.Created}",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
                return new BaseResponse<TaskEntity>()
                {
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
