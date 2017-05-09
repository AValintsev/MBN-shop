using FluentValidation;
using Nop.Plugin.Misc.SMS.Models;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.SMS
{
    class Validator : AbstractValidator<ConfigurationModel>
    {
        public Validator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Misc.SMS.SettingsLogin.Required"));

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Misc.SMS.SettingsPassword.Required"));

            RuleFor(x => x.ApiUrl)
               .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Misc.SMS.SettingsApiUrl.Required"));

            RuleFor(x => x.AdminPhoneNumber)
            .Matches(@"^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}$").WithMessage(localizationService.GetResource("Plugins.Misc.SMS.SettingsAdminPhoneNumber.ErrorMessage"));
        }
    }
}