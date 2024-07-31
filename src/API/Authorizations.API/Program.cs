namespace Authorizations.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                Console.WriteLine("Update database started");
                context.Database.SetCommandTimeout(TimeSpan.FromHours(2));
                context.Database.EnsureCreated(); //Migrate();
                Console.WriteLine("Update database ended");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
