using Microsoft.Extensions.Configuration;
using RecipeApp_Eboaguillaume.Models;
using RecipeApp_Eboaguillaume.Services;

namespace RecipeApp_Eboaguillaume.Tests.Services;

file sealed class FakeHttpClientFactory : IHttpClientFactory
{
    public HttpClient CreateClient(string name) => new();
}

public class ChatGptServiceTests
{
    private static ChatGptService CreateService(string apiKey = "test-key")
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["OpenAI:ApiKey"] = apiKey })
            .Build();

        return new ChatGptService(new FakeHttpClientFactory(), config);
    }

    [Fact]
    public void Constructor_WithoutApiKey_Throws()
    {
        var config = new ConfigurationBuilder().Build();

        Assert.Throws<InvalidOperationException>(() => new ChatGptService(new FakeHttpClientFactory(), config));
    }

    [Fact]
    public void ParseResult_ExtractsStepsTagsAndShoppingList()
    {
        var service = CreateService();
        var content = """
            SECTION:DESCRIPTION
            Une recette rapide et savoureuse.

            SECTION:STEPS
            1. Couper les légumes.
            2. Cuire à feu moyen.

            SECTION:TAGS
            rapide
            végétarien

            SECTION:SHOPPINGLIST
            Carottes|2 pièces
            Oignon
            """;

        var result = service.ParseResult(content);

        Assert.Equal("Une recette rapide et savoureuse.", result.Description);
        Assert.Equal(new[] { "1. Couper les légumes.", "2. Cuire à feu moyen." }, result.Steps);
        Assert.Equal(new[] { "rapide", "végétarien" }, result.SuggestedTags);
        Assert.Equal(new[] { "Carottes|2 pièces", "Oignon" }, result.ShoppingList);
    }

    [Fact]
    public void ParseResult_WithMissingSections_ReturnsEmptyCollections()
    {
        var service = CreateService();

        var result = service.ParseResult("Réponse inattendue sans sections.");

        Assert.Empty(result.Steps);
        Assert.Empty(result.SuggestedTags);
        Assert.Empty(result.ShoppingList);
    }

    [Fact]
    public void BuildPrompt_IncludesRecipeTitleAndIngredients()
    {
        var service = CreateService();
        var recipe = new Recipe
        {
            Title = "Ndole",
            Description = "Plat camerounais",
            Ingredients = new List<IngredientLine>
            {
                new() { Name = "Feuilles de ndole", Qty = new Quantity { Amount = 500, Unit = Unit.Gram } }
            }
        };

        var prompt = service.BuildPrompt(recipe);

        Assert.Contains("Ndole", prompt);
        Assert.Contains("Feuilles de ndole", prompt);
        Assert.Contains("500", prompt);
    }
}
