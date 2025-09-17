using Netgo.Application.Contracts.Persistence;
using Netgo.Domain;

namespace Netgo.Persistence.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(NetgoDbContext context) : base(context)
        {
        }
    }
}
