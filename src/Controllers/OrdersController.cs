using DaxkoOrderAPI.Features.Commands;
using DaxkoOrderAPI.Features.Handlers;
using DaxkoOrderAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaxkoOrderAPI.Controllers
{
    [Route("v1/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUrlHelper _urlHelper;
        private readonly IItemIDHandler _itemIDHandler;
        private readonly IOrderItemHandler _orderItemHandler;
        private readonly IPastOrderHandler _pastOrderHandler;
        private readonly ISaveOrderHandler _saveOrderHandler;

        public OrdersController(IItemIDHandler itemIDHandler, IOrderItemHandler orderItemHandler, IPastOrderHandler pastOrderHandler, ISaveOrderHandler saveOrderHandler, IUrlHelper urlHelper)
        {
            _itemIDHandler = itemIDHandler;
            _orderItemHandler = orderItemHandler;
            _pastOrderHandler = pastOrderHandler;
            _saveOrderHandler = saveOrderHandler;
            _urlHelper = urlHelper;
        }

        /// <summary>
        /// Gets the list of Items currently for sale
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<OrderItemDto>), 200)]
        [ProducesResponseType(400)]
        [Route("Items")]
        [HttpGet]
        public async Task<IActionResult> Items()
        {
            var result = await _orderItemHandler.HandlerAsync();
            return Ok(result);
        }

        /// <summary>
        /// Gets the Item by ID
        /// </summary>
        /// <param name="ItemID"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(OrderItemDto), 200)]
        [ProducesResponseType(400)]
        [Route("Item/{ItemID}")]
        [HttpGet]
        public async Task<IActionResult> Item([FromRoute]long ItemID)
        {
            var result = await _itemIDHandler.HandlerAsync(ItemID);
            return Ok(result);
        }

        /// <summary>
        /// Gets the list of Past Orders
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(PastOrdersDto), 200)]
        [ProducesResponseType(400)]
        [Route("PastOrders")]
        [HttpGet]
        public async Task<IActionResult> PastOrders()
        {
            var result = await _pastOrderHandler.HandlerAsync();
            return Ok(result);
        }

        /// <summary>
        /// Enters a new Order of Items
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(long), 200)]
        [ProducesResponseType(400)]
        [Route("Order")]
        [HttpPost]
        public async Task<IActionResult> OrderedItems([FromBody] List<OrderCommand> command)
        {
            var result = await _saveOrderHandler.HandlerAsync(command);
            return Ok(result);
        }


    }
}
