using Netgo.Application.Models;

namespace Netgo.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> Send(Email email);
    }
}
