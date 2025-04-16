using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using restaurant.server.Context;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();