using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using Vehicle_Registration_System.Data;
using Vehicle_Registration_System.Repositories.Implementations;
using Vehicle_Registration_System.Repositories.Interfaces;
using Vehicle_Registration_System.Services.AuthenticationService;
using Vehicle_Registration_System.Services.Implementations;
using Vehicle_Registration_System.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot configuration = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
.Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
            }
        },
        new string[] {}
    }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityCore<IdentityUser>()
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "MyIssuer",
        ValidAudience = "MyAudience",
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.ASCII.GetBytes("Mmx44IfURe84A/c4i0g2eY8m/DEhzUzXyyVPwKIo2SU="))
    };
});

builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>();

builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IOwnerService, OwnerService>();
builder.Services.AddScoped<IInsuranceService, InsuranceService>();

builder.Services.AddScoped<TokenService, TokenService>();
builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddScoped<RoleManager<IdentityRole>>();


builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
