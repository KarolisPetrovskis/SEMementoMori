using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace MementoMori.Server.Database
{
    public class AppDbContext : DbContext 
    {
        protected readonly IConfiguration Configuration;
        
        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Card> Cards { get; set; }

        public DbSet<UserCardData> UserCards { get; set; }

        public DbSet<User> Users { get; set; }
        
        //fix to always use utc time
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
            {
                property.SetValueConverter(new ValueConverter<DateTime, DateTime>(
                    v => v.ToUniversalTime(), // Convert to UTC when saving
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc))); // Convert to UTC when reading
            }

            base.OnModelCreating(modelBuilder);
}
    }

}
