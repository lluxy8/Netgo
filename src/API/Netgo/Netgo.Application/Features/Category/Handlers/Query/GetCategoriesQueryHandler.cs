using AutoMapper;
using MediatR;
using Netgo.Application.Common;
using Netgo.Application.Contracts.Persistence;
using Netgo.Application.DTOs.Category;
using Netgo.Application.Features.Category.Requests.Query;

namespace Netgo.Application.Features.Category.Handlers.Query
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, Result<List<ListCategoryDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ListCategoryDTO>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.Categories.GetAll();
            var categoriesDTO = _mapper.Map<List<ListCategoryDTO>>(categories);
            return Result<List<ListCategoryDTO>>.Success(categoriesDTO);
        }
    }
}
