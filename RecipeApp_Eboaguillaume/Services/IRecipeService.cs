using RecipeApp_Eboaguillaume.Models;

namespace RecipeApp_Eboaguillaume.Services;

/// <summary>
/// Définit les opérations pour la gestion des recettes.
/// </summary>
public interface IRecipeService
{
    /// <summary>
    /// Récupère toutes les recettes.
    /// </summary>
    /// <returns>Une liste de toutes les recettes.</returns>
    List<Recipe> GetAll();
    /// <summary>
    /// Récupère une recette par son identifiant unique.
    /// </summary>
    /// <param name="id">L'identifiant de la recette.</param>
    /// <returns>La recette correspondante ou null si non trouvée.</returns>
    Recipe? Get(int id);
    /// <summary>
    /// Ajoute une nouvelle recette.
    /// </summary>
    /// <param name="recipe">La recette à ajouter.</param>
    /// <returns>La recette ajoutée.</returns>
    Recipe Add(Recipe recipe);
    /// <summary>
    /// Met à jour une recette existante.
    /// </summary>
    /// <param name="recipe">La recette avec les informations mises à jour.</param>
    /// <returns>True si la mise à jour a réussi, sinon false.</returns>
    bool Update(Recipe recipe);
    /// <summary>
    /// Supprime une recette par son identifiant.
    /// </summary>
    /// <param name="id">L'identifiant de la recette à supprimer.</param>
    /// <returns>True si la suppression a réussi, sinon false.</returns>
    bool Delete(int id);
}
