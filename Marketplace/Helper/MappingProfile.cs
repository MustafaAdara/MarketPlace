using AutoMapper;
using Marketplace.Dtos;
using Marketplace.Models;

namespace Marketplace.Helper
{
    public class MappingProfile : Profile
    {

        public MappingProfile() 
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<Market, MarketDto>();
            CreateMap<MarketDto, Market>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<ShoppingCartDto, ShoppingCart>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
        }
    }
}
