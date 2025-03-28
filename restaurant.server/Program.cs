using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextPool<RestaurantContext>(opt => 
    opt.UseNpgsql(builder.Configuration.GetConnectionString("RestaurantContext")));

var app = builder.Build();

app.Run();