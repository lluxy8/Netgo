namespace Netgo.Application.DTOs.Message
{
    public class CreateMessageDTO
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public required string Content { get; set; }
    }
}
