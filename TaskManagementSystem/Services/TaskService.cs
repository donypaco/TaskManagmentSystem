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

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
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
        public async Task<List<TaskModel>> GetTasks(string sortOrder, int? filterByUser)
        {
            try
            {
                var query = _context.Tasks.AsQueryable();
                sortOrder = (!string.IsNullOrEmpty(sortOrder)) ? sortOrder : "asc";

                query = sortOrder.ToLower() == "desc" ?
                    query.OrderByDescending(t => EF.Property<object>(t, "CreatedDate")) :
                    query.OrderBy(t => EF.Property<object>(t, "CreatedDate"));

                if (filterByUser.HasValue)
                {
                    query = query.Where(t => t.UserAssignedId == filterByUser);
                }
                var tasksWithRelatedData = await query.Include(t => t.Status)
                                    .Include(t => t.UserAssigned)
                                    .Select(t => new TaskModel
                                    {
                                        Id = t.Id,
                                        Title = t.Title,
                                        Description = t.Description,
                                        CreatedDate = t.CreatedDate,
                                        CompletedDate = t.CompletedDate,
                                        Status = t.Status.Status,
                                        StatusId = t.StatusId,
                                        UserAssigned = t.UserAssigned.UserName
                                    })
                                    .ToListAsync();

                return tasksWithRelatedData;
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

                existingTask.Title = model.Title;
                existingTask.Description = model.Description;
                _context.Entry(existingTask).State = EntityState.Modified;

                int savedChangesCount = await _context.SaveChangesAsync();

                return savedChangesCount > 0;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error has occurred.");
            }
        }
        public async Task<bool> DeleteTask(Task existingTask)
        {
            try
            {
                 _context.Tasks.Remove(existingTask);
                int savedChangesCount = await _context.SaveChangesAsync();
                return (savedChangesCount > 0);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error has occurred.");
            }
        }
        public async Task<List<TaskStatusDTO>> GetTaskStatuses(int? statusId)
        {
            var query = _context.TaskStatuses.AsQueryable();

            if (statusId.HasValue)
            {
                query = query.Where(ts => ts.StatusId == statusId);
            }

            var statuses = await query
                .Select(r => new TaskStatusDTO
                {
                    Id = r.StatusId,
                    Status = r.Status
                })
                .ToListAsync();

            return statuses;
        }
        public async Task<bool> UpdateStatus(Task existingTask, int statusId)
        {
            try
            {
                var newStatus = await _context.TaskStatuses.FindAsync(statusId);
                existingTask.Status = newStatus;
                if(newStatus.Status.ToLower() == "done")
                    existingTask.CompletedDate = DateTime.Now;
                int savedChangesCount = await _context.SaveChangesAsync();
                return (savedChangesCount > 0);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("An error has occurred.");
            }
        }


    }
}
