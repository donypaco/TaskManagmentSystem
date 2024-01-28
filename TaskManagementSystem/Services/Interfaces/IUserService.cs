using TaskManagementSystem.DTO;
using TaskManagementSystem.Data;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        string GenerateJwtToken(User user);
        Task<string> Register(RegisterModel model);
        Task<string> Login(LoginModel model);
        Task<List<RoleDTO>> GetRoles();
        Task<List<Employee>> GetAvailableEmployees();
    }
}
