namespace SamAnDMBackEnd.DTO
{
    public class DocumentsUpdateDto
    {
        public string DocumentName { get; set; }
        public bool IsProtected { get; set; }
        public required string Address { get; set; }
    }
}
