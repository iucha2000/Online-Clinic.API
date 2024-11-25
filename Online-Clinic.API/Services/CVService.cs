using Online_Clinic.API.Interfaces;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using static System.Net.Mime.MediaTypeNames;

namespace Online_Clinic.API.Services
{
    public class CVService : ICVService
    {

        public Dictionary<string, string> ParseExperienceFromPdf(byte[] cvAsBytes)
        {
            var experiences = new Dictionary<string, string>();

            IFormFile cv = ConvertByteArrayToFormFile(cvAsBytes);

            string extractedText = ExtractTextFromPdf(cv);
            var experienceSection = ExtractExperienceSection(extractedText);

            var regex = new Regex(@"(\d{4}\s*-\s*\d{4}|\d{4}\s*-\s*(Present|დღემდე)?)\s*[-,\s]*\s*([^\d]+(?:,\s*[^\d]+)*)");
            var matches = regex.Matches(experienceSection);

            foreach (Match match in matches)
            {
                string timePeriod = match.Groups[1].Value.Trim();
                string description = match.Groups[3].Value.Trim();
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
            var startKeywords = new List<string> { "Experience", "History", "გამოცდილება" };
            var endKeywords = new List<string> {"Education", "განათლება", "Skills", "უნარები", "  "};

            foreach (var startkeyword in startKeywords)
            {
                if (text.Contains(startkeyword))
                {
                    int startKeywordIndex = text.IndexOf(startkeyword) + startkeyword.Length;
                    
                    foreach (var endKeyword in endKeywords)
                    {
                        if(text.IndexOf(endKeyword, startKeywordIndex) != -1)
                        {
                            int endKeywordIndex = text.IndexOf(endKeyword, startKeywordIndex);

                            return text.Substring(startKeywordIndex, endKeywordIndex - startKeywordIndex);
                        }
                    }

                    return text.Substring(startKeywordIndex);
                }
            }

            return string.Empty;
        }
    }
}
