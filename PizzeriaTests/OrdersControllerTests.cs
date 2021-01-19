using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pizzeria_API.Controllers;
using Pizzeria_API.Data;
using Pizzeria_API.Data.Factory;
using Pizzeria_API.Models;
using System.Collections.Generic;

namespace PizzeriaTests
{
    [TestClass]
    public class OrdersControllerTests
    {
        public Orders Orders { get; set; }

        [TestMethod]
        public void GetOrders_OrdersIsNull_ReturnsNotFoundResult()
        {
            var controller = new OrdersController();
            Orders orders = null;
            controller.Orders = orders;

            var actual = controller.GetOrders();

            Assert.IsInstanceOfType(actual, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetOrders_OrdersIsInstantiated_ReturnsOk()
        {
            var controller = new OrdersController();

            var actual = controller.GetOrders();

            Assert.IsInstanceOfType(actual, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetOrders_WhenCalled_ReturnsAllOrdersFromTheQueue()
        {
            var controller = new OrdersController();
            var orders = Orders.GetOrders();

            var expectedOrders = orders.Queue;
            var okResult = controller.GetOrders() as OkObjectResult;
            var actualOrders = okResult.Value as List<Order>;

            CollectionAssert.AreEquivalent(expectedOrders, actualOrders);
        }

        [TestMethod]
        public void GetOrder_IdExists_Returns200()
        {
            var controller = new OrdersController();

            var objectResult = controller.GetOrder(1) as OkObjectResult;
            var actualStatusCode = objectResult.StatusCode;

            Assert.AreEqual(200, actualStatusCode);
        }

        [TestMethod]
        public void GetOrder_IdDoesntExist_Returns404()
        {
            var controller = new OrdersController();

            var objectResult = controller.GetOrder(0) as NotFoundResult;
            var actualStatusCode = objectResult.StatusCode;

            Assert.AreEqual(404, actualStatusCode);
        }

        [TestMethod]
        public void GetOrder_WhenCalled_ReturnsOrderWithSameId()
        {
            var controller = new OrdersController();

            var expectedOrder = new Order() { Id = 1 };
            var okResult = controller.GetOrder(1) as OkObjectResult;
            var actualOrder = okResult.Value as Order;

            Assert.AreEqual(expectedOrder.Id, actualOrder.Id);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetProductNames), DynamicDataSourceType.Method)]
        public void CreateOrder_ProductNames_ReturnsObjectResult(string productName, ObjectResult expectedObjectResult)
        {
            var controller = new OrdersController();

            var actual = controller.CreateOrder(productName);
            var expectedResult = expectedObjectResult.GetType();

            Assert.IsInstanceOfType(actual, expectedResult);
        }

        [DataTestMethod]
        [DynamicData(nameof(GetStatuses), DynamicDataSourceType.Method)]
        public void GetOrdersByStatus_WhenCalled_ReturnsObjectResult(string status, ObjectResult expectedObjectResult)
        {
            var controller = new OrdersController();

            var actualResult = controller.GetOrdersByStatus(status);
            var expectedResult = expectedObjectResult.GetType();

            Assert.IsInstanceOfType(actualResult, expectedResult);
        }

        public static IEnumerable<object[]> GetStatuses()
        {
            yield return new object[]
            {
                "inprogress",
                new OkObjectResult(new { count = 1, orders = new List<Order>() })
            };
            yield return new object[]
            {
                "submitted",
                new OkObjectResult(new { count = 1, orders = new List<Order>() })
            };
            yield return new object[]
            {
                "delivered",
                new OkObjectResult(new { count = 1, orders = new List<Order>() })
            };
            yield return new object[]
            {
                "cancelled",
                new OkObjectResult(new { count = 1, orders = new List<Order>() })
            };
            yield return new object[]
            {
                "invalid-status",
                new BadRequestObjectResult("Invalid status: invalid-status")
            };
        }

        public static IEnumerable<object[]> GetProductNames()
        {
            yield return new object[]
            {
                "Shrimps",
                new BadRequestObjectResult("You can not add the ingredient because you do not have a pizza on your order")
            };
            yield return new object[]
            {
                "Fanta",
                new OkObjectResult(new Order() { Pizzas = new List<Product>() { new Margherita() }})
            };
            yield return new object[]
            {
                "Margherita",
                new OkObjectResult(new Order() { Pizzas = new List<Product>() { new Margherita() }})
            };
            yield return new object[]
            {
                "Shrimps",
                new BadRequestObjectResult("You can not add the ingredient because you do not have a pizza on your order")
            };
            yield return new object[]
            {
                "Non Existing Product",
                new BadRequestObjectResult("The product you are trying to order is not on the menu")
            };
        }
        [DataTestMethod]
        [DynamicData(nameof(GetProductNames), DynamicDataSourceType.Method)]
        public void AddProductToOrder_ProductNames_ReturnsObjectResult(string productName, ObjectResult expectedObjectResult)
        {
            var controller = new OrdersController();

            var actual = controller.CreateOrUpdateOrder(productName);
            var expectedResult = expectedObjectResult.GetType();

            Assert.IsInstanceOfType(actual, expectedResult);
        }

        [TestMethod]
        public void AddProductToOrder_IdDoesNotExist_ReturnsNotFound()
        {
            var controller = new OrdersController();
            var orderIdDoesNotExist = 0;
            var actualActionResult = controller.CreateOrUpdateOrder("Margherita", orderIdDoesNotExist);

            Assert.IsInstanceOfType(actualActionResult, typeof(NotFoundResult));
        }


        public static IEnumerable<object[]> GetProductNamesAndOrderId()
        {
            yield return new object[]
            {
                "Shrimps",
                new OkObjectResult(new Order()
                {
                    Pizzas= new List<Product>() { new Margherita()},
                    Sodas = new List<Soda>() { new Fanta() }
                })
            };
            yield return new object[]
            {
                "Fanta",
                new OkObjectResult(new Order()
                {
                    Pizzas= new List<Product>() {new Margherita()},
                    Ingredients = new List<Ingredient>() {new Shrimps()}
                })
            };
            yield return new object[]
            {
                "Margherita",
                 new OkObjectResult(new Order()
                 {
                    Sodas = new List<Soda>() { new Fanta() }
                 })
            };
            yield return new object[]
            {
                "Non Existing Product",
                new BadRequestObjectResult("The product cannot be found on your order")
            };
        }


        [DataTestMethod]
        [DynamicData(nameof(GetProductNamesAndOrderId), DynamicDataSourceType.Method)]
        public void DeleteProductFromOrder_StatusInProgress_ReturnObjectResult(string productName, ObjectResult expectedObjectResult)
        {
            var mockOrder = new Order()
            {
                Pizzas = new List<Product>() { new Margherita() },
                Sodas = new List<Soda>() { new Fanta() },
                Ingredients = new List<Ingredient>() { new Shrimps() },
                Status = Status.InProgress
            };
            Orders = Orders.GetOrders();
            Orders.Queue.Add(mockOrder);

            var controller = new OrdersController();
            var actual = controller.DeleteProductFromOrder(productName, mockOrder.Id);

            Assert.IsInstanceOfType(actual, expectedObjectResult.GetType());
        }

        public static IEnumerable<object[]> GetOrdersDifferentStatus()
        {
            yield return new object[]
            {
                "Margherita",
                new Order()
                {
                    Pizzas= new List<Product>() { new Margherita()},
                    Sodas = new List<Soda>() { new Fanta() },
                    Status= Status.InProgress
                },

                new OkObjectResult(new Order()
                {
                    Sodas = new List<Soda>() { new Fanta() }
                })
            };
            yield return new object[]
            {
                "Margherita",
                new Order()
                {
                    Pizzas= new List<Product>() { new Margherita()},
                    Sodas = new List<Soda>() { new Fanta() },
                    Status= Status.Submitted
                },
                new BadRequestObjectResult("Your order is not in progress so you can not delete products from it.")
            };
            yield return new object[]
            {
                "Margherita",
                 new Order()
                 {
                    Pizzas= new List<Product>() { new Margherita()},
                    Sodas = new List<Soda>() { new Fanta() },
                    Status= Status.Delivered
                 },
                 new BadRequestObjectResult("Your order is not in progress so you can not delete products from it.")
            };
            yield return new object[]
            {
                "Margherita",
                new Order()
                {
                    Pizzas= new List<Product>() { new Margherita()},
                    Sodas = new List<Soda>() { new Fanta() },
                    Status= Status.Cancelled
                },
                new BadRequestObjectResult("Your order is not in progress so you can not delete products from it.")
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetOrdersDifferentStatus), DynamicDataSourceType.Method)]
        public void DeleteProductFromOrder_DifferentStatus_ReturnObjectResult(string productName, Order order, ObjectResult expectedObjectResult)
        {
            Orders = Orders.GetOrders();
            Orders.Queue.Add(order);

            var controller = new OrdersController();
            var actual = controller.DeleteProductFromOrder(productName, order.Id);

            Assert.IsInstanceOfType(actual, expectedObjectResult.GetType());
        }

        [DataTestMethod]
        [DynamicData(nameof(GetUpdateActions), DynamicDataSourceType.Method)]
        public void UpdateOrder_WhenCalled_ReturnsObjectResult(string action, IActionResult expectedObjectResult)
        {
            var controller = new OrdersController();

            var actualResult = controller.UpdateOrder(1, action, "product name");
            var expectedResult = expectedObjectResult.GetType();

            Assert.IsInstanceOfType(actualResult, expectedResult);
        }

        public static IEnumerable<object[]> GetUpdateActions()
        {
            yield return new object[]
            {
                "add",
                new ObjectResult(new object())
            };
            yield return new object[]
            {
                "delete",
                new ObjectResult(new object())
            };
            yield return new object[]
            {
                "invalid action",
                new BadRequestResult()
            };
        }

        [DataTestMethod]
        [DynamicData(nameof(GetOrders), DynamicDataSourceType.Method)]
        public void SubmitOrder_WhenCalled_ReturnsObjectResult(Order order, IActionResult expectedObjectResult)
        {
            var controller = new OrdersController();
            var orders = Orders.GetOrders();
            orders.Queue.Clear();
            orders.Queue.Add(order);
            controller.Orders = orders;

            var actual = controller.SubmitOrder(1);
            var expected = expectedObjectResult.GetType();

            Assert.IsInstanceOfType(actual, expected);
        }

        public static IEnumerable<object[]> GetOrders()
        {
            yield return new object[]
            {
                new Order() { Id = 0 },
                new NotFoundResult()
            };
            yield return new object[]
            {
                new Order() { Id = 1 },
                new BadRequestObjectResult("")
            };
            yield return new object[]
            {
                new Order() { Id = 1, Pizzas = new List<Product>() { new Margherita() } },
                new OkObjectResult("")
            };
            yield return new object[]
            {
                new Order() { Id = 1, Status = Status.Delivered, Pizzas = new List<Product>() { new Margherita() } },
                new BadRequestObjectResult("")
            };            
        }

        [DataTestMethod]
        [DynamicData(nameof(GetOrdersForStatusChange), DynamicDataSourceType.Method)]
        public void ChangeOrderStatus_WhenCalled_ReturnsObjectResult(Order order, string status, IActionResult expectedObjectResult)
        {
            var controller = new OrdersController();
            var orders = Orders.GetOrders();
            orders.Queue.Clear();
            orders.Queue.Add(order);
            controller.Orders = orders;

            var actual = controller.ChangeOrderStatus(1, status);
            var expected = expectedObjectResult.GetType();

            Assert.IsInstanceOfType(actual, expected);
        }

        public static IEnumerable<object[]> GetOrdersForStatusChange()
        {
            yield return new object[]
            {
                new Order() { Id = 0 },
                "delivered",
                new NotFoundResult()
            };
            yield return new object[]
            {
                new Order() { Id = 1, Status = Status.Submitted },
                "cancelled",
                new OkObjectResult("")
            };
            yield return new object[]
            {
                new Order() { Id = 1, Status = Status.InProgress },
                "delivered",
                new BadRequestObjectResult("")
            };
            yield return new object[]
            {
                new Order() { Id = 1, Status = Status.Submitted },
                "invalid status",
                new BadRequestObjectResult("")
            };
        }
    }
}
