
using SamAnDMBackEnd.DTO;
using SamAnDMBackEnd.Model;
using SamAnDMBackEnd.Repository;

namespace SamAnDMBackEnd.Service
{
    public interface IDocumentService
    {
        Task<IEnumerable<Documents>> GetAllDocumentsAsync();
        Task<Documents> GetDocumentByIdAsync(int id);
        Task UploadDocumentsAsync(DocumentsUploadDto documentUploadDto);
        Task UpdateDocumentAsync(int id, DocumentsUploadDto documentUploadDto);
        Task SoftDeleteDocumentAsync(int id);
    }
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentsRepository _documentsRepository;

        public DocumentService(IDocumentsRepository documentsRepository)
        {
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

        public async Task UpdateDocumentAsync(int id, DocumentsUploadDto documentUploadDto)
        {
            var document = await _documentsRepository.GetDocumentsByIdAsync(id);
            if (document == null) throw new Exception("Documento no encontrado.");

            document.DocumentName = documentUploadDto.DocumentName;
            document.IsProtected = documentUploadDto.IsProtected;
            document.IsShared = documentUploadDto.IsShared;
            document.Address = documentUploadDto.Address;
            document.TypeDocument = documentUploadDto.TypeDocument;
            document.IsDeleted = documentUploadDto.IsDeleted;
            document.DocumentSize = documentUploadDto.DocumentFile.Length;

            using (var memoryStream  = new MemoryStream())
            {
                await documentUploadDto.DocumentFile.CopyToAsync(memoryStream);
                document.DocumentContent = memoryStream.ToArray();
            }

            await _documentsRepository.UpdateDocumentsAsync(document);
        }

        public async Task UploadDocumentsAsync(DocumentsUploadDto documentUploadDto)
        {
            var document = new Documents
            {
                DocumentName = documentUploadDto.DocumentName,
                IsProtected = documentUploadDto.IsProtected,
                IsShared = documentUploadDto.IsShared,
                Address = documentUploadDto.Address,
                TypeDocument = documentUploadDto.TypeDocument,
                IsDeleted = documentUploadDto.IsDeleted,
                DocumentSize = documentUploadDto.DocumentFile.Length,
            };

            using (var memoryStream = new MemoryStream())
            {
                await documentUploadDto.DocumentFile.CopyToAsync(memoryStream);
                document.DocumentContent = memoryStream.ToArray();
            }

            await _documentsRepository.UpdateDocumentsAsync(document);
        }
    }
}
