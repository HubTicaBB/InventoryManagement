using InventoryAPI.Models;
using InventoryAPI.Models.DTO;
using InventoryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IIngredientRepository _ingredientRepository;

        public InventoryController(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_ingredientRepository.GetAll());
        }

        [HttpPut]
        public IActionResult IncreaseQuantity(IngredientDto ingredient)
        {
            return _ingredientRepository.PlaceManualOrder(ingredient);
        }           

        [HttpPut("bulk")]
        public IActionResult IncreaseMultipleQuantity()
        {
            return _ingredientRepository.PlaceBulkOrder();
        }

        [HttpPut("consume")]
        public IActionResult DecreaseQuantity(OrderDto order)
        {
            return _ingredientRepository.ConsumeIngredients(order.OrderItems);
        }
    }
}
                                  