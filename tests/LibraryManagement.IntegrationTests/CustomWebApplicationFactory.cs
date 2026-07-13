using LibraryManagement.DataAccess.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private const string TestConnectionString =
        "Host=localhost;Port=5432;Database=LibraryDB_Test;Username=postgres;Password=postgres";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<LibraryDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<LibraryDbContext>(options =>
                options.UseNpgsql(TestConnectionString));
        });
    }

    // Test DB'sini hazırla (dispose sorununu önlemek için host dışında)
    public void EnsureDatabaseCreated()
    {
        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
        db.Database.EnsureCreated();
    }
}