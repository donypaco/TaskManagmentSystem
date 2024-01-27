using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Data;
using TaskManagementSystem.DTO;
using TaskManagementSystem.Services.Interfaces;
using Task = TaskManagementSystem.Data.Task;

namespace TaskManagementSystem.Services
{
    public class TaskService: ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public TaskService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<bool> CreateTask(TaskModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Description))
                {
                    throw new ArgumentException("Title and description are required.");
                }

                var user = await _context.Users.FindAsync(model.UserAssignedId);

                var status = await _context.TaskStatuses.FindAsync(model.StatusId);


                Task Task = new Task
                {
                    Title = model.Title,
                    Description = model.Description,
                    UserAssigned = user,
                    CreatedDate = DateTime.UtcNow,
                    Status = status
                };
                _context.Tasks.Add(Task);
                int savedChangesCount = await _context.SaveChangesAsync();

                return savedChangesCount > 0;
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error has occurred.");
            }
        }
        public async Task<List<Task>> GetAllTasks()
        {
            try
            {
               return await _context.Tasks.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error has occurred.");
            }
        }
        public async Task<Task> GetTaskById(int taskId)
        {
            try
            {
                return await _context.Tasks.FindAsync(taskId);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error has occurred.");
            }
        }
        public async Task<bool> UpdateTask(Task existingTask, TaskModel model)
        {
            try
            {
                var status = await _context.TaskStatuses.FindAsync(model.StatusId);
                var user = await _context.Users.FindAsync(model.UserAssignedId);

                existingTask.Title = model.Title;
                existingTask.Description = model.Description;
                existingTask.UserAssigned = user;
                existingTask.Status = status;

                _context.Entry(existingTask).State = EntityState.Modified;

                int savedChangesCount = await _context.SaveChangesAsync();

                return savedChangesCount > 0;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error has occurred.");
            }
        }


    }
}
