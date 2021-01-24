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
            var actionResult = _unitOfWork.Ingredient.PlaceManualOrder(ingredient);
            return actionResult;
        }           

        [HttpPut("bulk")]
        public IActionResult IncreaseMultipleQuantity()
        {
            return _unitOfWork.Ingredient.PlaceBulkOrder();
        }

        [HttpPut("consume")]
        public IActionResult DecreaseQuantity(OrderDto order)
        {
            return _unitOfWork.Ingredient.ConsumeIngredients(order.OrderItems);
        }
    }
}
                                  