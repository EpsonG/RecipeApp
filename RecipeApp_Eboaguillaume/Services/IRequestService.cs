using RecipeApp_Eboaguillaume.Models;

namespace RecipeApp_Eboaguillaume.Services;

/// <summary>
/// Définit les opérations pour le suivi des demandes de consultation de recettes.
/// </summary>
public interface IRequestService
{
    /// <summary>
    /// Enregistre une nouvelle demande pour une recette spécifique.
    /// </summary>
    /// <param name="recipeId">L'identifiant de la recette consultée.</param>
    /// <param name="user">L'identifiant de l'utilisateur qui fait la demande.</param>
    void AddRequest(int recipeId, string user);
    /// <summary>
    /// Récupère la liste de toutes les demandes enregistrées.
    /// </summary>
    /// <returns>Une liste d'objets <see cref="RecipeRequest"/>.</returns>
    List<RecipeRequest> GetAll();
    /// <summary>
    /// Compte le nombre de demandes pour une recette spécifique.
    /// </summary>
    /// <param name="recipeId">L'identifiant de la recette.</param>
    /// <returns>Le nombre total de demandes pour cette recette.</returns>
    int GetCountForRecipe(int recipeId);

}