using Core.DTOs;
using Core.Interfaces.Services;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Cargar la configuración desde el archivo appsettings.json
builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

// Registrar los valores de configuración de JWT
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Se configura la conexion - Fer Codas 
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers();
builder.Services.AddRepositories();
builder.Services.AddDatabase(builder.Configuration);



// Se configura la autenticación con JWT(Json Web Token)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "EXPENSE",
            ValidAudience = "Expense",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MiClaveSecretaSuperSeguraQueEsDeMasDe32Bytes1234"))
            
        };
    });

builder.Services.AddAuthorization();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
