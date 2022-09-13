using Core.Templating.Services;

namespace Roundcomb.Services.EmailTemplates
{
    public class RoundcombTemplateProvider : ITemplateProvider
    {
        public void RegisterTemplates(ITemplateRegistration templateRegistration)
        {
            templateRegistration.RegisterTemplate<PropertyApplicationUserAcceptedNotificationTemplate>();

            templateRegistration.RegisterTemplate<PropertyApplicationUserDeclinedNotificationTemplate>();
        }
    }
}