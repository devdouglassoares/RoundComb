using System.Collections.Generic;

namespace Core.Templating.Services
{
    public interface ITemplateProvider : IDependency
    {
        void RegisterTemplates(ITemplateRegistration templateRegistration);
    }

    public class DefaultTemplateProvider : ITemplateProvider
    {
        public void RegisterTemplates(ITemplateRegistration templateRegistration)
        {
            
        }
    }
}