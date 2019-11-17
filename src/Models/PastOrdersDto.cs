using System.Collections.Generic;

namespace DaxkoOrderAPI.Models
{
    public class PastOrdersDto
    {
        public PastOrdersDto()
        {
            pastOrderItems = new List<PastOrderItemDto>();
        }
        public long ID { get; set; }
        public decimal OrderTotal { get; set; }
        public int TotalNumOfItems { get; set; }
        public List<PastOrderItemDto> pastOrderItems { get; set; }
    }

    public class PastOrderItemDto
    {
        public string Name { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalItemPrice { get; set; }
        public int Quantity { get; set; }
    }
}
