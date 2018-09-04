using System.Data.Entity;
using SantaSystem.Models.DomainModels;
using Microsoft.AspNet.Identity.EntityFramework;
using SantaSystem.Data.Migrations;

namespace SantaSystem.Data
{
    public class SantaSystemDbContext : IdentityDbContext<User>
    {
        public virtual IDbSet<Group> Groups { get; set; }
        public virtual IDbSet<Invitation> Invitations { get; set; }

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
                .HasForeignKey(c => c.CreatorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Invitation>()
                .HasRequired(c => c.User)
                .WithMany(p => p.Invitations)
                .HasForeignKey(c => c.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Group>()
                .HasMany(t => t.Members)
                .WithMany(t => t.MemberOfGroups)
                .Map(m =>
                {
                    m.ToTable("MemberGroups");
                    m.MapLeftKey("GroupId");
                    m.MapRightKey("UserId");
                });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
