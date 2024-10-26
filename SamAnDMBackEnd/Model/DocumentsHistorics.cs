namespace SamAnDMBackEnd.Model
{
    public class DocumentsHistorics
    {
        public int DocumentId { get; set; }
        public Documents Documents { get; set; }

        public int HistoricId { get; set; }
        public Historics Historics { get; set; }
    }
}
