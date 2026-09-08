using System.ComponentModel.DataAnnotations;
using RecipeApp_Eboaguillaume.Models;

namespace RecipeApp_Eboaguillaume.Tests.Models;

public class RecipeValidationTests
{
    private static IList<ValidationResult> Validate(object model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        return results;
    }

    [Fact]
    public void Recipe_WithEmptyTitle_FailsValidation()
    {
        var recipe = new Recipe { Title = "", Servings = 4 };

        var results = Validate(recipe);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Recipe.Title)));
    }

    [Fact]
    public void Recipe_WithTitleOver120Chars_FailsValidation()
    {
        var recipe = new Recipe { Title = new string('a', 121), Servings = 4 };

        var results = Validate(recipe);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Recipe.Title)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(49)]
    public void Recipe_WithServingsOutOfRange_FailsValidation(int servings)
    {
        var recipe = new Recipe { Title = "Valide", Servings = servings };

        var results = Validate(recipe);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(Recipe.Servings)));
    }

    [Fact]
    public void Recipe_WithValidData_PassesValidation()
    {
        var recipe = new Recipe { Title = "Recette valide", Servings = 4 };

        var results = Validate(recipe);

        Assert.Empty(results);
    }

    [Fact]
    public void IngredientLine_WithEmptyName_FailsValidation()
    {
        var ingredient = new IngredientLine { Name = "" };

        var results = Validate(ingredient);

        Assert.Contains(results, r => r.MemberNames.Contains(nameof(IngredientLine.Name)));
    }
}
