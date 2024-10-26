using Microsoft.EntityFrameworkCore;
using SamAnDMBackEnd.Model;

namespace SamAnDMBackEnd.Context
{
    public class DbContextDM : DbContext
    {
        public DbContextDM(DbContextOptions<DbContextDM> options) : base(options) { }

        public DbSet<Persons> Persons { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<UserTypePermissions> UserTypePermissions { get; set; }
        public DbSet<FamilyGroup> FamilyGroups { get; set; }
        public DbSet<Documents> Documents { get; set; }
        public DbSet<Historics> Historics { get; set; }
        public DbSet<DocumentsHistorics> DocumentsHistorics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Documents>()
            .HasKey(u => u.DocumentId);

            modelBuilder.Entity<Historics>()
            .HasKey(u => u.HistoricId);

            modelBuilder.Entity<Permissions>()
            .HasKey(u => u.PermissionId);

            modelBuilder.Entity<Persons>()
            .HasKey(u => u.PersonId);

            modelBuilder.Entity<Users>()
                .HasKey(u => u.UserId);

            // Relación Many-to-Many UserTypes-Permissions
            modelBuilder.Entity<UserTypePermissions>()
                .HasKey(utp => new { utp.UserTypeId, utp.PermissionId });

            modelBuilder.Entity<UserTypePermissions>()
                .HasOne(utp => utp.UserType)
                .WithMany(ut => ut.UserTypePermissions)
                .HasForeignKey(utp => utp.UserTypeId);

            modelBuilder.Entity<UserTypePermissions>()
                .HasOne(utp => utp.Permissions)
                .WithMany(p => p.UserTypePermissions)
                .HasForeignKey(utp => utp.PermissionId);

            // Relación Many-to-Many Document-Historic
            modelBuilder.Entity<DocumentsHistorics>()
                .HasKey(dh => new { dh.DocumentId, dh.HistoricId });

            modelBuilder.Entity<DocumentsHistorics>()
                .HasOne(dh => dh.Documents)
                .WithMany(d => d.DocumentsHistorics)
                .HasForeignKey(dh => dh.DocumentId);

            modelBuilder.Entity<DocumentsHistorics>()
                .HasOne(dh => dh.Historics)
                .WithMany(h => h.DocumentsHistorics)
                .HasForeignKey(dh => dh.HistoricId);
        }
    }
}
