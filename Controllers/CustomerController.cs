using ChristineCA3.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace ChristineCA3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class CustomerController : ControllerBase
    {
        private DatabaseContext _db = new DatabaseContext();

        [HttpGet("GetCustomerList")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<Customer>))]
        public IResult GetCustomerList()
        {
            IEnumerable<Customer> list = _db.CustomerSet.ToList();
            return Results.Ok(list);
        }

        [HttpGet("GetCustomerById")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Customer))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer ID not found")]
        public IResult GetCustomerById(int id)
        {
            var result = _db.CustomerSet.Find(id);
            if (result == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(result);
        }

        [HttpPost("CreateCustomer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Customer Created")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input value incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized client")]
        public IResult Create([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return Results.BadRequest();
            }
            _db.CustomerSet.Add(customer);
            _db.SaveChanges();
            return Results.Ok();
        }

        [HttpPut("UpdateCustomer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Customer Updated")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Input value incorrect")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer ID not found")]
        public IResult Update(int id, [FromBody] Customer customer)
        {
            var result = _db.CustomerSet.Find(id);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                //potentially add here value checks that could result in a BadRequest
                result.FirstName = customer.FirstName;
                result.LastName = customer.LastName;
                result.DateOfBirth = customer.DateOfBirth;
                result.AnnualSpend = customer.AnnualSpend;

                _db.CustomerSet.Update(result);
                _db.SaveChanges();
            }
            return Results.Ok();
        }

        [HttpDelete("DeleteCustomer")]
        [SwaggerResponse(StatusCodes.Status200OK, "Customer Deleted")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized client")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Customer ID not found")]
        public IResult Delete(int id)
        {
            var result = _db.CustomerSet.Find(id);
            if (result == null)
            {
                return Results.NotFound();
            }
            else
            {
                _db.CustomerSet.Remove(result);
                _db.SaveChanges();
            }
            return Results.Ok();
        }
    }
}
