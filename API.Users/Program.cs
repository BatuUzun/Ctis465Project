using APP.Users.Domain;
using APP.Users.Features;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("UsersDb");
builder.Services.AddDbContext<UsersDb>(options => options.UseSqlServer(connectionString));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(UsersDbHandler).Assembly));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
