using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoListProject.DAL.Interfaces;
using ToDoListProject.Domain.Entity;
using ToDoListProject.Domain.Response;
using ToDoListProject.Domain.ViewModels.Task;
using ToDoListProject.Service.Interfaces;
using ToDoListProject.Domain.Enum;
using ToDoListProject.Domain.Filters.Task;
using ToDoListProject.Domain.Extensions;
using System.Diagnostics;
using System.Globalization;
using System.Data;

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

        public async Task<IBaseResponse<IEnumerable<TaskVM>>> CalculateComletedTasks()
        {
            try
            {
                var tasks = await _repository.GetAll()
                    .Where(x => x.Created.Date == DateTime.Today)
                    .Select(x => new TaskVM()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsDone = x.IsDone == true ? "Done" : "Undone",
                        Priority = x.Priority.GetDisplayName(),
                        Created = x.Created.ToString(CultureInfo.InvariantCulture)
                    })
                    .ToListAsync();

                return new BaseResponse<IEnumerable<TaskVM>>()
                {
                    Data = tasks,
                    StatusCode = StatusCode.OK,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.CalculateComletedTasks]: {ex.Message}");
                return new BaseResponse<IEnumerable<TaskVM>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskCompletedVM>>> GetCompletedTasks()
        {
            try
            {
                var tasks = await _repository.GetAll()
                    .Where(x => x.IsDone)
                    .Where(x => x.Created.Date == DateTime.Today)
                    .Select(x => new TaskCompletedVM()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description.Substring(0, Math.Min(x.Description.Length, 5))
                    }).ToListAsync();

                return new BaseResponse<IEnumerable<TaskCompletedVM>>()
                {
                    Data = tasks,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.GetCompletedTasks]: {ex.Message}");
                return new BaseResponse<IEnumerable<TaskCompletedVM>>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<TaskEntity>> Create(CreateTaskVM model)
        {
            try
            {
                model.Validate();

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
                    Description = $"Task created",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
                return new BaseResponse<TaskEntity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> EndTask(long id)
        {
            try
            {
                var task = await _repository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                if (task is null)
                {
                    return new BaseResponse<bool>()
                    {
                        Description = "Task not found",
                        StatusCode = StatusCode.TaskNotFound
                    };
                }
                task.IsDone = true;

                await _repository.Update(task);

                return new BaseResponse<bool>()
                {
                    Description = "Task is done",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.EndTask]: {ex.Message}");
                return new BaseResponse<bool>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<DataTableResult> GetTasks(TaskFilter filter)
        {
            try
            {
                var task = await _repository.GetAll()
                    .Where(x => x.Created.Date == DateTime.Today)
                    .Where(x => !x.IsDone)
                    .WhereIf(!string.IsNullOrWhiteSpace(filter.Name), x => x.Name == filter.Name)
                    .WhereIf(filter.Priority.HasValue, x => x.Priority == filter.Priority)
                    .Select(x => new TaskVM()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        IsDone = x.IsDone == true ? "Done" : "Undone",
                        Priority = x.Priority.GetDisplayName(),
                        Created = x.Created.ToLongDateString()
                    })
                    .Skip(filter.Skip)
                    .Take(filter.PageSize)
                    .ToListAsync();

                var count = _repository.GetAll().Count(x => !x.IsDone);

                return new DataTableResult()
                {
                    Data = task,
                    Total = count
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"[TaskService.Create]: {ex.Message}");
                return new DataTableResult()
                {
                    Data = null,
                    Total = 0
                };
            }
        }
    }
}
