using Azure.Core;
using CoffeeToGo.EntityFramework.DbContexts;
using CoffeeToGo.EntityFramework.Entities;
using CoffeeToGo.Web.Models;
using MediatR;

namespace CoffeeToGo.Web.CQRS.Commands.Orders
{
	public class AddOrderCommand : IRequest<Order> 
	{
		public Order Order { get; set; }
		public List<CartItemModel> OrderItems { get; set; }
		public decimal TotalOrderPrice { get; set; }
		public AddOrderCommand(Order order, List<CartItemModel> orderItems, decimal totalOrderPrice) {
			Order = order;
			OrderItems = orderItems;
			TotalOrderPrice = totalOrderPrice;
		}
	}

	public class AddOrderHandler : IRequestHandler<AddOrderCommand, Order>
	{
		private readonly AppDbContext _context;

        public AddOrderHandler(AppDbContext context)
        {
			_context = context;
        }

        public async Task<Order> Handle(AddOrderCommand request, CancellationToken cancellationToken)
		{
			request.Order.TotalPrice = request.TotalOrderPrice;
			await _context.Orders.AddAsync(request.Order);
			await _context.SaveChangesAsync();
			int id = request.Order.Id;

			foreach (var item in request.OrderItems)
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

			return request.Order;
		}
	}
}
