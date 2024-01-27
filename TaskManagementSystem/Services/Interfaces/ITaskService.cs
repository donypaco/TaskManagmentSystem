using TaskManagementSystem.DTO;
using Task = TaskManagementSystem.Data.Task;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface ITaskService
    {
        Task<bool> CreateTask(TaskModel model);
        Task<List<Task>> GetAllTasks();
        Task<Task> GetTaskById(int TaskId);
        Task<bool> UpdateTask(Task existingTask, TaskModel model);
    }
}
