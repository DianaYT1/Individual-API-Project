using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Contracts;
using Services.Implementations;
using Services.Data.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<ApplicationDbContext>(_ => new ApplicationDbContext("Server=DESKTOP-HCQHH6R\\SQLEXPRESS;Database=myDataBase;Trusted_Connection=True;Encrypt=False;"));
builder.Services.AddMemoryCache();


builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
