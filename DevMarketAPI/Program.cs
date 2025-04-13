using AutoMapper;
using DevMarketAPI.Authorization.Handlers;
using DevMarketAPI.Authorization.Requirements;
using DevMarketAPI.Data;
using DevMarketAPI.Helpers;
using DevMarketAPI.Models;
using DevMarketAPI.Routing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],


        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],


        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])), 

    };

    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StudioAccessPolicy", policy =>
       policy.Requirements.Add(new StudioAccessRequirement()));
});
builder.Services.AddSingleton<IAuthorizationHandler, StudioAccessHandler>();
builder.Services.AddAutoMapper(typeof(StudioProfileMapping));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
