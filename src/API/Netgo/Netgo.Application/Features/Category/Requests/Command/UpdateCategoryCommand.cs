using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Category;

namespace Netgo.Application.Features.Category.Requests.Command
{
    public class UpdateCategoryCommand : IRequest<Result>
    {
        public required CategoryUpdateDTO CategoryDTO { get; set; }
    }
}
