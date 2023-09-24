using CoffeeToGo.EntityFramework.DbContexts;
using CoffeeToGo.EntityFramework.Entities;
using CoffeeToGo.Web.Models;

namespace CoffeeToGo.Web.Repositories
{
	public class OrderRepo
	{
		private readonly AppDbContext _context;
		public OrderRepo(AppDbContext context)
		{
			_context = context;
		}

		public async Task AddOrder(Order order, List<CartItemModel> orderItems, decimal totalOrderPrice)
		{
			order.TotalPrice = totalOrderPrice;
			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();
			int id = order.Id;

            foreach (var item in orderItems)
            {
				await _context.OrderItems.AddAsync(new OrderItem
				{
					CoffeeId = item.Id,
					OrderId = id,
					Quantity = item.Quantity,
					TotalPrice = item.Price * item.Quantity,
				});
            }
			await _context.SaveChangesAsync();
		}
	}
}
