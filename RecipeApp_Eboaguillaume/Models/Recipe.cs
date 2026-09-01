﻿using System.ComponentModel.DataAnnotations;

namespace RecipeApp_Eboaguillaume.Models;

/// <summary>
/// Représente une recette de cuisine complète.
/// </summary>
public class Recipe
{
    /// <summary>
    /// L'identifiant unique de la recette.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Le titre de la recette. Requis et limité à 120 caractères.
    /// </summary>
    [Required, MaxLength(120)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Une courte description de la recette. Limitée à 300 caractères.
    /// </summary>
    [MaxLength(300)]
    public string? Description { get; set; }

    /// <summary>
    /// Le nombre de portions que la recette produit. Doit être entre 1 et 48.
    /// </summary>
    [Range(1, 48)]
    public int Servings { get; set; } = 4;

    /// <summary>
    /// Une liste de tags pour catégoriser la recette (ex: "italien", "rapide").
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// La liste des ingrédients nécessaires pour la recette.
    /// </summary>
    public List<IngredientLine> Ingredients { get; set; } = new();

    /// <summary>
    /// Les étapes de préparation de la recette.
    /// </summary>
    public List<InstructionStep> Steps { get; set; } = new();

    /// <summary>
    /// La date et l'heure de création de la recette en UTC.
    /// </summary>
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indique si la recette est publiée et visible pour les utilisateurs.
    /// </summary>
    public bool IsPublished { get; set; } = true;

    /// <summary>
    /// Crée une copie profonde de la recette.
    /// La nouvelle recette aura un nouvel Id et un titre préfixé par "Copy of ".
    /// </summary>
    /// <returns>Une nouvelle instance de <see cref="Recipe"/> qui est une copie de l'original.</returns>
    public Recipe Clone()
    {
        return new Recipe
        {
            Id = 0, // Important: Un nouvel ID pour la copie
            Title = $"Copy of {this.Title}",
            Description = this.Description,
            Servings = this.Servings,
            Tags = new List<string>(this.Tags),
            Ingredients = this.Ingredients.Select(i => i.Clone()).ToList(),
            Steps = this.Steps.Select(s => s.Clone()).ToList(),
            IsPublished = this.IsPublished
        };
    }


}
