using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace LibraryManagement.IntegrationTests;

public class AuthTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AuthTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Register_Then_Login_Returns_Token()
    {
        var client = _factory.CreateClient();
        _factory.EnsureDatabaseCreated();

        var token = await RegisterAndLoginAsync(client);

        Assert.False(string.IsNullOrEmpty(token));
    }

    [Fact]
    public async Task Get_Books_WithValidToken_Returns200()
    {
        var client = _factory.CreateClient();
        _factory.EnsureDatabaseCreated();

        // Token al ve Authorization header'ına koy
        var token = await RegisterAndLoginAsync(client);
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        // Artık token'lı istek: 401 değil 200 beklenir
        var response = await client.GetAsync("/api/Books");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // Yardımcı: benzersiz kullanıcı register et, login ol, token'ı döndür
    private static async Task<string> RegisterAndLoginAsync(HttpClient client)
    {
        var email = $"test_{Guid.NewGuid():N}@library.com";
        const string password = "Test1234!";

        var registerBody = new
        {
            firstName = "Furkan",
            lastName = "Isik",
            email = email,
            password = password,
            phoneNumber = "5551234567",
            address = "Istanbul"
        };
        await client.PostAsJsonAsync("/api/Auth/register", registerBody);

        var loginBody = new { email = email, password = password };
        var loginResponse = await client.PostAsJsonAsync("/api/Auth/login", loginBody);
        var result = await loginResponse.Content.ReadFromJsonAsync<LoginResult>();

        return result!.Token;
    }

    private class LoginResult
    {
        public string Token { get; set; } = string.Empty;
    }
}