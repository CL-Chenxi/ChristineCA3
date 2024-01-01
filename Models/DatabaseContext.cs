using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChristineCA3.Models
{
    public class DatabaseContext : DbContext
    {
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {

            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");

            base.ConfigureConventions(builder);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var str = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=CA3_DB;User id=sa;Password=pwd;Trusted_Connection=True;Integrated Security=SSPI;";
            optionsBuilder.UseSqlServer(str);
        }

        public DbSet<Customer> CustomerSet { get; set; }
        public DbSet<Order> OrderSet { get; set; }
        public DbSet<OrderItem> OrderItemSet { get; set;}

    }

    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        public DateOnlyConverter() : base(dateOnly =>
                    dateOnly.ToDateTime(TimeOnly.MinValue),
                dateTime => DateOnly.FromDateTime(dateTime))
        { }
    }
}
/*
 * 
In visual studio choose : Tools -> Nuget Package Manager -> Package Manager Console

In the console window type following 2 commands (hit enter after each command)

     add-migration CreateDB  

     update-database –verbose 

Note : 

add-migration command creates the migration folder which has code for creating and maintaining database. this should only be run first time

update-database command should be run every time you change Db (or if DB doesn't exist because you are running this on a new/different PC
*/