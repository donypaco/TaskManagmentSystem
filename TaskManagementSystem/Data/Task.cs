using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementSystem.Data
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime CompletedDate { get; set; }
        public int StatusId { get; set; }

        //public int UserCreatedId { get; set; }

        //[ForeignKey("UserCreatedId")]
        //public User UserCreated { get; set; }

        public int UserAssignedId { get; set; }
        [ForeignKey("UserAssignedId")]
        public User UserAssigned { get; set; }

        [ForeignKey("StatusId")]
        public TaskStatus Status { get; set; }
    }
}
