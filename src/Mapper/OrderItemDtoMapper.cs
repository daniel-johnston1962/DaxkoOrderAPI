using AutoMapper;
using DaxkoOrderAPI.Data.Orders;
using DaxkoOrderAPI.Models;

namespace DaxkoOrderAPI.Mapper
{
    public class OrderItemDtoMapper : Profile 
    {
        public OrderItemDtoMapper()
        {
            CreateMap<Item, OrderItemDto>()
                .ForMember(d => d.ItemID, o => o.MapFrom(s => s.ID))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.Price))
                ;
        }
    }
}
