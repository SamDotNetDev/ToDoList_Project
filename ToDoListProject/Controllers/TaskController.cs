using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoListProject.Domain.Filters.Task;
using ToDoListProject.Domain.Helpers;
using ToDoListProject.Domain.ViewModels.Task;
using ToDoListProject.Models;
using ToDoListProject.Service.Interfaces;

namespace ToDoListProject.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CalculateComletedTasks()
        {
            var response = await _service.CalculateComletedTasks();
            if(response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                var csvService = new CsvBaseService<IEnumerable<TaskVM>>();
                var uploadFile = csvService.UploadFiles(response.Data);

                return File(uploadFile, "text/csv", $"Statistics for {DateTime.Now.ToLongDateString()}.csv");
            }

            return BadRequest(new {description = response.Description});
        }

        [HttpGet]
        public async Task<IActionResult> GetCompletedTasks()
        {
            var result = await _service.GetCompletedTasks();
            if (result.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Json(new { data = result.Data });
            }
            else
            {
                return StatusCode((int)result.StatusCode, result.Description);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskVM VM)
        {
            var response = await _service.Create(VM);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new {description = response.Description});
            }
            return BadRequest(new {description = response.Description});
        }

        [HttpPost]
        public async Task<IActionResult> EndTask(long id)
        {
            var response = await _service.EndTask(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new { description = response.Description });
            }
            return BadRequest(new { description = response.Description });
        }

        [HttpPost]
        public async Task<IActionResult> TaskHandler(TaskFilter filter)
        {
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();

            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;

            filter.Skip = skip;
            filter.PageSize = pageSize;

            var response = await _service.GetTasks(filter);
            return Json(new { recordsFiltered = response.Total, recordsTotal = response.Total, data = response.Data});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
