using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ChristineCA3.Models
{
    public class OrderItem
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int OrderItemId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [AllowNull]
        [MaxLength(50)]
        public string ProductName { get; set; }
        public int Quantity {  get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; } = decimal.Zero;
    }
}
