using MementoMori.Server;
using MementoMori.Server.Database;
using MementoMori.Server.Service;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register CardFileReader as a service using its interface
builder.Services.AddScoped<ICardFileReader, CardFileReader>();
builder.Services.AddDbContext<AppDbContext, AppDbContext>();
builder.Services.AddScoped<DeckHelper, DeckHelper>();
builder.Services.AddScoped<AuthService, AuthService>();
builder.Services.AddControllers();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy =>
        {
            policy.WithOrigins("https://localhost:5173") // Allow the React frontend
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Include credentials if necessary
        });
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
