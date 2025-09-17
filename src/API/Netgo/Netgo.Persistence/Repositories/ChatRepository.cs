using Microsoft.EntityFrameworkCore;
using Netgo.Application.Contracts.Persistence;
using Netgo.Domain;

namespace Netgo.Persistence.Repositories
{
    public class ChatRepository : GenericRepository<Chat>, IChatRepository
    {
        private readonly NetgoDbContext _context;
        public ChatRepository(NetgoDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<List<Chat>> GetChatsByUserId(Guid id)
            => await _context.Chats
                .Where(x => x.FirstUserId == id || x.SecondUserId == id)
                .OrderByDescending(x => x.Messages.Max(m => (DateTime?)m.DateCreated))
                .ToListAsync();

        public Task<Chat?> GetChatWithMessages(Guid id)
            => _context.Chats
                    .Include(x => x.Messages)
                    .FirstOrDefaultAsync(x => x.Id == id);

        public Task<Chat?> GetChatByUsers(Guid firstUser, Guid secondUser)
            => _context.Chats.FirstOrDefaultAsync(x =>
                (x.FirstUserId == firstUser && x.SecondUserId == secondUser) ||
                (x.FirstUserId == secondUser && x.SecondUserId == firstUser));

    }
}
