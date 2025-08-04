using Psycheflow.Api.Interfaces.ReportExporteres.PdfExporteres;
using System.Reflection;

namespace Psycheflow.Api.Factories
{
    public class QuestPdfTemplateFactory
    {
        private readonly Dictionary<string, IQuestPdfDocument> Templates;

        public QuestPdfTemplateFactory(IServiceProvider serviceProvider)
        {
            IEnumerable<IQuestPdfDocument?> templates = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IQuestPdfDocument).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(t => (IQuestPdfDocument)(serviceProvider.GetService(t) ?? Activator.CreateInstance(t)));

            Templates = templates.ToDictionary(t => t.TemplateName, t => t);
        }

        public IQuestPdfDocument GetTemplate(string templateName)
        {
            if (Templates.TryGetValue(templateName, out var template))
            {
                return template;
            }

            throw new InvalidOperationException($"Template '{templateName}' não encontrado.");
        }
    }
}
