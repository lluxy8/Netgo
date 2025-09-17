using Netgo.Domain.Common;

namespace Netgo.Domain
{
    public class Message : DomainEntity
    {
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        public string Content { get; set; }
        public string DisplayContent { get; set; }
        public List<string> OldContents { get; set; } = [];
        public DateTime? DateRead { get; set; }
        public DateTime? DateDeleted { get; set; } = null;
        public DateTime? DateModified { get; set; } = null;
        public Chat Chat { get; set; }
    }
}
