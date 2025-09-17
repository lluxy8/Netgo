using Netgo.Domain.Common;

namespace Netgo.Domain
{
    public class Chat : DomainEntity
    {
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
        public List<Message> Messages { get; set; }
    }
}
