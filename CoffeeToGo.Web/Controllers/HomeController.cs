using CoffeeToGo.EntityFramework.DbContexts;
using CoffeeToGo.EntityFramework.Entities;
using CoffeeToGo.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace CoffeeToGo.Web.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;

		public HomeController(AppDbContext context)
		{
			_context = context;
		}
		[ResponseCache(NoStore = true)]
		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult PerformOrder()
		{

			return View("OrderSuccess");
		}

		[HttpPost]
		public IActionResult ConfirmOrder(List<CartItemModel> model)
		{
			decimal totalOrderPrice = 0;
            foreach (var item in model)
            {
				item.TotalPrice = item.Price * item.Quantity;
				totalOrderPrice += item.TotalPrice;
            }
			ViewData["TotalForOrder"] = totalOrderPrice;
            return View(model);
		}

		


		[ResponseCache(NoStore = true)]
		public IActionResult AddToCart(int id)
		{
			var cartItems = new List<Coffee>();

			if (Request.Cookies["Cart"] != null)
			{
				cartItems = JsonConvert.DeserializeObject<List<Coffee>>(Request.Cookies["Cart"]);

				// Check if the item with the specified ID already exists in the cart
				if (cartItems.Any(item => item.Id == id))
				{
					return RedirectToAction("Products", "Home");
				}
			}

			// Add the new item to the cart
			var newItem = _context.CoffeeProducts.FirstOrDefault(x => x.Id == id);
			if (newItem != null)
			{
				cartItems.Add(newItem);
			}

			// Update the "Cart" cookie with the modified cart items
			CookieOptions options = new CookieOptions();
			options.Expires = DateTime.Now.AddDays(7);
			Response.Cookies.Append("Cart", JsonConvert.SerializeObject(cartItems), options);

			return RedirectToAction("Products", "Home");
		}

		[ResponseCache(NoStore = true)]
		public IActionResult DeleteFromCart(int id)
		{
			if (Request.Cookies["Cart"] != null)
			{
				var cartItems = JsonConvert.DeserializeObject<List<Coffee>>(Request.Cookies["Cart"]);

				// Remove the item with the specified ID from the cart
				var itemToRemove = cartItems.FirstOrDefault(item => item.Id == id);
				if (itemToRemove != null)
				{
					cartItems.Remove(itemToRemove);

					// Update the "Cart" cookie with the modified cart items
					CookieOptions options = new CookieOptions();
					options.Expires = DateTime.Now.AddDays(7);
					Response.Cookies.Append("Cart", JsonConvert.SerializeObject(cartItems), options);
				}
			}

			return RedirectToAction("Cart", "Home");
		}


		[ResponseCache(NoStore = true)]
		public IActionResult Products()
		{
			IEnumerable<Coffee> model = _context.CoffeeProducts.Take(5);
			return View(model);
		}
		[ResponseCache(NoStore = true)]
		public IActionResult Cart()
		{
			if (Request.Cookies["Cart"] == null)
			{
				return View(new List<CartItemModel>());
			}
			else
			{
				IEnumerable<Coffee> selectedCoffees = JsonConvert.DeserializeObject<IEnumerable<Coffee>>(Request.Cookies["Cart"]);
				List<CartItemModel> cartItems = new List<CartItemModel>();
				foreach (var item in selectedCoffees)
				{
					cartItems.Add(new CartItemModel()
					{
						Id = item.Id,
						Name = item.Name,
						ImageURL = item.ImageURL,
						Price = item.Price,
						Quantity = 1,
						TotalPrice = 1 * item.Price
					});
					
				}
				return View(cartItems);
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}