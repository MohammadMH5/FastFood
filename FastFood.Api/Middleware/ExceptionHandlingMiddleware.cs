using FastFood.Api.Exceptions;
using System.Net;
using System.Text.Json;

namespace FastFood.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OperationCanceledException)
                when (context.RequestAborted.IsCancellationRequested)
            {
                _logger.LogDebug(
                    "The request was cancelled by the client.");
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(
                    ex,
                    "A business rule was violated.");

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.BadRequest,
                    ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An unhandled exception occurred.");

                await HandleExceptionAsync(
                    context,
                    HttpStatusCode.InternalServerError,
                    "خطای غیرمنتظره‌ای در سرور رخ داده است.");
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string message)
        {
            if (context.Response.HasStarted)
            {
                throw new InvalidOperationException(
                    "The response has already started.");
            }

            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType =
                "application/json; charset=utf-8";

            var response = new
            {
                statusCode = (int)statusCode,
                message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
