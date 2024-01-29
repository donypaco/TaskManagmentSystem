namespace TaskManagementSystem.Services.Interfaces
{
    public interface ILogService
    {
        void LogException(HttpContext context, Exception ex);
    }
}
