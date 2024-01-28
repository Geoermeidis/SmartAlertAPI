
using FluentValidation;
using SmartAlertAPI.Models.Dto;

namespace SmartAlertAPI.Utils.Filters
{
    public class BasicValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
            if (validator is not null)
            {
                var entity = context.Arguments.OfType<T>().FirstOrDefault(a => a?.GetType() == typeof(T));
                if (entity is not null)
                {
                    var validation = validator.Validate(entity);
                    if (validation.IsValid)
                    {
                        return await next(context);
                    }
                    return Results.ValidationProblem(validation.ToDictionary());
                }
            }
            else {
                return Results.Problem("Could not find entity");
            }
            
            

            return await next(context);
        }
    }
}
