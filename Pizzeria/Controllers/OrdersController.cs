﻿using Microsoft.AspNetCore.Mvc;
using Pizzeria.Data;
using Pizzeria.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Pizzeria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public Orders Orders { get; set; }
        public Menu Menu { get; set; }

        public OrdersController()
        {
            Orders = Orders.GetOrders();
            Menu = Menu.GetMenu();
        }

        [HttpGet]
        public IActionResult GetOrders()
        {
            if (Orders == null)
            {
                return NotFound();
            }
            return Ok(Orders.Queue);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = GetOrderBy(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("status={status}")]
        public IActionResult GetOrdersByStatus(string status)
        {
            if (Enum.TryParse(status, true, out Status orderStatus))
            {
                var orders = Orders.Queue.Where(o => o.Status == orderStatus).ToList();
                return Ok(new { count = orders.Count, orders });
            }
            return BadRequest($"Invalid status: {status}");
        }

        [HttpPost]
        public IActionResult CreateOrder([FromBody] string productName)
        {
            return CreateOrUpdateOrder(productName);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, [FromQuery] string action, [FromBody] string productName)
        {
            switch (action)
            {
                case "add":
                    return CreateOrUpdateOrder(productName, id);
                case "delete":
                    return DeleteProductFromOrder(productName, id);
                default:
                    return BadRequest();
            }
        }

        [HttpPut("{id}/submit")]
        public IActionResult SubmitOrder(int id)
        {
            var order = GetOrderBy(id);
            if (order is null)
            {
                return NotFound();
            }

            if (IsOrderEmpty(order))
            {
                return BadRequest("It is not posible to submit an empty order.");
            }
            if (order.Status == Status.InProgress)
            {
                order.Status = Status.Submitted;
                var calculator = new OrderSumCalculator();
                order.Total = calculator.CalculateOrderSum(order);
                return Ok(order);
            }
            return BadRequest($"It is not possible to change the status of a {order.Status.ToString().ToLower()} order to submitted.");
        }

        // TODO: REFACTOR!!!
        [HttpPut("{id}/status={status}")]
        public IActionResult ChangeOrderStatus(int id, string status)
        {
            var order = GetOrderBy(id);
            if (order is null)
            {
                return NotFound();
            }

            if (order.Status == Status.Submitted)
            {
                if (Enum.TryParse(status, true, out Status newStatus))
                {                    
                    if (newStatus == Status.Delivered)
                    {
                        var responseStatus = UpdateInventory(order).StatusCode;
                        if (responseStatus == HttpStatusCode.OK )
                        {
                            order.Status = Status.Delivered;
                            return Ok(order);
                        }
                    };
                    return BadRequest("Something went wrong, the order has not been delivered!");
                }
                else
                {
                    return BadRequest($"Invalid status parameter: {status}");
                }
            }
            return BadRequest($"It's not possible to change status of a {order.Status} order to {status}.");
        }

        // TODO: REFACTOR!!!
        private static IRestResponse UpdateInventory(Order order)
        {
            var client = new RestClient("http://inventoryapi:80/api/inventory/consume") { Timeout = -1 };
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");

            List<string> consumedIngredients = new List<string>();

            var ingredientsPerPizza = order.Pizzas.Select(pizza => pizza.Ingredients).ToList();
            ingredientsPerPizza.ToList().ForEach(pi => pi.ToList().ForEach(i => consumedIngredients.Add(i.Name)));
            var extraIngredients = order.Ingredients.Select(i => i.Name).ToList();
            consumedIngredients.AddRange(extraIngredients);

            var consumedWithQuantity = consumedIngredients.GroupBy(name => name).Select(group => new { Name = group.Key, Quantity = group.Count() });

            var body = new { OrderDetails = consumedWithQuantity };

            request.AddParameter("application/json", JsonSerializer.Serialize(body), ParameterType.RequestBody);
            var response = client.Execute(request);
            return response;
        }

        private Order GetOrderBy(int? id)
        {
            return Orders.Queue.Find(o => o.Id == id);
        }

        public IActionResult CreateOrUpdateOrder(string productName, int? id = null)
        {
            var order = id is null ? new Order() : GetOrderBy(id);
            if (order == null)
            {
                return NotFound();
            }
            if (order.Status != Status.InProgress)
            {
                return BadRequest($"It is not possible to change a {order.Status.ToString().ToLower()} order.");
            }

            var pizza = Menu.Pizzas.Where(p => p.Name.ToLower() == productName.ToLower()).FirstOrDefault();
            var soda = Menu.Sodas.Where(s => s.Name.ToLower() == productName.ToLower()).FirstOrDefault();
            var ingredient = Menu.Ingredients.Where(i => i.Name.ToLower() == productName.ToLower()).FirstOrDefault();

            if (pizza == null && soda == null && ingredient == null)
            {
                return BadRequest("The product you are trying to order is not on the menu");
            }
            else
            {
                if (pizza != null)
                {
                    order.Pizzas.Add(pizza);
                }
                if (soda != null)
                {
                    order.Sodas.Add(soda);
                }
                if (ingredient != null)
                {
                    if (order.Pizzas.Count == 0)
                    {
                        return BadRequest("You can not add the ingredient because you do not have a pizza on your order");
                    }
                    order.Ingredients.Add(ingredient);
                }

                Orders.Queue.Add(order);
                return Ok(order);
            }
        }

        public IActionResult DeleteProductFromOrder(string productName, int? id = null)
        {
            var order = GetOrderBy(id);
            if (order == null)
            {
                return NotFound("No order found");
            }

            if (order.Status != Status.InProgress)
            {
                return BadRequest("Your order is not in progress so you can not delete products from it.");
            }

            var pizzaToRemove = order.Pizzas.FirstOrDefault(p => p.Name.ToLower() == productName.ToLower());
            if (pizzaToRemove != null)
            {
                order.Pizzas.Remove(pizzaToRemove);
                if (order.Pizzas.Count == 0)
                {
                    order.Ingredients.Clear();
                }
            }

            var sodaToRemove = order.Sodas.FirstOrDefault(s => s.Name.ToLower() == productName.ToLower());
            if (sodaToRemove != null)
            {
                order.Sodas.Remove(sodaToRemove);
            }

            var ingredientToToRemove = order.Ingredients.FirstOrDefault(i => i.Name.ToLower() == productName.ToLower());
            if (ingredientToToRemove != null)
            {
                order.Ingredients.Remove(ingredientToToRemove);
            }

            if (pizzaToRemove is null && sodaToRemove is null && ingredientToToRemove is null)
            {
                return BadRequest("The product cannot be found on your order");
            }

            return Ok(order);
        }

        private bool IsOrderEmpty(Order order)
        {
            return order.Pizzas.Count == 0 &&
                   order.Sodas.Count == 0 &&
                   order.Ingredients.Count == 0;
        }
    }
}
