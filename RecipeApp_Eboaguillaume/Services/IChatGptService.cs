using System.Net.Http.Headers;
using System.Text.Json;
using RecipeApp_Eboaguillaume.Models;

namespace RecipeApp_Eboaguillaume.Services;
/// <summary>
/// Définit un générateur d'identifiants uniques.
/// </summary>
public interface IChatGptService
{
    // Tache asynchrone pour recuperer les resultats de l'IA pour une recette donnée
    Task<RecipeAiResult> GenerateForRecipeAsync(Recipe recipe, CancellationToken ct = default);
    
}
