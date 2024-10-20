using Microsoft.EntityFrameworkCore;
using DMS.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DmsDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DmsDb"),
    new MySqlServerVersion(new Version(8, 0, 0))));



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DMS API", Version = "v1" });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build(); // Ensure 'app' is declared after services are configurednNnnnn

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Show detailed errors in development
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DMS API v1"));
}

app.UseCors("AllowLocalhost");
app.UseAuthorization();
app.MapControllers();
app.Run();
