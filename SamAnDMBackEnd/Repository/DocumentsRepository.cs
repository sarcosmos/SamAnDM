using Microsoft.EntityFrameworkCore;
using SamAnDMBackEnd.Context;
using SamAnDMBackEnd.Model;


namespace SamAnDMBackEnd.Repository
{
    public interface IDocumentsRepository
    {
        Task<IEnumerable<Documents>> GetAllDocumentsAsync();
        Task<Documents> GetDocumentsByIdAsync(int id);
        Task UploadDocumentsAsync(Documents documents);
        Task UpdateDocumentsAsync(Documents documents);
        Task SoftDeleteDocumentsAsync(int id);
    }
    public class DocumentsRepository : IDocumentsRepository
    {
        private readonly DbContextDM _context;

        public DocumentsRepository(DbContextDM context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Documents>> GetAllDocumentsAsync()
        {
            return await _context.Documents
                .Where(d => !d.IsDeleted)
                .ToListAsync();
        }

        public async Task<Documents> GetDocumentsByIdAsync(int id)
        {
            return await _context.Documents
                .FirstOrDefaultAsync(d => d.DocumentId == id && !d.IsDeleted);
        }

        public async Task SoftDeleteDocumentsAsync(int id)
        {
            var documents = await _context.Documents.FindAsync(id);
            if (documents != null) 
            {
                documents.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateDocumentsAsync(Documents documents)
        {
            _context.Documents.Update(documents);
            await _context.SaveChangesAsync();
        }

        public async Task UploadDocumentsAsync(Documents documents)
        {
            await _context.Documents.AddAsync(documents);
            await _context.SaveChangesAsync();
        }
    }
}
