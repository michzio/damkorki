using System;
using Microsoft.AspNetCore.Mvc;

namespace DamkorkiWebApi.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
