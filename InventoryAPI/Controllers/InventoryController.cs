using InventoryAPI.Models;
using InventoryAPI.Models.DTO;
using InventoryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace InventoryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var ingredients = _unitOfWork.Ingredient.GetAll();
            return Ok(ingredients);
        }

        [HttpPut]
        public IActionResult IncreaseQuantity(IngredientDto ingredient)
        {
            _unitOfWork.Ingredient.PlaceManualOrder(ingredient);
            return Ok(ingredient);
        }

        [HttpPut("bulk")]
        public IActionResult IncreaseMultipleQuantity()
        {
            var ingredients = _unitOfWork.Ingredient.GetAll();
            _unitOfWork.Ingredient.PlaceBulkOrder(ingredients);
            return Ok();
        }

        [HttpPut("consume")]
        public IActionResult DecreaseQuantity(OrderDto order)
        {
            var allOnStock = _unitOfWork.Ingredient.CheckIfAllOnStock(order.OrderItems);

            if (allOnStock)
            {
                order.OrderItems.ToList().ForEach(item =>
                {
                    var ingredientId = _unitOfWork.Ingredient.GetId(item.Name);
                    var ingredient = new IngredientDto { Id = ingredientId, ReorderQuantity = item.Quantity };

                    _unitOfWork.Ingredient.ReduceStockUnits(
                        new IngredientDto { Id = ingredientId, ReorderQuantity = item.Quantity }
                     );
                });
                return Ok();
            }
            else
            {
                return BadRequest("The order cannot be processed as some ingredients are out of stock.");
            }
        }
    }
}
                                  