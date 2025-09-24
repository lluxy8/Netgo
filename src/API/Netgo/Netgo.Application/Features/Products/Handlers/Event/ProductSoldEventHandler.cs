using MediatR;
using Microsoft.Extensions.Logging;
using Netgo.Application.Contracts.Identity;
using Netgo.Application.Contracts.Infrastructure;
using Netgo.Domain.Events.Products;

namespace Netgo.Application.Features.Products.Handlers.Event
{
    public class ProductSoldEventHandler : INotificationHandler<ProductSoldEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ProductCreatedEventHandler> _logger;
        private readonly IUserService _usersService;

        public ProductSoldEventHandler(
            IEmailService emailService,
            ILogger<ProductCreatedEventHandler> logger,
            IUserService userService)
        {
            _emailService = emailService;
            _logger = logger;
            _usersService = userService;
        }
        public async Task Handle(ProductSoldEvent notification, CancellationToken cancellationToken)
        {
            var user = await _usersService.GetUser(notification.Product.UserId.ToString());

            _logger.LogInformation("A product has marked as sold {Product}.", notification.Product);

            await _emailService.Send(new Models.Email
            {
                To = user.Email,
                Body = $"""
                    Hi {user.FirstName} {user.LastName}, Your product has been marked as sold. '{notification.Product.Title}'.
                        ~ Netgo 
                """,
                Subject = "Netgo ~ Product marked sold"
            });
        }
    }
}
