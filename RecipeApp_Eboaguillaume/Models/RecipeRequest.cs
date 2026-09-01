namespace RecipeApp_Eboaguillaume.Models
{
    public class RecipeRequest
    {
        public int Id { get; set; } = 0;
        public int RecipeId { get; set; }
        public string? RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; } = DateTime.UtcNow;
    }
}
