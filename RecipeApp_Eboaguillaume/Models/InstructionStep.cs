using System.ComponentModel.DataAnnotations;

namespace RecipeApp_Eboaguillaume.Models;

/// <summary>
/// Représente une étape d'instruction dans une recette.
/// </summary>
public class InstructionStep
{
    /// <summary>
    /// Le numéro d'ordre de l'étape dans la recette.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Le texte décrivant l'instruction. Requis et limité à 500 caractères.
    /// </summary>
    [Required, MaxLength(500)]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Crée une copie de l'étape d'instruction.
    /// </summary>
    /// <returns>Une nouvelle instance de <see cref="InstructionStep"/> avec les mêmes valeurs.</returns>
    public InstructionStep Clone()
    {
        return new InstructionStep
        {
            Order = this.Order,
            Text = this.Text
        };
    }
}
