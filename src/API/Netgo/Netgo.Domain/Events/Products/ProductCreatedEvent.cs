using MediatR;

namespace Netgo.Domain.Events.Products
{
    public record ProductCreatedEvent(Product product) : INotification;
}
