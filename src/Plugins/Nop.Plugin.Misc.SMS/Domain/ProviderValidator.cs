using FluentValidation;
using Nop.Services.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.SMS.Domain
{
    public class ProviderValidator : AbstractValidator<SMSProvider>
    {
        public ProviderValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Misc.SMS.ProviderLoginRequired"));

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Misc.SMS.ProviderApiRequired"));

            RuleFor(x => x.Api)
               .NotEmpty().WithMessage(localizationService.GetResource("Plugins.Misc.SMS..TrialTracker.NameRequired"));
        }
    }
}
