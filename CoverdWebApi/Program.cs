using Databases;

namespace CoverdWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();
            app.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            
            builder.ConfigureServices((context, services) =>
                {
                    services.AddCors(options =>
                    {
                        options.AddPolicy("AllowReact", policy =>
                            policy.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader());
                    });
                    services.AddControllers();
                    services.AddSingleton<IDatabase, Database>();
                });
            
            builder.ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
            return builder;
        }
    }
}