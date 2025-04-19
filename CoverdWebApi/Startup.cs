using CoverdWebApi.Controllers;
using Databases;
using TransactionHandler.Interfaces;

namespace CoverdWebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowReact", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            
            services.AddControllers()
                .AddApplicationPart(typeof(PingController).Assembly)
                .AddApplicationPart(typeof(BlackjackController).Assembly); // Enable attribute-based controllers
            services.AddSingleton<IDatabase, Database>(); // Ensure your DB setup
            services.AddSingleton<ITransactionHandler, TransactionHandler.TransactionHandler>();
            
            foreach (var svc in services)
            {
                Console.WriteLine($"{svc.ServiceType.FullName}");
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Set minimum threads (worker threads, IO threads)
            ThreadPool.SetMinThreads(workerThreads: 10, completionPortThreads: 10);

            // Optional: Set maximum threads
            ThreadPool.SetMaxThreads(workerThreads: 100, completionPortThreads: 100);
            
            app.UseCors("AllowReact");
            app.UseRouting();
            
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Map attribute-routed controllers
            });
        }
    }
}