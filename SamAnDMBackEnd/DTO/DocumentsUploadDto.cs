using System.Text.Json.Serialization;

namespace SamAnDMBackEnd.DTO
{
    public class DocumentsUploadDto
    {
        public string DocumentName { get; set; }
        public required string Address { get; set; }
        public IFormFile DocumentFile { get; set; }
    }
}
