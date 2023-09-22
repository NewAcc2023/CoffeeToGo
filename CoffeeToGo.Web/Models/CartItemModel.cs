using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoffeeToGo.Web.Models
{
	public class CartItemModel
	{
		public int Id { get; set; }
		public string Name { get; set; } 
		public string ImageURL { get; set; } 
		public decimal Price { get; set; }
		public decimal Quantity { get; set; }
		public decimal TotalPrice { get; set; }
	}
}
