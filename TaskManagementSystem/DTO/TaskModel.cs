using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TaskManagementSystem.Data;
using TaskStatus = TaskManagementSystem.Data.TaskStatus;

namespace TaskManagementSystem.DTO
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public int UserAssignedId { get; set; }
        public string UserAssigned { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }

    }
}
