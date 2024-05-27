using Microsoft.EntityFrameworkCore;
using ToDoListProject.DAL;
using ToDoListProject.DAL.Interfaces;
using ToDoListProject.DAL.Repositories;
using ToDoListProject.Domain.Entity;
using ToDoListProject.Service.Implementations;
using ToDoListProject.Service.Interfaces;

namespace ToDoListProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("MSSQL");
            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddScoped<IBaseRepository<TaskEntity>, TaskRepository>();
            builder.Services.AddScoped<ITaskService, TaskService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Task/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Task}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
