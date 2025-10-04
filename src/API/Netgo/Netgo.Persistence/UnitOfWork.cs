using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Netgo.Application.Contracts.Persistence;

namespace Netgo.Persistence
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        private readonly NetgoDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public IProductRepository Products { get; }
        public IProductDetailRepository ProductDetails { get; }
        public ICategoryRepository Categories { get; }
        public IChatRepository Chats { get; }
        public IMessageRepository Messages { get; }

        public UnitOfWork(
            NetgoDbContext context,
            IProductRepository products,
            IProductDetailRepository productDetails,
            ICategoryRepository categories,
            IChatRepository chats,
            IMessageRepository messages)
        {
            _context = context;
            Products = products;
            ProductDetails = productDetails;
            Categories = categories;
            Chats = chats;
            Messages = messages;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null) return;
            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

<<<<<<< Updated upstream
=======
        public async Task DispatchDomainEventsAsync(CancellationToken cancellation = default)
        {
            var entities = _context.ChangeTracker
                .Entries<DomainEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0);

            var events = entities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            entities.ToList()
                .ForEach(x => x.Entity.ClearDomainEvents());

            foreach (var @event in events)
            {
                await _mediator.Publish(@event, cancellation);
            }

            foreach(var entity in entities)
            {
                entity.Entity.ClearDomainEvents();
            }
        }

>>>>>>> Stashed changes
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null) return;

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                await _currentTransaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync(cancellationToken);
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_currentTransaction != null)
                await _currentTransaction.DisposeAsync();

            await _context.DisposeAsync();
        }
    }

}
