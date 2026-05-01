using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;

namespace PlotCreator_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception on {Path}", ctx.Request.Path);
                ctx.Response.StatusCode = 500;
                ctx.Response.ContentType = "application/json";
                var body = new BaseResponse<object?>
                {
                    Data = null,
                    StatusCode = StatusCode.InternalServerError,
                    Description = ex.Message,
                    ErrorForUser = "Internal server error"
                };
                await ctx.Response.WriteAsync(JsonSerializer.Serialize(body, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }));
            }
        }
    }
}
