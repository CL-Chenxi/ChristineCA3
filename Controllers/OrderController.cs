using ChristineCA3.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChristineCA3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class OrderController : ControllerBase
    {
        private DatabaseContext _db = new DatabaseContext();

        [HttpGet("GetAllOrders")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
        public IResult GetAllOrders()
        {
            IEnumerable<Order> list = _db.OrderSet.ToList();
            return Results.Ok(list);
        }

        [HttpGet("GetOrderById")]
        [SwaggerResponse(StatusCodes.Status200OK,Type = typeof(Order))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public IResult GetOrderById(int orderId) 
        {
            var order = _db.OrderSet.Find(orderId);
            if(order == null) 
                return Results.NotFound();
            else
                return Results.Ok(order);
        }

        [HttpGet("GetAllOrdersForCustomer")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public IResult GetAllOrdersForCustomer(int customerId) 
        {
            var customer = _db.CustomerSet.Find(customerId);
            if(customer == null)
                return Results.NotFound();

            IEnumerable<Order> orderList = _db.OrderSet.Where(c => c.CustomerId == customerId).ToList();
            return Results.Ok(orderList);
        }

        [HttpPost("CreateOrder")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order Created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input value incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized client")]
        public IResult CreateOrder([FromBody] Order order)
        {
            if (order == null)
            {
                return Results.BadRequest();
            }
            _db.OrderSet.Add(order);
            _db.SaveChanges();
            return Results.Ok();
        }

        [HttpPut("UpdateOrder")]
        [SwaggerResponse(StatusCodes.Status200OK, "Customer Updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input value incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer ID not found")]
        public IResult UpdateOrder(int orderId, [FromBody] Order order)
        {
            var result = _db.OrderSet.Find(orderId);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                //value checks that could result in a BadRequest
                if(_db.CustomerSet.Find(order.CustomerId) == null) 
                {
                    return Results.BadRequest("Customer " + order.CustomerId + " not found");
                }

                result.OrderDate = order.OrderDate;
                result.CustomerId = order.CustomerId;

                _db.OrderSet.Update(result);
                _db.SaveChanges();
            }
            return Results.Ok();
        }


        [HttpDelete("DeleteOrder")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order Deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Order ID not found")]
        public IResult DeleteOrder(int orderId)
        {
            var result = _db.OrderSet.Find(orderId);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                IEnumerable<OrderItem> itemList = _db.OrderItemSet.Where(item => item.OrderId == orderId).ToList();
                foreach (var item in itemList) 
                {
                    _db.OrderItemSet.Remove(item);
                }

                _db.OrderSet.Remove(result);
                _db.SaveChanges();
            }
            return Results.Ok();
        }
    }
}
