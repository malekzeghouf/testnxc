using Neoledge.Nxc.Domain.Exceptions;

namespace Neoledge.NxC.Api.Middlewares
{
    public class ApiExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionMiddleware> _logger;

        public ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred");

                context.Response.ContentType = "application/json";

                var (statusCode, message) = ex switch
                {
                    EntityNotFoundException => (404, ex.Message),
                    EntityAlreadyExistsException => (400, ex.Message),
                    EntityValidationException => (400, ex.Message),
                    UnauthorizedOperationException => (401, ex.Message),
                    _ => (500, "An internal server error occurred.")
                };

                context.Response.StatusCode = statusCode;

                var errorResponse = new
                {
                    status = statusCode,
                    error = message
                };

                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}