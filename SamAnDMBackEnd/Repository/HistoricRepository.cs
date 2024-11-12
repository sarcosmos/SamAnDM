using SamAnDMBackEnd.Context;
using SamAnDMBackEnd.Model;

namespace SamAnDMBackEnd.Repository
{
    public interface IHistoricRepository
    {
        Task AddAsync(Historics historic);
    }
    public class HistoricRepository : IHistoricRepository
    {
        private readonly DbContextDM _context;

        public HistoricRepository(DbContextDM context)
        {
            _context = context;
        }
        public async Task AddAsync(Historics historic)
        {
            _context.Historics.Add(historic);
            await _context.SaveChangesAsync();
        }
    }
}
