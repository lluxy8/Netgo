using Netgo.Domain;

namespace Netgo.Application.Contracts.Persistence
{
    public interface IChatRepository : IGenericRepository<Chat>
    {
        Task<List<Chat>> GetChatsByUserId(Guid id);
        Task<Chat?> GetChatWithMessages(Guid id);
        Task<Chat?> GetChatByUsers(Guid firstUser, Guid secondUser);
    }
}
