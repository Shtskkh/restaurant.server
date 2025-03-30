using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using restaurant.server.Context;
using restaurant.server.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<RestaurantContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("RestaurantContext"), 
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddScoped<IPositionsRepository, PositionsRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.Run();