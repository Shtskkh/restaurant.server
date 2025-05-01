using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using restaurant.server.Context;
using restaurant.server.DTOs;
using restaurant.server.Repositories;
using restaurant.server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(7280, listenOptions => listenOptions.UseHttps());
});

builder.Services.AddCors();

builder.Services.AddDbContextPool<RestaurantContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("RestaurantContext"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.Configure<JwtSettingsModel>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>(),
            ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>(),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("Jwt:SecretKey").Get<string>() ??
                throw new InvalidOperationException()))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IDishesRepository, DishesRepository>();

builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IDishesService, DishesService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();