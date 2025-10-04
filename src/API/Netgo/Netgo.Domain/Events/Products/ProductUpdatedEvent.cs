using MediatR;

namespace Netgo.Domain.Events.Products
{
    public record ProductUpdatedEvent(Product OldProduct, Product NewProduct) : INotification;
}
