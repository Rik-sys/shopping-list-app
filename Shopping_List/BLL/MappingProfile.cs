using AutoMapper;
using DBEntities.Models;
using DTO;

namespace BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category mappings
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<CategoryDto, Category>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id));

            // Shopping Session mappings  
            CreateMap<ShoppingSession, ShoppingCartSummaryDto>()
                .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.ShoppingSessionId));

            // Shopping Cart Item mappings
            CreateMap<ShoppingCartItem, ShoppingCartItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ShoppingCartItemId))
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore()); // Set manually

            CreateMap<ShoppingCartItemDto, ShoppingCartItem>()
                .ForMember(dest => dest.ShoppingCartItemId, opt => opt.MapFrom(src => src.Id));

            CreateMap<AddItemRequestDto, ShoppingCartItem>()
                .ForMember(dest => dest.ShoppingCartItemId, opt => opt.Ignore())
                .ForMember(dest => dest.SessionId, opt => opt.Ignore())
                .ForMember(dest => dest.AddedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CheckedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Session, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.IsChecked, opt => opt.MapFrom(src => false));

            // Completed Order mappings
            CreateMap<CompletedOrder, CompletedOrderDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CompletedOrderId))
                .ForMember(dest => dest.Items, opt => opt.Ignore()); // Set manually

            CreateMap<CompletedOrderDto, CompletedOrder>()
                .ForMember(dest => dest.CompletedOrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.Session, opt => opt.Ignore());

            CreateMap<CompletedOrderItem, CompletedOrderItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CompletedOrderItemId));

            CreateMap<CompletedOrderItemDto, CompletedOrderItem>()
                .ForMember(dest => dest.CompletedOrderItemId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<ShoppingCartItem, CompletedOrderItem>()
                .ForMember(dest => dest.CompletedOrderItemId, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryName, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());
            CreateMap<ShoppingSession, ShoppingSessionDto>()
    .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.ShoppingSessionId));

            CreateMap<ShoppingSessionDto, ShoppingSession>()
                .ForMember(dest => dest.ShoppingSessionId, opt => opt.MapFrom(src => src.SessionId))
                .ForMember(dest => dest.CartItems, opt => opt.Ignore())
                .ForMember(dest => dest.CompletedOrders, opt => opt.Ignore());
        }
    }
}
