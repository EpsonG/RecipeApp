using RecipeApp_Eboaguillaume.Models;

namespace RecipeApp_Eboaguillaume.Tests.Models;

public class RecipeCloneTests
{
    private static Recipe CreateSampleRecipe() => new()
    {
        Id = 42,
        Title = "Tarte aux pommes",
        Description = "Dessert classique",
        Servings = 6,
        IsPublished = true,
        Tags = new List<string> { "dessert", "pâtisserie" },
        Ingredients = new List<IngredientLine>
        {
            new() { Name = "Pommes", Qty = new Quantity { Amount = 5, Unit = Unit.Piece } }
        },
        Steps = new List<InstructionStep>
        {
            new() { Order = 1, Text = "Préchauffer le four." }
        }
    };

    [Fact]
    public void Clone_ResetsIdToZero()
    {
        var original = CreateSampleRecipe();

        var clone = original.Clone();

        Assert.Equal(0, clone.Id);
    }

    [Fact]
    public void Clone_PrefixesTitleWithCopyOf()
    {
        var original = CreateSampleRecipe();

        var clone = original.Clone();

        Assert.Equal("Copy of Tarte aux pommes", clone.Title);
    }

    [Fact]
    public void Clone_CopiesScalarValues()
    {
        var original = CreateSampleRecipe();

        var clone = original.Clone();

        Assert.Equal(original.Description, clone.Description);
        Assert.Equal(original.Servings, clone.Servings);
        Assert.Equal(original.IsPublished, clone.IsPublished);
    }

    [Fact]
    public void Clone_ProducesIndependentCollections()
    {
        var original = CreateSampleRecipe();

        var clone = original.Clone();
        clone.Tags.Add("nouveau-tag");
        clone.Ingredients[0].Name = "Poires";

        Assert.DoesNotContain("nouveau-tag", original.Tags);
        Assert.Equal("Pommes", original.Ingredients[0].Name);
    }

    [Fact]
    public void Clone_CopiesIngredientsAndSteps()
    {
        var original = CreateSampleRecipe();

        var clone = original.Clone();

        Assert.Single(clone.Ingredients);
        Assert.Equal("Pommes", clone.Ingredients[0].Name);
        Assert.Equal(5, clone.Ingredients[0].Qty.Amount);
        Assert.Single(clone.Steps);
        Assert.Equal("Préchauffer le four.", clone.Steps[0].Text);
    }
}
