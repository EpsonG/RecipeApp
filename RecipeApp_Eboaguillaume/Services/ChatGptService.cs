using RecipeApp_Eboaguillaume.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RecipeApp_Eboaguillaume.Services;

public class ChatGptService : IChatGptService
{
    // HttpClient instance for making requests
    private readonly HttpClient _http;
    private readonly string _apiKey;

    // Constructor to initialize HttpClient and API key
    public ChatGptService(IHttpClientFactory factory, IConfiguration config)
    {
        _http = factory.CreateClient("OpenAI");
        _apiKey = config["OpenAI:ApiKey"] 
                  ?? throw new InvalidOperationException("OpenAI:ApiKey non configurée.");
    }
    // Generates recipe details using OpenAI's GPT model
    public async Task<RecipeAiResult> GenerateForRecipeAsync(Recipe recipe, CancellationToken ct = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "v1/chat/completions");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        var prompt = BuildPrompt(recipe);

        var body = new
        {   // On utilise le modèle gpt-4o-mini
            model = "gpt-4o-mini",
            messages = new[]
            {
                new { role = "system", content = "You are a helpful culinary assistant. Generate recipe details in plain text with sections: STEPS, TAGS, SHOPPINGLIST." },
                new { role = "user", content = prompt }
            }
        };
        // On sérialise le corps de la requête en JSON
        request.Content = JsonContent.Create(body);
        // On envoie la requête à l'API OpenAI
        using var response = await _http.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode)
        {
            var err = await response.Content.ReadAsStringAsync(ct);
            throw new Exception($"Erreur OpenAI {(int)response.StatusCode}: {err}");
        }
        // On lit le contenu de la réponse
        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
        // On parse la réponse
        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString() ?? string.Empty;

        return ParseResult(content);
    }
    // Builds the prompt to send to the AI
    private string BuildPrompt(Recipe recipe)
    {
        var ingredientsText = string.Join("\n", recipe.Ingredients.Select(i =>
            $"- {i.Qty.Amount} {i.Qty.Unit} {i.Name}"));

        return $"""
        Analyze the following recipe and generate the details exactly in the format below.
        Respond in plain text only.

        Recipe Title: {recipe.Title}
        Description: {recipe.Description}
        Ingredients:
        {ingredientsText}

        SECTION:DESCRIPTION
        [A short and appealing description of the recipe]

        SECTION:STEPS
        [Numbered list of preparation steps]

        SECTION:TAGS
        [List of simple keywords, one per line]

        SECTION:SHOPPINGLIST
        [List of items to buy, one per line, optional notes after '|']
        """;
    }

    // Parses the AI response into a RecipeAiResult object
    private RecipeAiResult ParseResult(string content)
    {
        var result = new RecipeAiResult();

        var sections = content.Split("SECTION:", StringSplitOptions.RemoveEmptyEntries);
        foreach (var sec in sections)
        {   // On traite chaque section
            var trimmed = sec.Trim();
            if (trimmed.StartsWith("STEPS", StringComparison.OrdinalIgnoreCase))
            {
                var lines = trimmed.Split('\n', StringSplitOptions.RemoveEmptyEntries).Skip(1);
                result.Steps = lines.Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            }
            else if (trimmed.StartsWith("DESCRIPTION", StringComparison.OrdinalIgnoreCase))
            {
                result.Description = trimmed.Substring("DESCRIPTION".Length).Trim();
            }
            else if (trimmed.StartsWith("TAGS", StringComparison.OrdinalIgnoreCase))
            {
                var lines = trimmed.Split('\n', StringSplitOptions.RemoveEmptyEntries).Skip(1);
                result.SuggestedTags = lines.Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            }
            else if (trimmed.StartsWith("SHOPPINGLIST", StringComparison.OrdinalIgnoreCase))
            {
                var lines = trimmed.Split('\n', StringSplitOptions.RemoveEmptyEntries).Skip(1);
                result.ShoppingList = lines.Select(l => l.Trim()).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            }
             else if (trimmed.StartsWith("DESCRIPTION", StringComparison.OrdinalIgnoreCase))
            {
                // On prend tout le texte après "DESCRIPTION" jusqu'à la fin ou une nouvelle section
                var desc = trimmed.Substring("DESCRIPTION".Length).Trim();
                result.Description = desc;
            }
        }

        return result;
    }
}
