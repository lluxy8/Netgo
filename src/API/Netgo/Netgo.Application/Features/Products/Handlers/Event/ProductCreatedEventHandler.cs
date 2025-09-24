using MediatR;
using Microsoft.Extensions.Logging;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Contracts.Infrastructure;
using Netgo.Application.Models.Identity;
using Netgo.Domain;
using Netgo.Domain.Events.Products;

namespace Netgo.Application.Features.Products.Handlers.Event
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly IUserService _usersService;

        public ProductCreatedEventHandler(
            IEmailService emailService, 
            ILogger<ProductCreatedEventHandler> logger,
            IUserService userService)
        {
            _emailService = emailService;
            _logger = logger;
            _usersService = userService;
        }

        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            var user = await _usersService.GetUser(notification.product.UserId.ToString());

            _logger.LogInformation("New product has been created {Product}.", notification.product);

            await _emailService.Send(new Models.Email
            {
                To = user.Email,
                Body = $"""
                    Hi {user.FirstName} {user.LastName}, your product has been created successfully '{notification.product.Title}'.
                        ~ Netgo 
                """,
                Subject = "Netgo ~ New product created."
            });
        }
    }
}
