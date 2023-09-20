using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeToGo.EntityFramework.Entities
{
	public class Coffee
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; } = null!;
		[Required]
		public string ImageURL { get; set; } = null!;
		[Required, Column(TypeName = "DECIMAL(9,2)")]
		public decimal Price { get; set; }
	}
}
