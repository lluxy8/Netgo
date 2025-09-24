using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using Netgo.API.Hubs;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Message;
using Netgo.Application.Features.Chat.Requests.Command;
using Netgo.Application.Features.Chat.Requests.Query;
using Netgo.Application.Features.Chats.Requests.Command;

namespace Netgo.API.Controllers
{
    [ApiController]
    [Route("api/chat")]
    [Authorize(Roles = UserRoles.User)]
    [EnableRateLimiting("sliding")]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatController(IMediator mediator, IHubContext<ChatHub> hubContext)
        {
            _mediator = mediator;
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDTO message)
        {
            var request = new CreateMessageCommand { MessageDTO = message };
            var result = await _mediator.Send(request);
            if(result.IsSuccess)
                await _hubContext.Clients.All.SendAsync("MessageSended", new MessageDTO
                {
                    Id = result.Data.MessageId,
                    ChatId = result.Data.ChatId,
                    Content = message.Content,
                    DisplayContent = message.Content
                });

            return new ResultActionResult(result);  
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMessage(UpdateMessageDTO message)
        {
            var request = new UpdateMessageCommand { MessageDTO = message };
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
                await _hubContext.Clients.All.SendAsync("MessageUpdated", new MessageDTO
                {
                    Id = result.Data.MessageId,
                    ChatId = result.Data.ChatId,
                    Content = message.Content,
                    DisplayContent = message.Content
                });

            return new ResultActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatById(Guid id)
        {
            var request = new GetChatByIdQuery { Id = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetChatByUserId(Guid id)
        {
            var request = new GetChatsByUserIdQuery { Id = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var request = new DeleteMessageCommand { Id = id };
            var result = await _mediator.Send(request);
            return new ResultActionResult(result);
        }
    }
}
