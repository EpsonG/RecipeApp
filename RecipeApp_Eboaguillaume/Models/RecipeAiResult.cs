namespace RecipeApp_Eboaguillaume.Models
{
    public class RecipeAiResult
    {
        public List<string> Steps { get; set; } = new();
        public List<string> SuggestedTags { get; set; } = new();
        public List<string> ShoppingList { get; set; } = new();
        public string Description { get; set; } = string.Empty;
    }
}
