using InventoryAPI.Controllers;
using InventoryAPI.Models;
using InventoryAPI.Models.DTO;
using InventoryAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    public class InventoryTests
    {
        private Mock<IIngredientRepository> _repositoryMock;
        private List<Ingredient> _ingredientsMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _repositoryMock = new Mock<IIngredientRepository>();
            _ingredientsMock = new List<Ingredient>
            {
                new Ingredient { Id = 1, Name = "Ingridient 1", UnitPrice = 10, QuantityOnStock = 100 },
                new Ingredient { Id = 2, Name = "Ingridient 2", UnitPrice = 15, QuantityOnStock = 100 },
                new Ingredient { Id = 3, Name = "Ingridient 3", UnitPrice = 20, QuantityOnStock = 0 },
            };
        }

        [TestMethod]
        public void GetAll_ReturnsOk()
        {
            var ingredientRepository = new Mock<IIngredientRepository>();
            var controller = new InventoryController(ingredientRepository.Object);

            var actualResult = controller.GetAll();

            Assert.IsInstanceOfType(actualResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetAll_ReturnsCorrectDataInsideActionResult()
        {            
            _repositoryMock.Setup(m => m.GetAll()).Returns(_ingredientsMock);
            var controller = new InventoryController(_repositoryMock.Object);

            var actualResult = controller.GetAll() as OkObjectResult;
            var actualData = actualResult.Value as List<Ingredient>;

            CollectionAssert.AreEquivalent(_ingredientsMock, actualData);
        }

        [TestMethod]
        public void IncreaseQuantity_InvalidIngredient_ReturnsNotFound()
        {
            var ingredient = new IngredientDto() { Id = 0, ReorderQuantity = 1 };
            _repositoryMock.Setup(m => m.PlaceManualOrder(ingredient)).Returns(new NotFoundResult());
            var controller = new InventoryController(_repositoryMock.Object);

            var actualResult = controller.IncreaseQuantity(ingredient);

            Assert.IsInstanceOfType(actualResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void IncreaseQuantity_ValidIngredient_ReturnsOk()
        {
            var ingredient = new IngredientDto() { Id = 1, ReorderQuantity = 1 };
            _repositoryMock.Setup(m => m.PlaceManualOrder(ingredient)).Returns(new OkResult());
            var controller = new InventoryController(_repositoryMock.Object);

            var actualResult = controller.IncreaseQuantity(ingredient);

            Assert.IsInstanceOfType(actualResult, typeof(OkResult));
        }

        [TestMethod]
        public void IncreaseMultipleQuantity_ReturnsOk()
        {
            _repositoryMock.Setup(m => m.PlaceBulkOrder()).Returns(new OkResult());
            var controller = new InventoryController(_repositoryMock.Object);

            var actualResult = controller.IncreaseMultipleQuantity();

            Assert.IsInstanceOfType(actualResult, typeof(OkResult));
        }

        [TestMethod]
        public void IncreaseMultipleQuantity_PlaceBulkOrder()
        {
            _repositoryMock.Setup(m => m.PlaceBulkOrder()).Returns(new OkResult());
            var controller = new InventoryController(_repositoryMock.Object);

            var actualResult = controller.IncreaseMultipleQuantity();

            _repositoryMock.Verify(m => m.PlaceBulkOrder(), Times.Once);
        }

        [TestMethod]
        public void DecreaseQuantity_AllIngredientsOnStock_ReturnsOk()
        {
            var order = new OrderDto()
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Name = "Ingridient 1", Quantity = 1 },
                    new OrderItem { Name = "Ingridient 2", Quantity = 1 }
                }
            };
            _repositoryMock.Setup(m => m.ConsumeIngredients(order.OrderItems)).Returns(new OkResult());
            var controller = new InventoryController(_repositoryMock.Object);

            var actualResult = controller.DecreaseQuantity(order);

            Assert.IsInstanceOfType(actualResult, typeof(OkResult));
        }

        [TestMethod]
        public void DecreaseQuantity_NotAllIngredientsOnStock_ReturnsBadRequest()
        {
            var order = new OrderDto()
            {
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Name = "Ingridient 1", Quantity = 1 },
                    new OrderItem { Name = "Ingridient 2", Quantity = 1 },
                    new OrderItem { Name = "Ingridient 3", Quantity = 1 }
                }
            };
            _repositoryMock.Setup(m => m.ConsumeIngredients(order.OrderItems)).Returns(new BadRequestResult());
            var controller = new InventoryController(_repositoryMock.Object);

            var actualResult = controller.DecreaseQuantity(order);

            Assert.IsInstanceOfType(actualResult, typeof(BadRequestResult));
        }
    }
}
