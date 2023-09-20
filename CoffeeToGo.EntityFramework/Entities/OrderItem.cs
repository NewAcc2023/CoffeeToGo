using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeToGo.EntityFramework.Entities
{
	public class OrderItem
	{
		[Required, ForeignKey(name: "Order")]
		public int OrderId { get; set; }

		[Required, ForeignKey(name: "Coffee")]
		public int CoffeeId { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required, Column(TypeName = "DECIMAL(9,2)")]
		public decimal TotalPrice { get; set; }
	}
}
