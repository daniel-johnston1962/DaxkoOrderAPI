using DaxkoOrderAPI.Data.Orders;
using DaxkoOrderAPI.Models;
using DaxkoOrderAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaxkoOrderAPI.Features.Handlers
{
    public class PastOrderHandler : IPastOrderHandler
    {
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<OrderDetail> _orderDetailRepo;
        private readonly IRepository<ShippedOrder> _shippedOrderRepo;

        public PastOrderHandler(IRepository<Item> itemRepo, IRepository<OrderDetail> orderDetailRepo, IRepository<ShippedOrder> shippedOrderRepo)
        {
            _itemRepo = itemRepo;
            _orderDetailRepo = orderDetailRepo;
            _shippedOrderRepo = shippedOrderRepo;
        }

        public async Task<List<PastOrdersDto>> HandlerAsync()
        {
            var shippedOrders = new List<PastOrdersDto>();

            var shippedOrdersIDs = await _shippedOrderRepo.EntitySet
                .AsNoTracking()
                .Select(x =>  new {ID = x.ID, ShippedOrderID = x.ShippedOrderID })
                .ToListAsync();
     
            foreach(var soid in shippedOrdersIDs)
            {
                var lineItem = await (from odr in _orderDetailRepo.EntitySet.AsNoTracking()
                                        join ir in _itemRepo.EntitySet.AsNoTracking()
                                        on odr.ItemID equals ir.ID
                                        where odr.ShippedOrderID == soid.ShippedOrderID
                                        orderby ir.Description
                                        select new PastOrderItemDto()
                                        {
                                            ItemPrice = odr.Price,
                                            Name = ir.Description,
                                            Quantity = odr.Quantity,
                                            TotalItemPrice = odr.TotalPrice
                                        }).ToListAsync();

                shippedOrders.Add(new PastOrdersDto()
                {
                    ID = soid.ID,
                    OrderTotal = lineItem.Sum(x => x.TotalItemPrice),
                    TotalNumOfItems = lineItem.Sum(x => x.Quantity),
                    pastOrderItems = lineItem
                });
            }

            return shippedOrders;
        }
    }

    public interface IPastOrderHandler
    {
        Task<List<PastOrdersDto>> HandlerAsync();
    }
}
