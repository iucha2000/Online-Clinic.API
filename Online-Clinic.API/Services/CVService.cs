using Online_Clinic.API.Interfaces;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using static System.Net.Mime.MediaTypeNames;

namespace Online_Clinic.API.Services
{
    public class CVService : ICVService
    {

        public Dictionary<string, string> ParseExperienceFromPdf(byte[] cvAsBytes)
        {
            //TODO fix pattern matching for every scenario (and uncomment cache for experience parsing request)
            var experiences = new Dictionary<string, string>();

            IFormFile cv = ConvertByteArrayToFormFile(cvAsBytes);

            string extractedText = ExtractTextFromPdf(cv);
            //extractedText = extractedText.Replace("\r\n", "\n").Replace("\r", "\n").Replace("–", "-").Trim
            var experienceSection = ExtractExperienceSection(extractedText);

            var regex = new Regex(@"(\d{4}-\d{4}|\d{4}\s*[-,]\s*(Present|დღემდე)?)\s*[-,\s]*\s*([^-\d]+(?:,\s*[^-\d]+)*)");
            var matches = regex.Matches(experienceSection);

            foreach (Match match in matches)
            {
                string timePeriod = match.Groups[1].Value;
                string description = match.Groups[3].Value;
                experiences[timePeriod] = description;
            }

            return experiences;

        }
        public IFormFile ConvertByteArrayToFormFile(byte[] fileBytes)
        {
            var stream = new MemoryStream(fileBytes);
            var file = new FormFile(stream, 0, fileBytes.Length, "file", "cv")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };
            return file;
        }

        public string ExtractTextFromPdf(IFormFile cv)
        {
            using (var stream = cv.OpenReadStream())
            using (var pdfDocument = PdfDocument.Open(stream))
            {
                var text = string.Empty;
                foreach (var page in pdfDocument.GetPages())
                {
                    text += page.Text;
                }
                return text;
            }
        }

        public string ExtractExperienceSection(string text)
        {
            var keywords = new List<string> { "Experience", "Work History", "Employment History", "გამოცდილება" };

            foreach (var keyword in keywords)
            {
                int startIndex = text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);

                if (startIndex > -1)
                {
                    int startAfterKeyword = startIndex + keyword.Length;
                    return text.Substring(startAfterKeyword).Trim();
                }
            }
            return string.Empty;
        }
    }
}
