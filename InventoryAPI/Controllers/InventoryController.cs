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
            var ingredients = _ingredientRepository.GetAll();
            return Ok(ingredients);
        }

        [HttpPut]
        public IActionResult IncreaseQuantity(IngredientDto ingredient)
        {
            var actionResult = _ingredientRepository.PlaceManualOrder(ingredient);
            return actionResult;
        }           

        [HttpPut("bulk")]
        public IActionResult IncreaseMultipleQuantity()
        {
            var actionResult = _ingredientRepository.PlaceBulkOrder();
            return actionResult;
        }

        [HttpPut("consume")]
        public IActionResult DecreaseQuantity(OrderDto order)
        {
            var actionResult = _ingredientRepository.ConsumeIngredients(order.OrderItems);
            return actionResult;
        }
    }
}
                                  