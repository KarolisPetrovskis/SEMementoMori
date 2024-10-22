using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MementoMori.Server.Database;

namespace MementoMori.Server
{
    class TestDb
    {
        static void Main(string[] args)
        {
            // Set up configuration to read from appsettings.json or environment variables
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Pass the configuration to AppDbContext
            using (var context = new AppDbContext(configuration))
            {
                // Ensure the database is created
                context.Database.EnsureCreated();

                // Check if there are any employees, if not, add one
                if (!context.Employees.Any())
                {
                    context.Employees.Add(new Employee { LastName = "Software" });
                    context.SaveChanges();
                }

                // Query employees with the last name "Doe"
                var employees = context.Employees.Where(e => e.LastName == "Doe").ToList();
                foreach (var employee in employees)
                {
                    Console.WriteLine($"Employee ID: {employee.Id}, Last Name: {employee.LastName}");
                }
            }
        }
    }
}