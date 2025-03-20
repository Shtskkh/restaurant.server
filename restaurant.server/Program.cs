using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using restaurant.server.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<RestaurantContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("RestaurantContext")));

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();

app.Run();