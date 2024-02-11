using GameOfLife.Services.Exceptions;
using FluentValidation;

namespace GameOfLife.Api.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            int statusCode = StatusCodes.Status500InternalServerError;

            if (exception is BoardNotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
            }
            if (exception is BoardNotActiveException)
            {
                statusCode = StatusCodes.Status400BadRequest;
            }
            if (exception is ValidationException)
            {
                statusCode = StatusCodes.Status400BadRequest;
            }
            if (exception is OverflowException)
            {
                statusCode = StatusCodes.Status508LoopDetected;
            }

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync(exception.Message);
        }
    }
}
