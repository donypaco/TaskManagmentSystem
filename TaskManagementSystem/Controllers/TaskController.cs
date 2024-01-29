using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Middleware;
using TaskManagementSystem.Services;
using TaskManagementSystem.Services.Interfaces;
using Task = TaskManagementSystem.Data.Task;

namespace TaskManagementSystem.Controllers
{
    //[Authorize("JwtPolicy")]
    [Route("api/Task")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;
        private readonly ILogService _logService;
        public TaskController(ITaskService taskService, ILogger<TaskController> logger, ILogService logService)
        {
            _taskService = taskService;
            _logger = logger;
            _logService = logService;
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description))
                {
                    return BadRequest("Title and description are required.");
                }

                await _taskService.CreateTask(model);

                return StatusCode(201, "Task successfully added.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logService.LogException(HttpContext, ex);
                return StatusCode(500, "An error occurred while creating the Task.");
            }
        }
        [HttpGet("GetTasks")]
        public async Task<ActionResult<List<TaskModel>>> GetTasks(
        [FromQuery(Name = "sortOrder")] string sortOrder,
        [FromQuery(Name = "filterByUser")] int? filterByUser)
        {
            try
            {
                return await _taskService.GetTasks( sortOrder, filterByUser);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPut("UpdateTask/{taskId}")]
        public async Task<IActionResult> PutTask(int taskId, [FromBody] TaskModel model)
        {
            try
            {
                Task existingTask = await _taskService.GetTaskById(taskId);
                if (existingTask == null)
                {
                    return NotFound("Task not found");
                }
                bool updateResult = await _taskService.UpdateTask(existingTask, model);

                if (updateResult)
                {
                    return StatusCode(200, "Task successfully updated.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while updating the task.");
                }
            }
            catch (Exception ex)
            {
                _logService.LogException(HttpContext, ex);
                throw;
            }
            return NoContent();
        }
        [HttpDelete("DeleteTask/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            try
            {
                Task existingTask = await _taskService.GetTaskById(taskId);
                if (existingTask == null)
                {
                    return NotFound("Task not found");
                }
                bool deleteResult = await _taskService.DeleteTask(existingTask);

                if (deleteResult)
                {
                    return StatusCode(200, "Task successfully deleted.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while deleting the task.");
                }
            }
            catch (Exception ex)
            {
                _logService.LogException(HttpContext, ex);
                throw;
            }
            return NoContent();
        }
        [HttpGet("Statuses")]
        public async Task<List<TaskStatusDTO>> GetStatuses([FromQuery] int? statusId = null)
        {
            try
            {
                return await _taskService.GetTaskStatuses(statusId);
            }
            catch (Exception ex)
            {
                _logService.LogException(HttpContext, ex);
                return null;
            }
        }
        [HttpPatch("UpdateStatus/{taskId}")]
        public async Task<IActionResult> UpdateStatus(int taskId, [FromBody] int statusId)
        {
            try
            {
                Task existingTask = await _taskService.GetTaskById(taskId);

                if (existingTask == null)
                {
                    return NotFound("Data not found");
                }
                var UpdateStatus = await _taskService.UpdateStatus(existingTask, statusId);

                if (UpdateStatus)
                {
                    return StatusCode(200, "Task Status successfully Updated.");
                }
                else
                {
                    return StatusCode(500, "An error occurred while Updating the task.");
                }
            }
            catch (Exception ex)
            {
                _logService.LogException(HttpContext, ex);
                return StatusCode(500, "An error occurred while updating the status");
            }
        }

    }
}
