using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MementoMori.Server.Database
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options ?? throw new ArgumentNullException(nameof(options))) { }
        public void Update<T, P>(P item) where T : P where P : DatabaseObject
        {
            var entity = Set<T>().FirstOrDefault(e => e.Id == item.Id);
            if (entity == null)
            {
                return;
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
        public DbSet<User> Users { get; set; }
    }

}
