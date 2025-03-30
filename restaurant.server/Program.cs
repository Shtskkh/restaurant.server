using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using restaurant.server.Context;
using restaurant.server.DTOs;
using restaurant.server.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<RestaurantContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("RestaurantContext"), 
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettingsModel>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddScoped<IPositionsRepository, PositionsRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Run();