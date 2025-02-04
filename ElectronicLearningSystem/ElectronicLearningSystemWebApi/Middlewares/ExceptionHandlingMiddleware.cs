using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Helpers.Exceptions;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using System.Text.Json;

namespace ElectronicLearningSystemWebApi.Middlewares
{
    /// <summary>
    /// Глобальный обработчик ошибок.
    /// </summary>
    /// <param name="next">Следующий обработчик.</param>
    /// <param name="logger">Логгер.</param>
    public class ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        /// <summary>
        /// Следующий обработчик.
        /// </summary>
        private readonly RequestDelegate _next = next;

        /// <summary>
        /// Логгер.
        /// </summary>
        private readonly ILogger<ExceptionHandlingMiddleware> _logger = logger;

        /// <summary>
        /// Выполнение следующего обработчика и отлов необработанных ошибок.
        /// </summary>
        /// <param name="context">Контекст выполнения запроса. </param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.ServerException, ex.ToString(), "Unhandled exception occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Генерация ответа в виде ошибки сервера.
        /// </summary>
        /// <param name="context">Контекст выполнения запроса. </param>
        /// <param name="exception">Ошибка выполнения запроса. </param>
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = exception switch
            {
                UnauthorizedAccessException => new ErrorResponse
                {
                    ErrorCode = "ACCOUNT_UNAUTHORIZED",
                    ErrorMessage = exception.Message
                },
                KeyNotFoundException => new ErrorResponse
                {
                    ErrorCode = "NOT_FOUND",
                    ErrorMessage = exception.Message
                },
                InvalidOperationException => new ErrorResponse
                {
                    ErrorCode = "ACCOUNT_BLOCKED",
                    ErrorMessage = exception.Message
                },
                DublicateUserException => new ErrorResponse
                {
                    ErrorCode = "ACCOUNT_DUBLICATE",
                    ErrorMessage = exception.Message
                },
                _ => new ErrorResponse
                {
                    ErrorCode = "SERVER_ERROR",
                    ErrorMessage = "Internal server error"
                }
            };

            context.Response.StatusCode = response.ErrorCode switch
            {
                "ACCOUNT_UNAUTHORIZED" => StatusCodes.Status401Unauthorized,
                "ACCOUNT_BLOCKED" => StatusCodes.Status403Forbidden,
                "ACCOUNT_DUBLICATE" => StatusCodes.Status409Conflict,
                "NOT_FOUND" => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
