using System.ComponentModel.DataAnnotations;
using RecipeApp_Eboaguillaume.Models;

namespace RecipeApp_Eboaguillaume.Models;

/// <summary>
/// Représente une quantité, composée d'une valeur numérique et d'une unité de mesure.
/// </summary>
public class Quantity
{
    /// <summary>
    /// La valeur numérique de la quantité (ex: 100, 1.5).
    /// </summary>
    [Range(0.0, 10000)]
    public decimal Amount { get; set; }
    
    /// <summary>
    /// L'unité de mesure (ex: Gram, Liter, Piece).
    /// </summary>
    [Required]
    public Unit Unit { get; set; } = Unit.Piece;
}
