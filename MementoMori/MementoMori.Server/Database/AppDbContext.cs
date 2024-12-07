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
    public override int SaveChanges()
    {
        PerformCascadingDeletes();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        PerformCascadingDeletes();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void PerformCascadingDeletes()
    {
        var deletedDecks = ChangeTracker.Entries<Deck>()
            .Where(e => e.State == EntityState.Deleted)
            .Select(e => e.Entity)
            .ToList();
        foreach (var deck in deletedDecks)
        {
            var relatedCards = Cards.Where(c => c.DeckId == deck.Id).ToList();
            Cards.RemoveRange(relatedCards);
        }
    }
        public async Task SecureUpdateAsync<T, P>(P item, Guid changedBy) where T : P where P : DatabaseObject
        {
            var entity = await Set<T>().FirstOrDefaultAsync(e => e.Id == item.Id);
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

        public async Task RemoveAsync<T>(Guid id, Guid changedBy) where T : DatabaseObject
        {
            var entity = await Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            if (entity == null)
            {
                return;
            }
            if (!entity.CanEdit(changedBy)) 
            {
                throw new UnauthorizedEditingException();
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
