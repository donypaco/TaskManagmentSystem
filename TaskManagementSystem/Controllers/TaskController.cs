using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Services.Interfaces;
using Task = TaskManagementSystem.Data.Task;

namespace TaskManagementSystem.Controllers
{
    [Route("api/Task")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
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
                return StatusCode(500, "An error occurred while creating the Task.");
            }
        }
        [HttpGet("GetAllTasks")]
        public async Task<List<Task>> GetTasks()
        {
            try
            {
                return await _taskService.GetAllTasks();
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
                await _taskService.UpdateTask(existingTask, model);

                return StatusCode(201, "Task successfully updated.");
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return NoContent();
        }


    }
}
