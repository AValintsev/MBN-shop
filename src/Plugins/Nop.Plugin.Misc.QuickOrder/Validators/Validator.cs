using FluentValidation;
using Nop.Plugin.Misc.QuickOrder.Models;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.QuickOrder
{
    class Validator : AbstractValidator<QOrderModel>
    {
        public Validator(ILocalizationService localizationService/*,*bool nameRequired = true,bool emailRequiered = true, bool phoneRequired = true*/)
        {
            RuleFor(x => x.CustomerName)
               .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Misc.QuickOrder.CustomerName.Required"));

            RuleFor(x => x.CustomerEmail)
                .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Misc.QuickOrder.CustomerEmail.Required"));

            RuleFor(x => x.CustomerEmail)
                .Matches(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$").WithMessage(localizationService.GetResource("Plugins.Misc.QuickOrder.CustomerEmail.ErrorMessage"));

            RuleFor(x => x.CustomerPhone)
                .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Misc.QuickOrder.CustomerPhone.Required"));

            RuleFor(x => x.CustomerPhone)
            .Matches(@"^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}$").WithMessage(localizationService.GetResource("Plugins.Misc.QuickOrder.CustomerPhone.ErrorMessage"));
        }
    }
}