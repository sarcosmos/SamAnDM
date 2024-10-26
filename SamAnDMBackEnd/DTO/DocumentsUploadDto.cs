namespace SamAnDMBackEnd.DTO
{
    public class DocumentsUploadDto
    {
        public string DocumentName { get; set; }
        public bool IsProtected { get; set; }
        public bool IsShared { get; set; }
        public required string Address { get; set; }
        public required string TypeDocument { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IFormFile DocumentFile { get; set; }
    }
}
