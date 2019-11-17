using AutoMapper;
using DaxkoOrderAPI.Data.Orders;
using DaxkoOrderAPI.Models;
using DaxkoOrderAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DaxkoOrderAPI.Features.Handlers
{
    public class ItemIDHandler : IItemIDHandler
    {
        private readonly IRepository<Item> _itemRepo;
        private readonly IMapper _mapper;
        public ItemIDHandler(IRepository<Item> itemRepo, IMapper mapper)
        {
            _itemRepo = itemRepo;
            _mapper = mapper;
        }

        public async Task<OrderItemDto> HandlerAsync(long ID)
        {
            var item = await _itemRepo.EntitySet
                .AsNoTracking()
                .Where(x => x.ID == ID)
                .FirstOrDefaultAsync();

            var mappedItem = _mapper.Map<OrderItemDto>(item);

            return mappedItem;
        }
    }

    public interface IItemIDHandler
    {
        Task<OrderItemDto> HandlerAsync(long ID);
    }
}
