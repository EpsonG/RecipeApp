﻿using RecipeApp_Eboaguillaume.Models;

namespace RecipeApp_Eboaguillaume.Services;

/// <summary>
/// Service pour la gestion des recettes. Fournit des opérations CRUD pour les recettes.
/// </summary>
public class RecipeService : IRecipeService
{
    private readonly List<Recipe> _recipes = new();

    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="RecipeService"/> avec des données de recette initiales.
    /// </summary>
    public RecipeService()
    {
        _recipes.Add(new Recipe
        {
            Title = "Spaghetti aglio e olio",
            Description = "Pâtes ail & huile d’olive.",
            Servings = 2,
            Tags = new() { "italien", "rapide" },
            Ingredients =
            {
                new IngredientLine{ Name="Spaghetti", Qty=new Quantity{ Amount=200, Unit=Unit.Gram }},
                new IngredientLine{ Name="Gousses d’ail", Qty=new Quantity{ Amount=3, Unit=Unit.Piece }},
                new IngredientLine{ Name="Huile d’olive", Qty=new Quantity{ Amount=3, Unit=Unit.Tablespoon }},
                new IngredientLine{ Name="Flocons de piment", Qty=new Quantity{ Amount=0.5m, Unit=Unit.Teaspoon }},
                new IngredientLine{ Name="Persil", Qty=new Quantity{ Amount=2, Unit=Unit.Tablespoon }},
                new IngredientLine{ Name="Sel", Qty=new Quantity{ Amount=1, Unit=Unit.Teaspoon }},
            },
            Steps =
            {
                new InstructionStep{ Order=1, Text="Cuire les pâtes dans l’eau salée."},
                new InstructionStep{ Order=2, Text="Dorer doucement l’ail dans l’huile."},
                new InstructionStep{ Order=3, Text="Ajouter piment & persil; mélanger."},
            }
        }
        );
        _recipes.Add(new Recipe
        {
            Title = "Salade de quinoa",
            Description = "Salade fraîcheur au quinoa.",
            Servings = 4,
            Tags = new() { "végétarien", "sain" },
            Ingredients =
            {
                new IngredientLine{ Name="Quinoa", Qty=new Quantity{ Amount=200, Unit=Unit.Gram }},
                new IngredientLine{ Name="Concombre", Qty=new Quantity{ Amount=1, Unit=Unit.Piece }},
                new IngredientLine{ Name="Tomates cerises", Qty=new Quantity{ Amount=150, Unit=Unit.Gram }},
                new IngredientLine{ Name="Feta", Qty=new Quantity{ Amount=100, Unit=Unit.Gram }},
                new IngredientLine{ Name="Citron", Qty=new Quantity{ Amount=1, Unit=Unit.Piece }},
                new IngredientLine{ Name="Huile d’olive", Qty=new Quantity{ Amount=2, Unit=Unit.Tablespoon }},
                new IngredientLine{ Name="Sel et poivre", Qty=new Quantity{ Amount=1, Unit=Unit.Teaspoon }},
            },
            Steps =
            {
                new InstructionStep{ Order=1, Text="Cuire le quinoa selon les instructions."},
                new InstructionStep{ Order=2, Text="Couper concombre et tomates."},
                new InstructionStep{ Order=3, Text="Mélanger quinoa, légumes, feta, jus de citron et huile."},
            }
        }
        );
        _recipes.Add(new Recipe
        {
            Title = "Poulet rôti au four",
            Description = "Poulet croustillant et juteux.",
            Servings = 4,
            Tags = new() { "classique", "four" },
            Ingredients =
            {
                new IngredientLine{ Name="Poulet entier", Qty=new Quantity{ Amount=1, Unit=Unit.Piece }},
                new IngredientLine{ Name="Beurre", Qty=new Quantity{ Amount=50, Unit=Unit.Gram }},
                new IngredientLine{ Name="Ail", Qty=new Quantity{ Amount=4, Unit=Unit.GarlicClove }},
                new IngredientLine{ Name="Herbes de Provence", Qty=new Quantity{ Amount=1, Unit=Unit.Tablespoon }},
                new IngredientLine{ Name="Sel et poivre", Qty=new Quantity{ Amount=1, Unit=Unit.Teaspoon }},
            },
            Steps =
            {
                new InstructionStep{ Order=1, Text="Préchauffer le four à 200°C."},
                new InstructionStep{ Order=2, Text="Enduire le poulet de beurre et assaisonner."},
                new InstructionStep{ Order=3, Text="Rôtir au four pendant 1h30."},
            }
        }
        );
        _recipes.Add(new Recipe
        {
            Title = "Smoothie banane-fraise",
            Description = "Boisson fruitée et énergisante.",
            Servings = 2,
            Tags = new() { "boisson", "fruité" },
            Ingredients =
            {
                new IngredientLine{ Name="Bananes", Qty=new Quantity{ Amount=2, Unit=Unit.Piece }},
                new IngredientLine{ Name="Fraises", Qty=new Quantity{ Amount=150, Unit=Unit.Gram }},
                new IngredientLine{ Name="Yaourt nature", Qty=new Quantity{ Amount=200, Unit=Unit.Milliliter }},
                new IngredientLine{ Name="Miel", Qty=new Quantity{ Amount=1, Unit=Unit.Tablespoon }},
                new IngredientLine{ Name="Glace pilée", Qty=new Quantity{ Amount=100, Unit=Unit.Gram }},
            },
            Steps =
            {
                new InstructionStep{ Order=1, Text="Couper les fruits en morceaux."},
                new InstructionStep{ Order=2, Text="Mixer tous les ingrédients jusqu’à consistance lisse."},
                new InstructionStep{ Order=3, Text="Servir frais."},
            }
        }
        );
        _recipes.Add(new Recipe
        {
            Title = "Omelette aux fines herbes",
            Description = "Omelette légère et savoureuse.",
            Servings = 1,
            Tags = new() { "rapide", "petit-déjeuner" },
            Ingredients =
            {
                new IngredientLine{ Name="Œufs", Qty=new Quantity{ Amount=3, Unit=Unit.Piece }},
                new IngredientLine{ Name="Lait", Qty=new Quantity{ Amount=30, Unit=Unit.Milliliter }},
                new IngredientLine{ Name="Fines herbes (ciboulette, persil)", Qty=new Quantity{ Amount=2, Unit=Unit.Tablespoon }},
                new IngredientLine{ Name="Beurre", Qty=new Quantity{ Amount=10, Unit=Unit.Gram }},
                new IngredientLine{ Name="Sel et poivre", Qty=new Quantity{ Amount=1, Unit=Unit.Teaspoon }},
            },
            Steps =
            {
                new InstructionStep{ Order=1, Text="Battre les œufs avec le lait et les herbes."},
                new InstructionStep{ Order=2, Text="Faire fondre le beurre dans une poêle."},
                new InstructionStep{ Order=3, Text="Cuire l’omelette jusqu’à ce qu’elle soit prise."},
            }
        }
        );
        _recipes.Add(new Recipe
        {
            Title = "Chili con carne",
            Description = "Plat épicé à base de viande et haricots.",
            Servings = 4,
            Tags = new() { "épicé", "mexicain" },
            Ingredients =
            {
                new IngredientLine{ Name="Viande hachée", Qty=new Quantity{ Amount=500, Unit=Unit.Gram }},
                new IngredientLine{ Name="Haricots rouges", Qty=new Quantity{ Amount=400, Unit=Unit.Gram }},
                new IngredientLine{ Name="Tomates concassées", Qty=new Quantity{ Amount=400, Unit=Unit.Gram }},
                new IngredientLine{ Name="Oignon", Qty=new Quantity{ Amount=1, Unit=Unit.Piece }},
                new IngredientLine{ Name="Ail", Qty=new Quantity{ Amount=2, Unit=Unit.GarlicClove }},
                new IngredientLine{ Name="Piment en poudre", Qty=new Quantity{ Amount=1, Unit=Unit.Tablespoon }},
                new IngredientLine{ Name="Cumin", Qty=new Quantity{ Amount=1, Unit=Unit.Teaspoon }},
                new IngredientLine{ Name="Sel et poivre", Qty=new Quantity{ Amount=1, Unit=Unit.Teaspoon }},
            },
            Steps =
            {
                new InstructionStep{ Order=1, Text="Faire revenir l’oignon et l’ail."},
                new InstructionStep{ Order=2, Text="Ajouter la viande et cuire jusqu’à brunissement."},
                new InstructionStep{ Order=3, Text="Incorporer les tomates, haricots et épices; mijoter 30 min."},
            }
        }
        );  
        _recipes.Add(new Recipe
        {
            Title = "Tarte aux pommes",
            Description = "Dessert classique aux pommes.",
            Servings = 6,
            Tags = new() { "dessert", "pâtisserie" },
            Ingredients =
            {
                new IngredientLine{ Name="Pâte brisée", Qty=new Quantity{ Amount=1, Unit=Unit.Piece }},
                new IngredientLine{ Name="Pommes", Qty=new Quantity{ Amount=5, Unit=Unit.Piece }},
                new IngredientLine{ Name="Sucre", Qty=new Quantity{ Amount=100, Unit=Unit.Gram }},
                new IngredientLine{ Name="Beurre", Qty=new Quantity{ Amount=50, Unit=Unit.Gram }},
                new IngredientLine{ Name="Cannelle", Qty=new Quantity{ Amount=1, Unit=Unit.Teaspoon }},
            },
            Steps =
            {
                new InstructionStep{ Order=1, Text="Préchauffer le four à 180°C."},
                new InstructionStep{ Order=2, Text="Étaler la pâte dans un moule."},
                new InstructionStep{ Order=3, Text="Disposer les pommes tranchées, saupoudrer de sucre et cannelle."},
                new InstructionStep{ Order=4, Text="Ajouter des morceaux de beurre sur le dessus."},
                new InstructionStep{ Order=5, Text="Cuire au four pendant 40 minutes."},
            }
        }
        );
    }

