using MediatR;

namespace Netgo.Domain.Events.Products
{
    public record ProductSoldEvent(Product Product) : INotification;
}
