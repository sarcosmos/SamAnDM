namespace SamAnDMBackEnd.Model
{
    public class Historics
    {
        public int HistoricId { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserModification { get; set; }
        public DateTime DateModification { get; set; }
        public string Action { get; set; }
        public string ReferenceId { get; set; }

        public ICollection<DocumentsHistorics> DocumentsHistorics { get; set; } 
    }
}
