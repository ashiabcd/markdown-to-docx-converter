/**
 * Program.cs
 *
 * This is the main entry point for the .NET Core Web API.
 * It sets up and configures:
 * - Dependency injection for services (e.g., Markdown to DOCX conversion)
 * - Controller support (routing)
 * - Swagger for API documentation/testing
 * - CORS policy to allow cross-origin requests from the frontend
 * - The HTTP request pipeline including middleware and routing
 *
 * The final product is a clean Web API that accepts Markdown content and returns a .docx file.
 */

using MarkdownToDocxApi.Services; // Import the custom service

var builder = WebApplication.CreateBuilder(args);

// -------------------------------------------
// Register services (Dependency Injection)
// -------------------------------------------

// Adds support for controller-based APIs
builder.Services.AddControllers();

// Registers endpoint explorer (for Swagger/OpenAPI metadata)
builder.Services.AddEndpointsApiExplorer();

// Registers Swagger UI generator (interactive API docs)
builder.Services.AddSwaggerGen();

// Register the Markdown-to-DOCX conversion service
// Scoped = one instance per HTTP request (ideal for stateless services)
builder.Services.AddScoped<MarkdownToDocxService>();

// -------------------------------------------
// Configure CORS Policy
// -------------------------------------------

// Enables CORS so that a frontend app (e.g., React on localhost:5173) can call this backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   // Allow requests from any domain
              .AllowAnyMethod()   // Allow all HTTP methods (GET, POST, etc.)
              .AllowAnyHeader();  // Allow all custom headers (like Content-Type)
    });
});

// -------------------------------------------
// Build the WebApplication instance
// -------------------------------------------
var app = builder.Build();

// -------------------------------------------
// Configure the Middleware Pipeline
// -------------------------------------------

// Enable Swagger middleware in development only
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();     // Serve the Swagger JSON
    app.UseSwaggerUI();   // Serve the Swagger web UI at /swagger
}

// Redirect HTTP requests to HTTPS automatically
app.UseHttpsRedirection();

// Enable CORS using the previously defined "AllowAll" policy
app.UseCors("AllowAll");

// Add middleware for handling authorization (no auth yet, but prepares the pipeline)
app.UseAuthorization();

// Maps controller routes to endpoints (e.g., /api/markdown/convert)
app.MapControllers();

// Start and run the app — listens for HTTP requests
app.Run();
