using FluentValidation;
using SmartAlertAPI.Models.DTO;

namespace SmartAlertAPI.Utils.Validations
{
    public class IncidentCreateValidation: AbstractValidator<IncidentCreateDTO>
    {
        public IncidentCreateValidation()
        {
            List<string> categories = ["Tornado", "Earthquake", "Avalanche", "andslide", "Storm", "Floods", "Typhoon", "Cyclone", "Fire", "Volcano erruption", "Heatwave"];
            RuleFor(model => model.CategoryName).NotEmpty().NotNull().Must(categories.Contains);
            RuleFor(model => model.PhotoURL).NotEmpty().NotEmpty().Matches("^([.|\\w|-])*\\.(?:jpg|gif|png)$");
            RuleFor(model => model.Comments).NotEmpty().NotNull().MaximumLength(250);
            RuleFor(model => model.UserId).NotEmpty().NotNull();
            RuleFor(model => model.Latitude).NotEmpty().NotNull();
            RuleFor(model => model.Longitude).NotEmpty().NotNull();
        }
    }
}
