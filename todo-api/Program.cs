using System.Reflection;
using todo_api.Repositories;
using todo_api.Services;
using todo_api.Models;
using Microsoft.EntityFrameworkCore;

namespace todo_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Todo API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            builder.Services.AddScoped<ITodoRepository, TodoRepository>();
            builder.Services.AddScoped<ITodoService, TodoService>();

            var env = builder.Environment;
            if (!env.IsEnvironment("Testing"))
            {
                builder.Services.AddDbContext<TodoDbContext>(opt =>
                    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            }

            var app = builder.Build();

            if (!env.IsEnvironment("Testing"))
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
