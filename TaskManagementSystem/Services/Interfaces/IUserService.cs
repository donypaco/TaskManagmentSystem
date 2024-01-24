using TaskManagementSystem.DTO;
using TaskManagementSystem.Data;

namespace TaskManagementSystem.Services.Interfaces
{
    public interface IUserService
    {
        string GenerateJwtToken(User user);
        Task<string> Register(RegisterModel model);
        string Login(LoginModel model);
    }
}
