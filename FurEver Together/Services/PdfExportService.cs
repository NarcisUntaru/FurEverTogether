using FurEver_Together.DataModels;
using FurEver_Together.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace FurEver_Together.Services
{
    public class PdfExportService : IPdfExportService
    {
        public byte[] GenerateAdoptionRequestsPdf(IEnumerable<Adoption> adoptionRequests)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header()
                        .Column(column =>
                        {
                            column.Item()
                                .AlignCenter()
                                .Width(200)
                                .Background("#2F3C7E")
                                .CornerRadius(15)
                                .PaddingVertical(15)
                                .PaddingHorizontal(20)
                                .Column(logoColumn =>
                                {
                                    logoColumn.Item()
                                        .Height(60)
                                        .Image("wwwroot/uploads/Logo/logo_wide.png");
                                });

                            column.Item()
                                .PaddingTop(15)
                                .AlignCenter()
                                .Text("Adoption Requests Report")
                                .SemiBold()
                                .FontSize(20)
                                .FontColor("#2F3C7E");
                        });

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(20);

                            column.Item().Text($"Generated on: {DateTime.Now:MMMM dd, yyyy HH:mm}")
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken2);

                            column.Item().Text($"Total Requests: {adoptionRequests.Count()}")
                                .SemiBold()
                                .FontSize(12);

                            foreach (var request in adoptionRequests)
                            {
                                column.Item().Element(c => CreateAdoptionRequestSection(c, request));
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                });
            });

            return document.GeneratePdf();
        }

        private void CreateAdoptionRequestSection(IContainer container, Adoption request)
        {
            container.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(15).Column(column =>
            {
                column.Spacing(10);

                // Header with Request ID and Status
                column.Item().Row(row =>
                {
                    row.RelativeItem().Text($"Request ID: {request.Id}").SemiBold().FontSize(14);
                    row.ConstantItem(120).AlignRight().Element(c => CreateStatusBadge(c, request.Status));
                });

                column.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten3);

                // Pet Information
                column.Item().Text("Pet Information").SemiBold().FontSize(12).FontColor("#2F3C7E");
                column.Item().PaddingLeft(10).Column(petColumn =>
                {
                    petColumn.Item().Text($"Name: {request.Pet?.Name ?? "N/A"}");
                    petColumn.Item().Text($"Type: {request.Pet?.Type.ToString() ?? "N/A"}");
                    petColumn.Item().Text($"Breed: {request.Pet?.Breed ?? "N/A"}");
                    petColumn.Item().Text($"Age: {request.Pet?.Age.ToString() ?? "N/A"} years");
                    petColumn.Item().Text($"Gender: {request.Pet?.Gender ?? "N/A"}");
                    petColumn.Item().Text($"Pet ID: {request.PetId}");
                });

                column.Item().PaddingTop(10);

                // User Information
                column.Item().Text("Adopter Information").SemiBold().FontSize(12).FontColor("#2F3C7E");
                column.Item().PaddingLeft(10).Column(userColumn =>
                {
                    userColumn.Item().Text($"User ID: {request.UserId ?? "N/A"}");
                    userColumn.Item().Text($"Email: {request.User?.Email ?? "N/A"}");
                    userColumn.Item().Text($"Phone: {request.User?.PhoneNumber ?? "N/A"}");
                });

                column.Item().PaddingTop(10);

                // Request Details
                column.Item().Text("Request Details").SemiBold().FontSize(12).FontColor("#2F3C7E");
                column.Item().PaddingLeft(10).Column(detailsColumn =>
                {
                    detailsColumn.Item().Text($"Request Date: {request.RequestDate?.ToString("MMMM dd, yyyy") ?? "N/A"}");
                    detailsColumn.Item().Text($"Adoption Date: {request.AdoptionDate?.ToString("MMMM dd, yyyy") ?? "Pending"}");
                    detailsColumn.Item().Text($"Status: {request.Status}");
                });
            });
        }

        private void CreateStatusBadge(IContainer container, Enums.ApplicationStatus status)
        {
            var (text, color) = status switch
            {
                Enums.ApplicationStatus.Pending => ("PENDING", Colors.Yellow.Darken2),
                Enums.ApplicationStatus.Approved => ("APPROVED", Colors.Green.Darken1),
                Enums.ApplicationStatus.Rejected => ("REJECTED", Colors.Red.Darken1),
                _ => (status.ToString(), Colors.Grey.Medium)
            };

            container
                .Background(color)
                .PaddingHorizontal(5)
                .PaddingVertical(3)
                .Text(text)
                .FontColor(Colors.White)
                .SemiBold()
                .FontSize(10);
        }
    }
}
