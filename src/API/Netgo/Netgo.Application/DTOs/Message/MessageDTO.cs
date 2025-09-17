namespace Netgo.Application.DTOs.Message
{
    public class MessageDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChatId { get; set; }
        public required string Content { get; set; }
        public required string DisplayContent { get; set; }
        public List<string> OldContents { get; set; } = [];
        public DateTime? DateRead { get; set; }
        public DateTime? DateDeleted { get; set; } = null;
        public DateTime? DateModified { get; set; } = null;

    }
}
