using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TechTalk.SpecFlow.BindingSkeletons
{
    public class ResourceSkeletonTemplateProvider : FileBasedSkeletonTemplateProvider
    {
        protected override string GetTemplateFileContent()
        {
            var resourceStream = GetType().GetTypeInfo().Assembly.GetManifestResourceStream("TechTalk.SpecFlow.BindingSkeletons.DefaultSkeletonTemplates.sftemplate");
            if (resourceStream == null)
                throw new SpecFlowException("Missing resource: DefaultSkeletonTemplates.sftemplate");

            using (var reader = new StreamReader(resourceStream))
                return reader.ReadToEnd();
        }
    }

    public class DefaultSkeletonTemplateProvider : FileBasedSkeletonTemplateProvider
    {
        private readonly ResourceSkeletonTemplateProvider resourceSkeletonTemplateProvider;

        public DefaultSkeletonTemplateProvider(ResourceSkeletonTemplateProvider resourceSkeletonTemplateProvider)
        {
            this.resourceSkeletonTemplateProvider = resourceSkeletonTemplateProvider;
        }

        protected override string GetTemplateFileContent()
        {
#if SILVERLIGHT || NETCORE //TODO[netcore]: reading the template should be solved
            return "";
#else
            string templateFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"SpecFlow\SkeletonTemplates.sftemplate");
            if (!File.Exists(templateFilePath))
                return "";

            return File.ReadAllText(templateFilePath);
#endif
        }

        protected internal override string GetTemplate(string key)
        {
            return base.GetTemplate(key) ?? resourceSkeletonTemplateProvider.GetTemplate(key);
        }
    }
}