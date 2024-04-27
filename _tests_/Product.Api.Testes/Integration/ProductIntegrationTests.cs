using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Api.Domain.Responses;
using Product.Api.Testes.Suport;
using System.Net.Http.Json;

namespace Product.Api.Testes.Integration;

public class ProductIntegrationTests : IntegrationTestBase
{

    [Fact(DisplayName = "Should get all producs with success")]
    public async Task Should_Get_All_Products()
    {
        // Arrange            
        var client = this.CreateClient();
        var products = BsonProductBuilder.Build(5);
        var config = this.Services.GetRequiredService<IConfiguration>();
        await dbContainerTest.SetValuesMongo(config, products.ToList());

        // Act
        var response = await client.GetAsync("product/");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadFromJsonAsync<List<ProductQueryResponse>>();
        Assert.NotNull(responseBody);
        Assert.Equal(responseBody?.Count, 5);
        Assert.Equal("application/json; charset=utf-8", response?.Content?.Headers?.ContentType?.ToString());
    }

    [Fact(DisplayName = "Should get all producs with empty response")]
    public async Task Should_Get_All_Products_With_Empty_Response()
    {
        // Arrange            
        var client = this.CreateClient();

        // Act
        var response = await client.GetAsync("product/");

        // Assert
        Assert.Equal(response?.StatusCode, System.Net.HttpStatusCode.NoContent);
    }
}
