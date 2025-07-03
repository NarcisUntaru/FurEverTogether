using DinkToPdf;
using DinkToPdf.Contracts;
using FurEver_Together.Services.Interfaces;

public class PdfExportService : IPdfExportService
{
    private readonly IConverter _converter;

    public PdfExportService(IConverter converter)
    {
        _converter = converter;
    }

    public byte[] GeneratePdfFromHtml(string htmlContent, string title = "Document")
    {
        var globalSettings = new GlobalSettings
        {
            ColorMode = ColorMode.Color,
            Orientation = Orientation.Portrait,
            PaperSize = PaperKind.A4,
            Margins = new MarginSettings { Top = 10 },
            DocumentTitle = title
        };

        var objectSettings = new ObjectSettings
        {
            PagesCount = true,
            HtmlContent = htmlContent,
            WebSettings = { DefaultEncoding = "utf-8" }
        };

        var pdfDoc = new HtmlToPdfDocument
        {
            GlobalSettings = globalSettings,
            Objects = { objectSettings }
        };

        return _converter.Convert(pdfDoc);
    }
}
