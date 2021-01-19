using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pizzeria_API.Controllers;
using Pizzeria_API.Models;

namespace PizzeriaTests
{
    [TestClass]
    public class MenuControllerTests
    {
        [TestMethod]
        public void Get_MenuIsNull_ReturnsNotFoundResult()
        {
            Menu menu = null;
            var controller = new MenuController();
            controller.Menu = menu;

            var actual = controller.Get();

            Assert.IsInstanceOfType(actual, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Get_MenuHasProducts_ReturnsOk()
        {
            var controller = new MenuController();

            var actual = controller.Get();

            Assert.IsInstanceOfType(actual, typeof(OkObjectResult));
        }
    }
}
