namespace SantaSystem.Data
{
    public class DbFactory : IDbFactory
    {
        SantaSystemDbContext dbContext;

        public SantaSystemDbContext GetContext()
        {
            return dbContext ?? (dbContext = new SantaSystemDbContext());
        }
    }
}
