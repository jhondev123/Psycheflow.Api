using Psycheflow.Api.Dtos.Responses;

namespace Psycheflow.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate Next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            Next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(
                    GenericResponseDto<object>.ToFail($"Houve um erro inesperado {ex.Message}"));
            }
        }
    }
}
