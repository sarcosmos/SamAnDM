namespace SamAnDMBackEnd.Model
{
    public class FamilyGroup
    {
        public int FamilyGroupId { get; set; }
        public string Members { get; set; }

        public int UserId { get; set; } // Foreign Key
        public Users Users { get; set; }

        public int DocumentId { get; set; } // Foreign Key
        public Documents Documents { get; set; }
    }
}
