using ChristineCA3.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChristineCA3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class OrderItemController : ControllerBase
    {
        private DatabaseContext _db = new DatabaseContext();

        [HttpGet("GetItemById")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(OrderItem))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public IResult GetItemById(int itemId)
        {
            var item = _db.OrderItemSet.Find(itemId);
            if (item == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(item);
        }


        [HttpGet("GetAllItemsForOrderId")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderItem>))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public IResult GetAllItemsForOrderId(int orderId)
        {
            var order = _db.OrderSet.Find(orderId);
            if (order == null)
            { 
                return Results.NotFound(); 
            }

            IEnumerable<OrderItem> itemList = _db.OrderItemSet.Where(item => item.OrderId == orderId).ToList();
            return Results.Ok(itemList);
        }

        [HttpPost("CreateOrderItem")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order Item Created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input value incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized client")]
        public IResult CreateOrderItem([FromBody] OrderItem item)
        {
            if (item == null)
            {
                return Results.BadRequest();
            }
            _db.OrderItemSet.Add(item);
            _db.SaveChanges();
            return Results.Ok();
        }

        [HttpPut("UpdateOrderItem")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order Item Updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input value incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Item ID not found")]
        public IResult UpdateOrderItem(int itemId, [FromBody] OrderItem orderItem)
        {
            var result = _db.OrderItemSet.Find(itemId);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                //value checks that could result in a BadRequest
                if(_db.OrderSet.Find(orderItem.OrderId) == null) 
                {
                    return Results.BadRequest("Order Id "+orderItem.OrderId+" not found");
                }
                if(result.Quantity < 0)
                {
                    return Results.BadRequest("Quantity can't be negative");
                }
                if (result.UnitPrice < 0)
                {
                    return Results.BadRequest("Unit price can't be negative");
                }
                result.OrderId = orderItem.OrderId;
                result.ProductName = orderItem.ProductName;
                result.Quantity = orderItem.Quantity;
                result.UnitPrice = orderItem.UnitPrice;

                _db.OrderItemSet.Update(result);
                _db.SaveChanges();
            }
            return Results.Ok();
        }

        [HttpDelete("DeleteOrderItem")]
        [SwaggerResponse(StatusCodes.Status200OK, "Order Item Deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Item ID not found")]
        public IResult DeleteOrderItem(int itemId)
        {
            var result = _db.OrderItemSet.Find(itemId);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                _db.OrderItemSet.Remove(result);
                _db.SaveChanges();
            }
            return Results.Ok();
        }
    }
}
