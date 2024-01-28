using TaskManagementSystem.DTO;
using Task = TaskManagementSystem.Data.Task;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface ITaskService
    {
        Task<bool> CreateTask(TaskModel model);
        Task<List<TaskModel>> GetTasks(string sortOrder, int? filterByUser);
        Task<Task> GetTaskById(int TaskId);
        Task<bool> UpdateTask(Task existingTask, TaskModel model);
        Task<bool> DeleteTask(Task existingTask);
        Task<List<TaskStatusDTO>> GetTaskStatuses(int? statusId);
        Task<bool> UpdateStatus(Task existingTask, int statusId);
    }
}
