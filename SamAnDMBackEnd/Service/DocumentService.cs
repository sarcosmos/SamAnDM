using SamAnDMBackEnd.DTO;
using SamAnDMBackEnd.Model;
using SamAnDMBackEnd.Repository;
using System.Security.Claims;

namespace SamAnDMBackEnd.Service
{
    public interface IDocumentService
    {
        Task<IEnumerable<Documents>> GetAllDocumentsAsync();
        Task<Documents> GetDocumentByIdAsync(int id);
        Task<Documents> UploadDocumentsAsync(DocumentsUploadDto documentUploadDto);
        Task<Documents> UpdateDocumentAsync(int id, DocumentsUpdateDto documentUpdateDto);
        Task SoftDeleteDocumentAsync(int id);
    }
    public class DocumentService : IDocumentService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDocumentsRepository _documentsRepository;

        public DocumentService(IHttpContextAccessor httpContextAccessor, IDocumentsRepository documentsRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _documentsRepository = documentsRepository;
        }

        public async Task<IEnumerable<Documents>> GetAllDocumentsAsync()
        {
            return await _documentsRepository.GetAllDocumentsAsync();
        }

        public async Task<Documents> GetDocumentByIdAsync(int id)
        {
            return await _documentsRepository.GetDocumentsByIdAsync(id);
        }

        public async Task SoftDeleteDocumentAsync(int id)
        {
           await _documentsRepository.SoftDeleteDocumentsAsync(id);
        }

        public async Task<Documents> UpdateDocumentAsync(int id, DocumentsUpdateDto documentUpdateDto)
        {
            var document = await _documentsRepository.GetDocumentsByIdAsync(id);
            if (document == null) throw new Exception("Documento no encontrado.");

            document.DocumentName = documentUpdateDto.DocumentName;
            document.IsProtected = documentUpdateDto.IsProtected;
            document.Address = documentUpdateDto.Address;

            await _documentsRepository.UpdateDocumentsAsync(document);
            return document;
        }

        public async Task<Documents> UploadDocumentsAsync(DocumentsUploadDto documentUploadDto)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var document = new Documents
            {
                DocumentName = documentUploadDto.DocumentName,
                IsProtected = false,
                Address = documentUploadDto.Address,
                IsDeleted = false,
                DocumentSize = documentUploadDto.DocumentFile.Length,
                OwnerUserId = userId // Asigna el usuario propietario
            };

            // Obtener la extensión del archivo
            var fileExtension = Path.GetExtension(documentUploadDto.DocumentFile.FileName);
            document.TypeDocument = fileExtension;


            using (var memoryStream = new MemoryStream())
            {
                await documentUploadDto.DocumentFile.CopyToAsync(memoryStream);
                document.DocumentContent = memoryStream.ToArray();
            }

            await _documentsRepository.UpdateDocumentsAsync(document);

            // Retorna el documento recién creado
            return document;
        }
    }
}
