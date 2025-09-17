using MediatR;
using Netgo.Application.Common;
using Netgo.Application.DTOs.Category;

namespace Netgo.Application.Features.Category.Requests.Query
{
    public class GetCategoriesQuery : IRequest<Result<List<ListCategoryDTO>>>
    {
    }
}
