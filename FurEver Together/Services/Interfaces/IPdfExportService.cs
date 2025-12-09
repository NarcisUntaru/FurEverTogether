namespace FurEver_Together.Services.Interfaces
{
    public interface IPdfExportService
    {
        byte[] GenerateAdoptionRequestsPdf(IEnumerable<DataModels.Adoption> adoptionRequests);
    }
}
