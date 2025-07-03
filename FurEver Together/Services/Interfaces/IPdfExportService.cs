namespace FurEver_Together.Services.Interfaces
{
    public interface IPdfExportService
    {
        byte[] GeneratePdfFromHtml(string htmlContent, string title = "Document");
    }
}
