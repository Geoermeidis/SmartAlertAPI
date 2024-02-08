using FluentValidation;
using SmartAlertAPI.Models.DTO;

namespace SmartAlertAPI.Utils.Validations
{
    public class IncidentCreateValidation: AbstractValidator<IncidentCreateDTO>
    {
        public IncidentCreateValidation()
        {
            List<string> categories = ["tornado", "earthquake", "avalanche", "landslide", "blizzard", 
                "storm", "tsunami", "floods", "industrial", "wildfire", "accident", "volcano", "heatwave"];
            RuleFor(model => model.CategoryName).NotEmpty().NotNull().Must(categories.Contains);
            RuleFor(model => model.PhotoURL).NotNull();
            RuleFor(model => model.Comments).NotNull().MaximumLength(250);
            RuleFor(model => model.UserId).NotEmpty().NotNull();
            RuleFor(model => model.Latitude).NotEmpty().NotNull();
            RuleFor(model => model.Longitude).NotEmpty().NotNull();
        }
    }
}
