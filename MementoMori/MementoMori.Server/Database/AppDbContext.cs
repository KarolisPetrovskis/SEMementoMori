using MementoMori.Server.Exceptions;
using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MementoMori.Server.Database
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options ?? throw new ArgumentNullException(nameof(options))) { }
        public void SecureUpdate<T, P>(P item, Guid changedBy) where T : P where P : DatabaseObject
        {
            var entity = Set<T>().FirstOrDefault(e => e.Id == item.Id);
            if (entity == null)
            {
                return;
            }

            if (!entity.CanEdit(changedBy)) 
            {
                throw new UnauthorizedEditingException();
            }

            Entry(entity).CurrentValues.SetValues(item);
            return;
        }

        public void Remove<T>(Guid id) where T : DatabaseObject
        {
            var entity = Set<T>().FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                return;
            }
            Remove(entity);
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
