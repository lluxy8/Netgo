using AutoMapper;
using Netgo.Application.DTOs.Category;
using Netgo.Application.DTOs.Chat;
using Netgo.Application.DTOs.Message;
using Netgo.Application.DTOs.Product;

namespace Netgo.Application.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Domain.Product, ProductDTO>().ReverseMap();
            CreateMap<Domain.Product, ListProductDTO>().ReverseMap();
            CreateMap<Domain.Product, CreateProductDTO>().ReverseMap();
            CreateMap<Domain.Product, UpdateProductDTO>().ReverseMap();
            CreateMap<Domain.ProductDetail, ProductDetailDto>().ReverseMap();

            CreateMap<Domain.Message, MessageDTO>().ReverseMap().ReverseMap();
            CreateMap<Domain.Message, CreateMessageDTO>()
                .ForMember(d => d.From, o => o.MapFrom(s => s.UserId))
                .ReverseMap();

            CreateMap<Domain.Category, CategoryDTO>().ReverseMap();
            CreateMap<Domain.Category, ListCategoryDTO>().ReverseMap();
            CreateMap<Domain.Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Domain.Category, CategoryUpdateDTO>().ReverseMap();

            CreateMap<Domain.Chat, ChatDTO>().ReverseMap();
            CreateMap<Domain.Chat, ListChatDTO>().ReverseMap();
        }
    }
}
