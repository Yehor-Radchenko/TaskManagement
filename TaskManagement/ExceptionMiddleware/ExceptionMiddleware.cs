using System.Net;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TaskManagement.Common.Exceptions;
using TaskManagement.Common.ResponseModels;

namespace TaskManagement.API.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostEnvironment env;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
        {
            this.next = next;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);
            try
            {
                await this.next(httpContext).ConfigureAwait(false);
            }
            catch (ArgumentNullException ex)
            {
                Log.Error(
                    ex,
                    "A required argument was null. Path: {Path}, QueryString: {QueryString}, Method: {Method}, User: {User}",
                    httpContext.Request.Path,
                    httpContext.Request.QueryString,
                    httpContext.Request.Method,
                    httpContext.User.Identity?.Name ?? "Anonymous");

                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await this.HandleHttpAsync(httpContext, ex).ConfigureAwait(false);
            }
            catch (UnauthorizedAccessException ex)
            {
                Log.Error(
                    ex,
                    "Unauthorized access attempt. Path: {Path}, QueryString: {QueryString}, Method: {Method}, User: {User}",
                    httpContext.Request.Path,
                    httpContext.Request.QueryString,
                    httpContext.Request.Method,
                    httpContext.User.Identity?.Name ?? "Anonymous");

                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await this.HandleHttpAsync(httpContext, ex).ConfigureAwait(false);
            }
            catch (ConflictException ex)
            {
                Log.Error(
                    ex,
                    "A conflict occurred. Path: {Path}, QueryString: {QueryString}, Method: {Method}, User: {User}",
                    httpContext.Request.Path,
                    httpContext.Request.QueryString,
                    httpContext.Request.Method,
                    httpContext.User.Identity?.Name ?? "Anonymous");

                httpContext.Response.StatusCode = (int)HttpStatusCode.Conflict;
                await this.HandleHttpAsync(httpContext, ex).ConfigureAwait(false);
            }
            catch (KeyNotFoundException ex)
            {
                Log.Error(
                    ex,
                    "A requested resource was not found. Path: {Path}, QueryString: {QueryString}, Method: {Method}, User: {User}",
                    httpContext.Request.Path,
                    httpContext.Request.QueryString,
                    httpContext.Request.Method,
                    httpContext.User.Identity?.Name ?? "Anonymous");

                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await this.HandleHttpAsync(httpContext, ex).ConfigureAwait(false);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(
                    ex,
                    "Database update error. Path: {Path}, QueryString: {QueryString}, Method: {Method}, User: {User}",
                    httpContext.Request.Path,
                    httpContext.Request.QueryString,
                    httpContext.Request.Method,
                    httpContext.User.Identity?.Name ?? "Anonymous");

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await this.HandleHttpAsync(httpContext, ex).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "An unknown error occurred. Path: {Path}, QueryString: {QueryString}, Method: {Method}, User: {User}",
                    httpContext.Request.Path,
                    httpContext.Request.QueryString,
                    httpContext.Request.Method,
                    httpContext.User.Identity?.Name ?? "Anonymous");

                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await this.HandleHttpAsync(httpContext, ex).ConfigureAwait(false);
            }
        }

        private async Task HandleHttpAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var errorDetails = new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = this.env.IsDevelopment() ? ex.Message : "Internal server error",
            };

            Log.Warning("Returning error response: {StatusCode} - {Message}", errorDetails.StatusCode, errorDetails.Message);

            await httpContext.Response.WriteAsync(errorDetails.ToString()).ConfigureAwait(false);
        }
    }
}
