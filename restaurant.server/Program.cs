using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<RestaurantContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("RestaurantContext")));

builder.Services.AddScoped<IPositionsRepository, PositionsRepository>();

var app = builder.Build();

app.Run();