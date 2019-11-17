using DaxkoOrderAPI.Data.Orders;
using Microsoft.EntityFrameworkCore;

namespace DaxkoOrderAPI.Data
{
    public class DaxkoDbContext : DbContext, IDaxkoDbContext
    {
        public DaxkoDbContext(DbContextOptions<DaxkoDbContext> options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShippedOrder> ShippedOrders { get; set; }
    }
	
	public interface IDaxkoDbContext
    {
        DbSet<Item> Items { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        DbSet<ShippedOrder> ShippedOrders { get; set; } 
    }
}
