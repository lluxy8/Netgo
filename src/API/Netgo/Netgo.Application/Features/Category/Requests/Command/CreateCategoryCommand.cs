using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Category;

namespace Netgo.Application.Features.Category.Requests.Command
{
    public class CreateCategoryCommand : IRequest<Result>
    {
        public required CategoryCreateDTO CategoryDto { get; set; }
    }
}
