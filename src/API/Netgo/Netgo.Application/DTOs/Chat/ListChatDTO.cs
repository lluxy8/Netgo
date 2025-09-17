namespace Netgo.Application.DTOs.Chat
{
    public class ListChatDTO
    {
        public Guid Id { get; set; }
        public Guid FirstUserId { get; set; }
        public Guid SecondUserId { get; set; }
    }
}
