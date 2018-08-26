using System.Data.Entity;
using SantaSystem.Models.DomainModels;
using Microsoft.AspNet.Identity.EntityFramework;
using SantaSystem.Data.Migrations;

namespace SantaSystem.Data
{
    public class SantaSystemDbContext : IdentityDbContext<User>
    {
        public virtual IDbSet<Group> Groups { get; set; }

        public SantaSystemDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<SantaSystemDbContext, Configuration>());
        }

        public static SantaSystemDbContext Create()
        {
            return new SantaSystemDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasRequired(c => c.Creator)
                .WithMany(p => p.CreatedGroups)
                .HasForeignKey(c => c.CreatorId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
