using RecipeApp_Eboaguillaume.Models;
using SQLAccess;

namespace RecipeApp_Eboaguillaume.Services
{
    /// <summary>
    /// Service pour gérer les demandes de consultation de recettes.
    /// Ce service est enregistré avec une durée de vie Scoped.
    /// </summary>
    public class MySqlRequestService : IRequestService
    {
        private readonly IMySQLDataAccess _db;
        private readonly string _connString;
        public MySqlRequestService(IMySQLDataAccess db, IConfiguration config)
        {
            _db = db;
            _connString = config.GetConnectionString("MySqlConnection")
                ?? throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");
        }

        /// <inheritdoc />
        public void AddRequest(int recipeId, string user)
        {
            var sql = "INSERT INTO reciperequests (RecipeId, RequestedBy) VALUES (@RecipeId, @RequestedBy);";
            _db.SaveData(sql, new { RecipeId = recipeId, RequestedBy = user}, _connString);
        }

        /// <inheritdoc />
        public List<RecipeRequest> GetAll()
        {
            var sql = "SELECT Id, RecipeId, RequestedBy FROM reciperequests;";
            return _db.LoadData<RecipeRequest, dynamic>(sql, new { }, _connString);
        }

        /// <inheritdoc />
        public int GetCountForRecipe(int recipeId)
        {
            var sql = "SELECT COUNT(Id) FROM reciperequests WHERE RecipeId = @RecipeId;";
            return _db.LoadData<int, dynamic>(sql, new { RecipeId = recipeId }, _connString).FirstOrDefault();
        }
    }
}
