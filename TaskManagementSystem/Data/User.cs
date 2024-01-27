using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TaskManagementSystem.Data
{
    public class User : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_User { get; set; }
        //[Required]
        //[StringLength(100)]
        //public string Username { get; set; }

        [Required]
        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; } = new Role();
        public List<Task> TasksAssigned { get; set; } = new List<Task>();
    }
}
