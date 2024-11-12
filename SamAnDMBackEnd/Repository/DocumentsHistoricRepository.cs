using SamAnDMBackEnd.Context;
using SamAnDMBackEnd.Model;

namespace SamAnDMBackEnd.Repository
{
    public interface IDocumentsHistoricRepository
    {
        Task AddAsync(DocumentsHistorics documentsHistoric);
    }

    public class DocumentsHistoricRepository : IDocumentsHistoricRepository
    {
        private readonly DbContextDM _context;

        public DocumentsHistoricRepository(DbContextDM context)
        {
            _context = context;
        }

        public async Task AddAsync(DocumentsHistorics documentsHasHistoric)
        {
            _context.DocumentsHistorics.Add(documentsHasHistoric);
            await _context.SaveChangesAsync();
        }
    }
}

