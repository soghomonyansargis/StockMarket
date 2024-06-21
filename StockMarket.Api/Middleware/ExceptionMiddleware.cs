using FluentValidation;
using StockMarket.Api.Extensions;
using StockMarket.Infrastructure.Exceptions;
using System.Net.Mime;
using System.Text.Json;

namespace StockMarket.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">Following delegate.</param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Forwards the http call to the destination.
        /// </summary>
        /// <param name="httpContext">Given HTTP context.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        /// <summary>
        /// Handles the case when an exception is being thrown during a request execution.
        /// </summary>
        /// <param name="context">Given HTTP context.</param>
        /// <param name="exception">Given exception.</param>
        /// <returns><see cref="Task"/>.</returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var error = exception switch
            {
                ValidationException ve => ve.ToBadRequest(),
                NotFoundException f => f.ToNotFound(),
                TooManyRequestsException x => x.TooManyRequests(),
                _ => exception.ToInternalServerError(),
            };

            context.Response.StatusCode = error.StatusCode;
            await context.Response.WriteAsJsonAsync(
                error,
                error.GetType(),
                (JsonSerializerOptions)null,
                MediaTypeNames.Application.Json);
        }
    }
}
