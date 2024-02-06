using FluentValidation;
using SmartAlertAPI.Models.DTO;

namespace SmartAlertAPI.Utils.Validations
{
    public class IncidentCreateValidation: AbstractValidator<IncidentCreateDTO>
    {
        public IncidentCreateValidation()
        {
            List<string> categories = ["tornado", "earthquake", "avalanche", "landslide", "storm", "floods", "typhoon", "cyclone", "fire", "volcano erruption", "heatwave"];
            RuleFor(model => model.CategoryName).NotEmpty().NotNull().Must(categories.Contains);
            RuleFor(model => model.PhotoURL).NotNull();
            RuleFor(model => model.Comments).NotNull().MaximumLength(250);
            RuleFor(model => model.UserId).NotEmpty().NotNull();
            RuleFor(model => model.Latitude).NotEmpty().NotNull();
            RuleFor(model => model.Longitude).NotEmpty().NotNull();
        }
    }
}
