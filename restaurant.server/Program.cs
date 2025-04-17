using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using restaurant.server.Context;
using restaurant.server.Repositories;
using restaurant.server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<RestaurantContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("RestaurantContext"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ASP.NET API ресторана"
    });
});

builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IDishesRepository, DishesRepository>();

builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IOrderService, OrdersService>();
builder.Services.AddScoped<IDishesService, DishesService>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();