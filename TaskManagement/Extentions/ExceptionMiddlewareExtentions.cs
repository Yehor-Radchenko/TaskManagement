using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using TaskManagement.API.ExceptionMiddleware;
using TaskManagement.Common.ResponseModels;

namespace TaskManagement.API.Extentions
{
    public static class ExceptionMiddlewareExtentions
    {
        public static void ConfigureExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware.ExceptionMiddleware>();
        }
    }
}
