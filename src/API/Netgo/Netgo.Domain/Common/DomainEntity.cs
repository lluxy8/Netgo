using MediatR;

namespace Netgo.Domain.Common
{
    public abstract class DomainEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        private List<INotification> _domainEvents = new List<INotification>();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        protected void AddDomainEvent(INotification domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(INotification domainEvent)
        {
            _domainEvents?.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}
