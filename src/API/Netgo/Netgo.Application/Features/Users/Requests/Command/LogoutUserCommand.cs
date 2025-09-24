using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netgo.Application.Features.Users.Requests.Command
{
    public class LogoutUserCommand
    {
        public required string Token { get; set; }
    }
}
