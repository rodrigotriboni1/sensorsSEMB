using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sensores_data;
using sensores_data.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SensorDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString(name: "SensorDbConnection")));
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString(name: "SensorDbConnection")));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
builder.WebHost.UseUrls("http://localhost:5202", "http://192.168.56.1:5202", "http://192.168.100.28:5202");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection(); 
app.UseAuthorization();
app.MapControllers();


// Database Migration and Seeding (Optional, but highly recommended)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SensorDbContext>();
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate(); // Apply any pending migrations
        }
        // You can add database seeding here if needed.
        //  e.g., AddSeedData(context); // Call a seeding method 

    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating/migrating the database.");
    }
}

app.Run();

