using Microsoft.EntityFrameworkCore;
using restaurant.server.Context;
using restaurant.server.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<RestaurantContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("RestaurantContext"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

builder.Services.AddScoped<IStaffRepository, StaffRepository>();

var app = builder.Build();

app.Run();