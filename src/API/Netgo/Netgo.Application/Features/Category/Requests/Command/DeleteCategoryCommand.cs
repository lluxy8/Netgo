using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Category;

namespace Netgo.Application.Features.Category.Requests.Command
{
    public class DeleteCategoryCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
    }
}
