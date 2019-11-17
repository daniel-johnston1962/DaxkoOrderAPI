using AutoMapper;
using DaxkoOrderAPI.Data.Orders;
using DaxkoOrderAPI.Models;
using DaxkoOrderAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaxkoOrderAPI.Features.Handlers
{
    public class OrderItemHandler : IOrderItemHandler
    {
        private readonly IRepository<Item> _itemRepo;
        private readonly IMapper _mapper;

        public OrderItemHandler(IRepository<Item> itemRepo, IMapper mapper)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
        }

        public async Task<List<OrderItemDto>> HandlerAsync()
        {
            var items = await _itemRepo.EntitySet
                .AsNoTracking()
                .Where(x => x.IsActive == true)
                .ToListAsync();

            var mappedItems = _mapper.Map<List<OrderItemDto>>(items);

            return mappedItems;
        }
    }

    public interface IOrderItemHandler
    {
        Task<List<OrderItemDto>> HandlerAsync();
    }
}
