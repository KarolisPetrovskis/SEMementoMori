using Microsoft.AspNetCore.Mvc;
using System.IO;
using MementoMori.Server;

namespace MementoMori.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CardFileController : ControllerBase
	{
		private readonly string _filePath;

		public CardFileController()
		{
			string serverDirectory = Directory.GetCurrentDirectory();
			// Assuming the file is always 001.txt if you want to display more files in a static way then you can do modifications inf GetFileContent
			_filePath = Path.Combine(serverDirectory, "CardFile", "001.txt"); 
		}

		[HttpGet("getFileContent")]
		public IActionResult GetFileContent()
		{
			if (!System.IO.File.Exists(_filePath))
			{
				return NotFound("Error: The deck file does not exist.");
			}

			try
			{
                // Do all the information packaging here
                CardFileReader deckInfo = new CardFileReader(_filePath);
				string[] fileContent = deckInfo.ExtractCards();
				// Return file content as an array of strings
				return Ok(fileContent);
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error reading file: {ex.Message}");
			}
		}
	}
}
