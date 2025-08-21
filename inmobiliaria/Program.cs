using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using inmobiliaria.core.Interfaces;
using inmobiliaria.infrastructure.Persistencia;              // RealEstateDbContext
using inmobiliaria.infrastructure.Service.UnitOfWork;        // UnitOfWork
using inmobiliaria.infrastructure.Dependencias.Adapters;     // InmobiliariaAdapter
using inmobiliaria.application.Service;
using Microsoft.OpenApi.Models;                       // InmobiliariaService

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger + JWT en Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Inmobiliaria API", Version = "v1" });

    // ---- (Opcional) Autorización por JWT en Swagger ----
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Inserta tu token JWT con el esquema Bearer. Ej: 'Bearer 12345abcdef'"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { new OpenApiSecurityScheme
            { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
          Array.Empty<string>() }
    });
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// DbContext (usa tu clave IDatabase del appsettings)
var cs = builder.Configuration.GetConnectionString("IDatabase")
         ?? throw new InvalidOperationException("Falta ConnectionString 'IDatabase'.");
builder.Services.AddDbContext<RealEstateDbContext>(opt => opt.UseSqlServer(cs));

// (Opcional) JWT Authentication usando JwtSettings del appsettings
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSection.GetValue<string>("SecretKey");
if (!string.IsNullOrWhiteSpace(secret))
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,  // ajústalo si tienes issuer/audience
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero
            };
        });
}

// Hexagonal DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IInmobiliariaAdapter, InmobiliariaAdapter>();
builder.Services.AddScoped<IInmobiliariaService, InmobiliariaService>();

var app = builder.Build();

// Swagger UI en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Inmobiliaria API v1");
        c.RoutePrefix = "swagger"; // UI en https://localhost:xxxx/swagger
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
