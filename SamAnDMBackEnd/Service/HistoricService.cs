using SamAnDMBackEnd.Model;
using SamAnDMBackEnd.Repository;

namespace SamAnDMBackEnd.Service
{
    public interface IHistoricService
    {
        Task RegisterHistoricAsync(int documentId, string action, int userId, string reference);
    }

    public class HistoricService : IHistoricService
    {
        private readonly IHistoricRepository _historicRepository;
        private readonly IDocumentsHistoricRepository _documentsHasHistoricRepository;

        public HistoricService(IHistoricRepository historicRepository, IDocumentsHistoricRepository documentsHasHistoricRepository)
        {
            _historicRepository = historicRepository;
            _documentsHasHistoricRepository = documentsHasHistoricRepository;
        }

        public async Task RegisterHistoricAsync(int documentId, string action, int userId, string reference)
        {
            var historicRecord = new Historics
            {
                DateCreated = DateTime.Now,
                UserModification = userId.ToString(),
                DateModification = DateTime.Now,
                Action = action,
                ReferenceId = reference
            };

            await _historicRepository.AddAsync(historicRecord);

            // Crear el enlace en la tabla intermedia
            var documentsHasHistoric = new DocumentsHistorics
            {
                DocumentId = documentId,
                HistoricId = historicRecord.HistoricId
            };

            await _documentsHasHistoricRepository.AddAsync(documentsHasHistoric);
        }
    }

}
