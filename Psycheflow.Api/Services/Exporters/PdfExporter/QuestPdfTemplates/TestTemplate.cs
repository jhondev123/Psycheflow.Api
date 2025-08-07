using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Interfaces.ReportExporteres.PdfExporteres;

namespace Psycheflow.Api.Services.Exporters.PdfExporter.QuestPdfTemplates
{
    public class TestTemplate : IQuestPdfDocument
    {
        public string TemplateName => "Test";

        public Task<byte[]> GenerateAsync(Document document)
        {
            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text("Test Report").FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Text("This is a randomly generated test PDF.").FontSize(14);
                        col.Item().Text("Below is some sample data:");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("ID").SemiBold();
                                header.Cell().Element(CellStyle).Text("Name").SemiBold();
                                header.Cell().Element(CellStyle).Text("Value").SemiBold();

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.SemiBold()).Padding(5).Background(Colors.Grey.Lighten2).Border(1);
                                }
                            });

                            // Rows
                            var random = new Random();
                            for (int i = 1; i <= 5; i++)
                            {
                                table.Cell().Element(CellStyle).Text(i.ToString());
                                table.Cell().Element(CellStyle).Text($"Item {i}");
                                table.Cell().Element(CellStyle).Text($"{random.Next(100, 999)}");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.Padding(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten1);
                                }
                            }
                        });

                        col.Item().PaddingTop(10).Text("Generated on: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
                });
            });

            byte[] bytes = pdf.GeneratePdf();
            return Task.FromResult(bytes);
        }
    }
}
