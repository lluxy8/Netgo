namespace Netgo.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        public IProductRepository Products { get; }
        public IProductDetailRepository ProductDetails { get; }
        public ICategoryRepository Categories { get; }
        public IChatRepository Chats { get; }
        public IMessageRepository Messages { get; }
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
