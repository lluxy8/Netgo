using Netgo.Application.Contracts.Infrastructure;
using Netgo.Application.Models;

namespace Netgo.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task<bool> Send(Email email)
        {
            await Task.Delay(1);
            Console.WriteLine(
                $"imulating sending email to {email.To} with subject '{email.Subject}' and body '{email.Body}'");
            return true;
        }
    }
}
