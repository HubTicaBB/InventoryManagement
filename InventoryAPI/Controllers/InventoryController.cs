using InventoryAPI.Models;
using InventoryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Update(IngredientDto ingredient)
        {
            _unitOfWork.Ingredient.Update(ingredient);
            return Ok(ingredient);
        }
    }
}
                                  