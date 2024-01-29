using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Win32;
using System;
using TaskManagementSystem.Data;
using TaskManagementSystem.Services;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementSystem.Middleware
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LogService _logService;

        public ErrorLoggingMiddleware(RequestDelegate next, LogService logService)
        {
            _next = next;
            _logService = logService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logService.LogException(context, ex);
                throw;
            }
        }

    }
}