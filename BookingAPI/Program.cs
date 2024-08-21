using BookingAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<BookingService>(); // Register the combined service

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
