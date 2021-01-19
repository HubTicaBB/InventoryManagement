using Microsoft.AspNetCore.Mvc;
using Pizzeria.Models;

namespace Pizzeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        public Menu Menu { get; set; }

        public MenuController()
        {
            Menu = Menu.GetMenu();
        }

        //// GET: api/menu
        [HttpGet]
        public IActionResult Get()
        {
            var products = Menu;
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }
    }
}
