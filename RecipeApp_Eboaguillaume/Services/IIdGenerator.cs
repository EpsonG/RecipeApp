namespace RecipeApp_Eboaguillaume.Services;
/// <summary>
/// Définit un générateur d'identifiants uniques.
/// </summary>
public interface IIdGenerator
{
    /// <summary>
    /// Génère un nouvel identifiant unique.
    /// </summary>
    /// <returns>Un nouveau <see cref="int"/>.</returns>
    int NewId();
}
/// <summary>
/// Implémentation de <see cref="IIdGenerator"/> qui utilise <see cref="int.NewGuid()"/>.
/// </summary>
public class intIdGenerator : IIdGenerator
{
    public int NewId() => 0;
}
