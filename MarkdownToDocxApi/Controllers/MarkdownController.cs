/**
 * MarkdownController.cs
 *
 * This is the API controller that handles incoming HTTP POST requests for converting
 * Markdown text into a downloadable DOCX file.
 *
 * Key responsibilities:
 * - Accept Markdown content from the frontend
 * - Validate the input
 * - Use MarkdownToDocxService to generate a DOCX byte array
 * - Return the result as a file download stream (.docx MIME type)
 *
 * Endpoint: POST /api/markdown/convert
 * Input: JSON object with `markdown` string
 * Output: .docx file as an application/octet-stream response
 */

using Microsoft.AspNetCore.Mvc;
using MarkdownToDocxApi.Services;

namespace MarkdownToDocxApi.Controllers
{
    // Marks this class as a Web API controller and enables automatic model binding/validation
    [ApiController]

    // Route base URL: e.g., /api/markdown
    [Route("api/[controller]")]
    public class MarkdownController : ControllerBase
    {
        // Service responsible for converting Markdown to DOCX
        private readonly MarkdownToDocxService _docService;

        // Constructor for injecting the conversion service (registered via Dependency Injection)
        public MarkdownController(MarkdownToDocxService docService)
        {
            _docService = docService;
        }

        // Inner class used to define the structure of the incoming POST request body
        public class MarkdownRequest
        {
            // Property to hold the Markdown string sent by the frontend
            public string Markdown { get; set; }
        }

        // POST endpoint: /api/markdown/convert
        // Accepts a MarkdownRequest JSON object, returns a .docx file as a byte stream
        [HttpPost("convert")]
        public IActionResult ConvertToDocx([FromBody] MarkdownRequest request)
        {
            // Basic validation: check if Markdown string is empty or null
            if (string.IsNullOrWhiteSpace(request.Markdown))
                return BadRequest("Markdown content is empty.");

            // Convert Markdown to DOCX byte stream using the service
            var fileBytes = _docService.ConvertMarkdownToDocx(request.Markdown);

            // Return the DOCX file as a stream with appropriate MIME type and download name
            return File(
                fileBytes, 
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document", // DOCX MIME type
                "output.docx" // Suggested filename
            );
        }
    }
}
