using MementoMori.Server.Exceptions;
using MementoMori.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MementoMori.Server.Database
{
    public class AppDbContext : DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options ?? throw new ArgumentNullException(nameof(options))) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>()
                .HasOne<Deck>()
                .WithMany(d => d.Cards)
                .HasForeignKey(c => c.DeckId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<User>().ToTable("Users");
            base.OnModelCreating(modelBuilder);
        }
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
        public DbSet<User> Users { get; set; }
    }

}
