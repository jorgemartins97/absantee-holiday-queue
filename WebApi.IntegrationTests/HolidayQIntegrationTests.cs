using System.Text.Json;
using DataModel.Repository;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IntegrationTests.Helpers;

namespace WebApi.IntegrationTests;

public class HolidayQIntegrationTests : IClassFixture<IntegrationTestsWebApplicationFactory<Program>>
{
    private readonly IntegrationTestsWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public HolidayQIntegrationTests(IntegrationTestsWebApplicationFactory<Program> factory)
    {
        _factory = factory;

        _client = factory.CreateClient();
    }


    [Theory]
    [InlineData("/api/Holiday")]
    [InlineData("/api/Holiday/1")]
    [InlineData("/api/Holiday/periods/1?startDate=2024-01-01&endDate=2024-12-12")]
    [InlineData("/api/Holiday/1/colabsComFeriasSuperioresAXDias")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        // var client = _factory.CreateClient();

        // Act
        var response = await _client.GetAsync(url);
        //requisição get para a URL

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }


    [Theory]
    [InlineData("/api/Holiday/1")]
    public async Task GetById_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            //Permite acessar os serviços configurados no contêiner de injeção de dependência.
            var scopedServices = scope.ServiceProvider;

            // Obtém uma instância do contexto de banco de dados AbsanteeContext
            var db = scopedServices.GetRequiredService<AbsanteeContext>();

            //Reinicializa o banco de dados
            Utilities.ReinitializeDbForTests(db);
        }

        // var client = _factory.CreateClient();

        // Act
        //Envia uma requisição GET para a URL
        var response = await _client.GetAsync(url);

        // Assert
        //Verifica se o código de status da resposta está na faixa de sucesso (200-299)
        response.EnsureSuccessStatusCode(); 

        // Verifica se o  conteúdo da resposta
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }


    [Theory]
    [InlineData("/api/Holiday", 5)]
    [InlineData("/api/Holiday/periods/1?startDate=2024-01-01&endDate=2024-12-12",2)]
    [InlineData("/api/Holiday/2/colabsComFeriasSuperioresAXDias",3)]
    public async Task Get_ReturnData(string url, int expected)
    {
        // Arrange
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AbsanteeContext>();

            Utilities.ReinitializeDbForTests(db);
        }

        // var client = _factory.CreateClient();

        var response = await _client.GetAsync(url);

        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.NotNull(responseBody);

        var jsonDocument = JsonDocument.Parse(responseBody);
        var jsonArray = jsonDocument.RootElement;

        Assert.True(jsonArray.ValueKind == JsonValueKind.Array, "Response body is not a JSON array");
        Assert.Equal(expected, jsonArray.GetArrayLength());
    }
}