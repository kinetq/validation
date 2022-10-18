using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Kinetq.Validation.Exceptions;
using Kinetq.Validation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Kinetq.Validation.Middleware
{
    public class ValidatorMiddleware
    {
        private readonly RequestDelegate _request;
        private readonly ILogger<ValidatorMiddleware> _logger;

        public ValidatorMiddleware(RequestDelegate pipeline, ILogger<ValidatorMiddleware> logger)
        {
            _request = pipeline;
            _logger = logger;
        }

        public Task Invoke(HttpContext context, JsonSerializerOptions jsonOptions) => InvokeAsync(context, jsonOptions); // Stops VS from nagging about async method without ...Async suffix.

        async Task InvokeAsync(HttpContext context, JsonSerializerOptions jsonOptions)
        {
            try
            {
                await _request(context);
            }
            catch (ValidationsException validationsException)
            {
                context.Response.Headers.Clear();
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var response = new ValidationResponse()
                {
                    Errors = validationsException.ValidationErrors.ErrorMessages
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
            }
            catch (ValidationException validationException)
            {
                context.Response.Headers.Clear();
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errorMessages = new List<ValidationError>()
                {
                    new()
                    {
                        Messages = new List<string>()
                        {
                            validationException.Message
                        }
                    }
                };

                var response = new ValidationResponse()
                {
                    Errors = errorMessages
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
            }
            catch (StatusCodeException exception)
            {
                context.Response.Headers.Clear();
                context.Response.StatusCode = (int)exception.StatusCode;
            }
            catch (Exception ex)
            {
                context.Response.Headers.Clear();
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                _logger.LogError(ex, "Unhandled exception in API");
            }
        }
    }
}