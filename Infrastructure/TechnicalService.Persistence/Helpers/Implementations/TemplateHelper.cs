using System.Reflection;
using TechnicalService.Persistence.Helpers.Contracts;

namespace TechnicalService.Persistence.Helpers.Implementations
{
    public class TemplateHelper : ITemplateHelper
    {
        public string GetTemplateContent(string templateName, Dictionary<string, string> replacements)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = $"{assembly.GetName().Name}.Helpers.EmailTemplates.{templateName}";

            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
                throw new FileNotFoundException($"Embedded resource not found: {resourceName}");

            using var reader = new StreamReader(stream);
            var templateContent = reader.ReadToEnd();

            foreach (var replacement in replacements)
                templateContent = templateContent.Replace($"{{{{{replacement.Key}}}}}", replacement.Value);

            return templateContent;
        }
    }
}
