using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Interfaces.ReportExporteres.PdfExporteres;
using DomainDocument = Psycheflow.Api.Entities.Document;

namespace Psycheflow.Api.Services.Exporters.PdfExporter.QuestPdfTemplates
{
    public class TestTemplate : IQuestPdfDocument
    {
        public string TemplateName => "Test";

        public Task<byte[]> GenerateAsync(DomainDocument document)
        {
            QuestPDF.Fluent.Document pdf = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // Header
                    page.Header()
                        .Text(document.Name ?? "Untitled Report")
                        .FontSize(20)
                        .SemiBold()
                        .FontColor(Colors.Blue.Medium);

                    // Content
                    page.Content().PaddingVertical(10).Column(col =>
                    {
                        col.Item().Text(document.Description ?? "No description provided").FontSize(14);
                        col.Item().Text("Document data:");

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100); // ID
                                columns.RelativeColumn();    // Name
                                columns.RelativeColumn();    // Value
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCellStyle).Text("ID");
                                header.Cell().Element(HeaderCellStyle).Text("Name");
                                header.Cell().Element(HeaderCellStyle).Text("Value");
                            });

                            if (document.Fields != null && document.Fields.Any())
                            {
                                int index = 1;
                                foreach (var field in document.Fields)
                                {
                                    table.Cell().Element(RowCellStyle).Text(index.ToString());
                                    table.Cell().Element(RowCellStyle).Text(field.Name ?? "-");
                                    table.Cell().Element(RowCellStyle).Text(field.Value?.ToString() ?? "-");
                                    index++;
                                }
                            }
                            else
                            {
                                table.Cell().ColumnSpan(3).Text("No fields available.").Italic();
                            }

                            static IContainer HeaderCellStyle(IContainer container) =>
                                container
                                    .DefaultTextStyle(x => x.SemiBold())
                                    .Padding(5)
                                    .Background(Colors.Grey.Lighten2)
                                    .Border(1);

                            static IContainer RowCellStyle(IContainer container) =>
                                container
                                    .Padding(5)
                                    .BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten1);
                        });

                        col.Item().PaddingTop(10).Text("Generated on: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    });

                    // Footer
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
                });
            });

            // Generate PDF as byte[]
            return Task.FromResult(pdf.GeneratePdf());
        }
    }
}
