using System.Data.Entity.Migrations;

namespace SantaSystem.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<SantaSystemDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
