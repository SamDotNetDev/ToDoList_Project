using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
        public async Task<IActionResult> Create(CreateTaskVM VM)
        {
            var response = await _service.Create(VM);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Ok(new {description = response.Description});
            }
            return BadRequest(new {description = response.Description});
        }

        public async Task<IActionResult> TaskHandler()
        {
            var response = await _service.GetTasks();
            return Json(new { data = response.Data});
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
