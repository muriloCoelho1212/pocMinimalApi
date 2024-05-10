using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Routes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEntityFrameworkNpgsql().AddDbContext<TodoDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

TodoEndpoints.Map(app);

app.Run();
return;
