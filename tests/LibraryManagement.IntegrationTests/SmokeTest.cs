using System.Net;

namespace LibraryManagement.IntegrationTests;

public class SmokeTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public SmokeTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_Books_WithoutToken_Returns401()
    {
        // Arrange: client oluştur (bu, host'u ayağa kaldırır), sonra test DB'sini hazırla
        var client = _factory.CreateClient();
        _factory.EnsureDatabaseCreated();

        // Act: token olmadan korumalı endpoint'e istek at
        var response = await client.GetAsync("/api/Books");

        // Assert: JWT koruması çalışıyorsa 401 dönmeli
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}