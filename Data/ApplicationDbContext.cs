using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartAlertAPI.Models;
using System.Reflection.Emit;

namespace SmartAlertAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(u => u.IncidentsReported)
                .WithMany(u => u.ReportedByUsers);

            builder.Entity<DangerCategory>()
                .HasMany(u => u.Incidents)
                .WithOne(u => u.Category)
                .HasForeignKey(e => e.CategoryId);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                // equivalent of modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                entityType.SetTableName(entityType.DisplayName());

                // equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
                entityType.GetForeignKeys()
                    .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                    .ToList()
                    .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
            }
        }

        public DbSet<Incident> Incidents { get; set; }
        public DbSet<DangerCategory> DangerCategories {  get; set; }
        public DbSet<User> Users {  get; set; }
        public DbSet<TokenBlackList> TokenBlackList { get; set; }
    }
}
