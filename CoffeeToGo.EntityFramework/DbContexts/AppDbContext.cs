using CoffeeToGo.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeToGo.EntityFramework.DbContexts
{
	public class AppDbContext : DbContext
	{
		private readonly DbContextOptions _options;

		public AppDbContext(DbContextOptions options) : base(options)
		{
			_options = options;
		}

		public DbSet<Coffee> CoffeeProducts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrderItem>().HasKey(ts => new { ts.OrderId, ts.CoffeeId });
			base.OnModelCreating(modelBuilder);
		}
	}
}