    /// <summary>
    /// Récupère toutes les recettes, triées par titre.
    /// </summary>
    /// <returns>Une liste de toutes les recettes.</returns>
    public List<Recipe> GetAll()
    {
        return _recipes.OrderBy(r => r.Title).ToList();
    }
    /// <summary>
    /// Récupère une recette par son identifiant unique.
    /// </summary>
    /// <param name="id">L'identifiant de la recette.</param>
    /// <returns>La recette correspondante ou null si non trouvée.</returns>
    public Recipe? Get(int id) => _recipes.FirstOrDefault(r => r.Id == id);

    /// <summary>
    /// Ajoute une nouvelle recette à la collection.
    /// </summary>
    /// <param name="recipe">La recette à ajouter.</param>
    /// <returns>La recette qui a été ajoutée.</returns>
    public Recipe Add(Recipe recipe)
    {
        _recipes.Add(recipe);
        return recipe;
    }

    /// <summary>
    /// Met à jour une recette existante.
    /// </summary>
    /// <param name="recipe">La recette avec les informations mises à jour.</param>
    /// <returns>True si la mise à jour a réussi, sinon false si la recette n'existe pas.</returns>
    public bool Update(Recipe recipe)
    {
        var existing = _recipes.FirstOrDefault(r => r.Id == recipe.Id);
        if (existing == null) return false;

        // Met à jour les propriétés de la recette existante.
        existing.Title = recipe.Title;
        existing.Description = recipe.Description;
        existing.Servings = recipe.Servings;
        existing.Tags = recipe.Tags;
        existing.Ingredients = recipe.Ingredients;
        existing.Steps = recipe.Steps;
        existing.IsPublished = recipe.IsPublished;
        return true;
    }

    /// <summary>
    /// Supprime une recette par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de la recette à supprimer.</param>
    /// <returns>True si la suppression a réussi, sinon false si la recette n'existe pas.</returns>
    public bool Delete(int id)
    {
        var recipe = _recipes.FirstOrDefault(r => r.Id == id);
        if (recipe == null) return false;

        _recipes.Remove(recipe);
        return true;
    }
}
