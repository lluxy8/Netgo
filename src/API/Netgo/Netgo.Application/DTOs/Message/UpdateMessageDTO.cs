namespace Netgo.Application.DTOs.Message
{
    public class UpdateMessageDTO
    {
        public Guid Id { get; set; }
        public required string Content { get; set; }
    }
}
