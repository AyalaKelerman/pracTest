using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace ConsoleAppWithApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureServices(services =>
                {
                    services.AddCors(options =>
                    {
                        options.AddPolicy("AllowReact",
                            builder =>
                            {
                                builder.WithOrigins("http://localhost:5173") // Update with your React frontend URL
                                       .AllowAnyHeader()
                                       .AllowAnyMethod();
                            });
                    });

                    services.AddRouting();
                })
                .Configure(app =>
                {
                    app.UseRouting();

                    // Apply CORS middleware globally
                    app.UseCors("AllowReact");

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/", async context =>
                        {
                            await context.Response.WriteAsync("Hello from C# Console App!");
                        });

                        endpoints.MapGet("/api/data", async context =>
                        {
                            // Example data response
                            await context.Response.WriteAsync("[\"data1\", \"data2\", \"data3\"]");
                        });
                    });
                })
                .Build();

            host.Run();
        }
    }
}
