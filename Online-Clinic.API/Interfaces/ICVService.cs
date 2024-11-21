namespace Online_Clinic.API.Interfaces
{
    public interface ICVService
    {
        Dictionary<string, string> ParseExperienceFromPdf(byte[] cvAsBytes);

        IFormFile ConvertByteArrayToFormFile(byte[] fileBytes);

        string ExtractTextFromPdf(IFormFile cv);

        string ExtractExperienceSection(string text);
    }
}
