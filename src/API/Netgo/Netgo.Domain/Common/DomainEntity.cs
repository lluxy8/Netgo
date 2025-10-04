namespace Netgo.Domain.Common
{
    public abstract class DomainEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
