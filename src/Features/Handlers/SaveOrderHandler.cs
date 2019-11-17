using DaxkoOrderAPI.Data.Orders;
using DaxkoOrderAPI.Features.Commands;
using DaxkoOrderAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaxkoOrderAPI.Features.Handlers
{
    public class SaveOrderHandler : ISaveOrderHandler
    {
        private readonly IRepository<Item> _itemRepo;
        private readonly IRepository<OrderDetail> _orderDetailRepo;
        private readonly IRepository<ShippedOrder> _shippedOrderRepo;
        public SaveOrderHandler(IRepository<Item> itemRepo, IRepository<OrderDetail> orderDetailRepo, IRepository<ShippedOrder> shippedOrderRepo)
        {
            _itemRepo = itemRepo;
            _orderDetailRepo = orderDetailRepo;
            _shippedOrderRepo = shippedOrderRepo;
        }

        public async Task<long> HandlerAsync(List<OrderCommand> command)
        {
            var guid = Guid.NewGuid();

            // save each item in the order

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach(var c in command)
            {

                var item = await _itemRepo.EntitySet
                .AsNoTracking()
                .Where(x => x.ID == c.ItemID)
                .FirstOrDefaultAsync();

                var od = new OrderDetail()
                {
                    ItemID = c.ItemID,
                    Price = item.Price,
                    Quantity = c.Quantity,
                    ShippedOrderID = guid,
                    TotalPrice = item.Price * c.Quantity
                };
                orderDetails.Add(od);
            }

            var savedOrders = await _orderDetailRepo.InsertAsync(orderDetails);

            // save the shipping order

            var shippingOrder = new ShippedOrder()
            {
                ShippedOrderID = guid,
                CreatedDateTime = DateTime.Now
            };

            var savedShippingOrder = await _shippedOrderRepo.InsertAsync(shippingOrder);

            return savedShippingOrder.ID;

        }
    }

    public interface ISaveOrderHandler
    {
        Task<long> HandlerAsync(List<OrderCommand> command);
    }
}
