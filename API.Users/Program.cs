using APP.Users;
using APP.Users.Domain;
using APP.Users.Features;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("UsersDb");
builder.Services.AddDbContext<UsersDb>(options => options.UseSqlServer(connectionString));
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(UsersDbHandler).Assembly));



var section = builder.Configuration.GetSection(nameof(AppSettings));
section.Bind(new AppSettings());



// Authentication:
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(config =>
{
    config.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = AppSettings.Issuer,
        ValidAudience = AppSettings.Audience,
        IssuerSigningKey = AppSettings.SigningKey,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});









builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Swagger:
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "API",
        Version = "v1"
    });
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1a2b3c\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {
            new OpenApiSecurityScheme() {
                Reference = new OpenApiReference() {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// Authentication:
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
