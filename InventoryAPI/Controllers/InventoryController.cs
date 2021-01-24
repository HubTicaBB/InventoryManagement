using InventoryAPI.Models;
using InventoryAPI.Models.DTO;
using InventoryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            foreach (var orderItem in order.OrderItems)
            {
                var ingredientId = _unitOfWork.Ingredient.GetId(orderItem.Name);
                _unitOfWork.Ingredient.ReduceStockUnits(
                    new IngredientDto { Id = ingredientId, ReorderQuantity = orderItem.Quantity }
                );
            }
            return Ok();
        }
    }
}
                                  