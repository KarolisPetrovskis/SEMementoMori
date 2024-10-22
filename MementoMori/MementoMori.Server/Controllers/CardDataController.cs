using Microsoft.AspNetCore.Mvc;
using MementoMori.Server.Extensions;
using MementoMori.Server.DTOS;
using Microsoft.EntityFrameworkCore;
using MementoMori.Server.Database;

namespace MementoMori.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardDataController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        [HttpPost("createCard")]
        public IActionResult PostInputData([FromBody] CardData data)
        {
            // Usage of extension method
            if (!data.IsValid())
            {
                return BadRequest("Invalid input data");
            }
            
            
            try
            {
                // Usage of the extension method to initialize FileWriter class
                var fileWriter = this.InitializeFileWriter();
                fileWriter.CreateFile(data.Question, data.Answer, data.DeckId.ToString());



                // use TestDb class

                // Ensure the database is created
                _dbContext.Database.EnsureCreated();

                // Add a default employee if none exist
                if (!_dbContext.Employees.Any())
                {
                    _dbContext.Employees.Add(new Employee { LastName = "Software" });
                    _dbContext.SaveChanges();
                }



                // Query employees with the last name "Doe"
                var employees = _dbContext.Employees.Where(e => e.LastName == "Doe").ToList();

                // Log or return employee data (if necessary for your use case)
                foreach (var employee in employees)
                {
                    // You can log or return this information if needed
                    Console.WriteLine($"Employee ID: {employee.Id}, Last Name: {employee.LastName}");
                }



                return Ok(new { message = "Data received successfully", question = data.Question, text = data.Answer, cardId = data.DeckId });
            }
            catch (InvalidOperationException ex)
            {
                // If the file already exists, return a 409 Conflict status code
                return Conflict(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred: " + ex.Message });
            }
        }
    }
}
