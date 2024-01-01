using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;


namespace ChristineCA3.Models
{
    public class Customer
    {
        [Key]
        [SwaggerSchema(ReadOnly = true)]
        public int CustomerId { get; set; }
        [MaxLength(50)]
        [AllowNull]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [AllowNull]
        public string LastName { get; set; }
        [AllowNull]
        public DateOnly DateOfBirth { get; set; }
        public int AnnualSpend { get; set; } = 0;

    }
}
