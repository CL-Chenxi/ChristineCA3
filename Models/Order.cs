using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChristineCA3.Models
{
    public class Order
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int OrderId { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public DateOnly OrderDate { get; set; }
    }
}
