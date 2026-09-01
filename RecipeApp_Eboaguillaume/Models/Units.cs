namespace RecipeApp_Eboaguillaume.Models;

/// <summary>
/// Énumération des unités de mesure couramment utilisées en cuisine.
/// </summary>
public enum Unit
{
    /// <summary>
    /// Unité non spécifiée ou non applicable.
    /// </summary>
    None = 0,
    /// <summary>
    /// Unité de masse : gramme.
    /// </summary>
    Gram,
    /// <summary>
    /// Unité de masse : kilogramme.
    /// </summary>
    Kilogram,
    /// <summary>
    /// Unité de volume : millilitre.
    /// </summary>
    Milliliter,
    /// <summary>
    /// Unité de volume : litre.
    /// </summary>
    Liter,
    /// <summary>
    /// Unité de volume : cuillère à café.
    /// </summary>
    Teaspoon,
    /// <summary>
    /// Unité de volume : cuillère à soupe.
    /// </summary>
    Tablespoon,
    /// <summary>
    /// Unité de volume : tasse.
    /// </summary>
    Cup,
    /// <summary>
    /// Unité de comptage : pièce, unité.
    /// </summary>
    Piece,
    /// <summary>
    /// Unité de comptage : gousse d'ail.
    /// </summary>
    GarlicClove
}
