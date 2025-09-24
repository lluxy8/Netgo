using MediatR;
using Microsoft.Extensions.Logging;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Contracts.Infrastructure;
using Netgo.Domain.Events.Products;

namespace Netgo.Application.Features.Products.Handlers.Event
{
    public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly IUserService _usersService;

        public ProductUpdatedEventHandler(
            IEmailService emailService,
            ILogger<ProductCreatedEventHandler> logger,
            IUserService userService)
        {
            _emailService = emailService;
            _logger = logger;
            _usersService = userService;
        }
        public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
        {
            var user = await _usersService.GetUser(notification.NewProduct.UserId.ToString());

            _logger.LogInformation("A product has been updated {Old} | {New}.", notification.OldProduct, notification.NewProduct);

            await _emailService.Send(new Models.Email
            {
                To = user.Email,
                Body = $"""
                    Hi {user.FirstName} {user.LastName}, Your product has been updated successfully. '{notification.NewProduct.Title}'.
                        ~ Netgo 
                """,
                Subject = "Netgo ~ Product updated.."
            });
        }
    }
}
