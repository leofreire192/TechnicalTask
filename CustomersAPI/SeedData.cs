using CustomersAPI.Models;

namespace CustomersAPI
{
    public static class SeedData
    {
        public static void Initialize(CustomerDbContext context)
        {
            // Check if the database has already been seeded
            if (context.Customers.Any())
            {
                return;   
            }

            var customers = new Customer[]
            {
            new Customer { FirstName = "Dexter", LastName = "Morgan", Email = "Dexter.Morgan@miamimetro.com", PhoneNumber = "305-866-2095" },
            new Customer { FirstName = "Angel", LastName = "Batista", Email = "Angel.Batista@miamimetro.com", PhoneNumber = "305-228-0971" }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }

}
