using InventoryAPI.Models;
using InventoryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult UpdateSingle(IngredientDto ingredient)
        {
            _unitOfWork.Ingredient.PlaceManualOrder(ingredient);
            return Ok(ingredient);
        }

        [HttpPut("bulk")]
        public IActionResult UpdateMultiple()
        {
            var ingredients = _unitOfWork.Ingredient.GetAll();
            _unitOfWork.Ingredient.PlaceBulkOrder(ingredients);
            return Ok();
        }
    }
}
                                  