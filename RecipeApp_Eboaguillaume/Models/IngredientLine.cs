using System.ComponentModel.DataAnnotations;

namespace RecipeApp_Eboaguillaume.Models;

/// <summary>
/// Représente une ligne d'ingrédient dans une recette, incluant le nom, la quantité et des notes.
/// </summary>
public class IngredientLine
{
    /// <summary>
    /// L'identifiant unique de la ligne d'ingrédient.
    /// </summary>
    public int Id { get; set; } = 0;

    /// <summary>
    /// Le nom de l'ingrédient. Requis et limité à 100 caractères.
    /// </summary>
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// La quantité de l'ingrédient requise.
    /// </summary>
    public Quantity Qty { get; set; } = new Quantity { Amount = 1, Unit = Unit.Piece };

    /// <summary>
    /// Notes optionnelles sur l'ingrédient (ex: "haché finement"). Limité à 200 caractères.
    /// </summary>
    [MaxLength(200)]
    public string? Notes { get; set; }

    /// <summary>
    /// Crée une copie de la ligne d'ingrédient.
    /// </summary>
    /// <returns>Une nouvelle instance de <see cref="IngredientLine"/> avec les mêmes valeurs.</returns>
    public IngredientLine Clone()
    {
        return new IngredientLine
        {
            Name = this.Name,
            Qty = new Quantity { Amount = this.Qty.Amount, Unit = this.Qty.Unit }
        };
    }
}
