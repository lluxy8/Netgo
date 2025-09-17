using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Category;

namespace Netgo.Application.Features.Category.Requests.Query
{
    public class GetCategoryByIdQuery : IRequest<Result<CategoryDTO>>
    {
        public Guid Id { get; set; }
    }
}
