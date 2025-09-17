using Netgo.Application.DTOs.Message;

namespace Netgo.Application.DTOs.Chat
{
    public class ChatDTO
    {
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
        public required List<MessageDTO> Messages { get; set; }
    }
}
