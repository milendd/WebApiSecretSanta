namespace SantaSystem.Data
{
    public interface IDbFactory
    {
        SantaSystemDbContext GetContext();
    }
}
