using RecipeApp_Eboaguillaume.Components.Pages;
using RecipeApp_Eboaguillaume.Models;
using RecipeApp_Eboaguillaume.Services;
using SQLAccess;

namespace RecipeApp_Eboaguillaume.Services
{
    public class MySqlRecipeService : IRecipeService
    {
        private readonly IMySQLDataAccess _db;
        private readonly string _connString;

        public MySqlRecipeService(IMySQLDataAccess db, IConfiguration config)
        {
            _db = db;
            _connString = config.GetConnectionString("MySqlConnection")
                ?? throw new InvalidOperationException("Connection string 'MySqlConnection' not found.");
        }

        public List<Recipe> GetAll()
        {
            try
            {
                var sql = @"SELECT Id, Title, Description, Servings FROM recipes ORDER BY Title;";
                var recipes = _db.LoadData<Recipe, dynamic>(sql, new { }, _connString);
                if (recipes == null || recipes.Count == 0)
                    return new List<Recipe>();

                foreach (var recipe in recipes)
                {
                    PopulateRecipe(recipe);
                }

                return recipes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Recipe>();
            }
        }

        public Recipe? Get(int id)
        {
            var sql = @"SELECT Id, Title, Description, Servings FROM recipes WHERE Id = @Id;";
            var rows = _db.LoadData<Recipe, dynamic>(sql, new { Id = id }, _connString);
            var recipe = rows.FirstOrDefault();
            if (recipe == null)
                return null;

            PopulateRecipe(recipe);
            return recipe;
        }

        public Recipe Add(Recipe recipe)
        {
            // 1. Insert main recipe
            string insertRecipeSql = @"
                INSERT INTO recipes (Title, Description, Servings, CreatedUtc)
                VALUES (@Title, @Description, @Servings, @CreatedUtc);
                SELECT LAST_INSERT_ID();
            ";
            int newId = _db.LoadData<int, dynamic>(
                insertRecipeSql,
                new
                {
                    Title = recipe.Title,
                    Description = recipe.Description,
                    Servings = recipe.Servings,
                    CreatedUtc = DateTime.UtcNow
                },
                _connString
            ).First();
            recipe.Id = newId;

            // 2. Insert steps
            string insertStepSql = @"INSERT INTO steps (RecipeId, StepOrder, StepText) VALUES (@RecipeId, @StepOrder, @StepText);";
            foreach (var step in recipe.Steps.OrderBy(s => s.Order))
            {
                _db.SaveData(
                    insertStepSql,
                    new { RecipeId = recipe.Id, StepOrder = step.Order, StepText = step.Text },
                    _connString
                );
            }

            // 3. Insert ingredients
            string insertIngredientSql = @"INSERT INTO ingredients (RecipeId, Name, Amount, Unit) VALUES (@RecipeId, @Name, @Amount, @Unit);";
            foreach (var ing in recipe.Ingredients)
            {
                _db.SaveData(
                    insertIngredientSql,
                    new
                    {
                        RecipeId = recipe.Id,
                        Name = ing.Name,
                        Amount = ing.Qty.Amount,
                        Unit = ing.Qty.Unit.ToString()
                    },
                    _connString
                );
            }

            // 4. Insert tags
            string selectTagIdSql = @"SELECT Id FROM tags WHERE Name = @Name;";
            string insertTagSql = @"INSERT INTO tags (Name) VALUES (@Name); SELECT LAST_INSERT_ID();";
            string insertRecipeTagSql = @"INSERT INTO recipetags (RecipeId, TagId) VALUES (@RecipeId, @TagId);";

            foreach (var tagName in recipe.Tags.Distinct())
            {
                var existingIds = _db.LoadData<int, dynamic>(selectTagIdSql, new { Name = tagName }, _connString);
                int tagId = existingIds.Count > 0 ? existingIds.First() :
                    _db.LoadData<int, dynamic>(insertTagSql, new { Name = tagName }, _connString).First();

                _db.SaveData(insertRecipeTagSql, new { RecipeId = recipe.Id, TagId = tagId }, _connString);
            }

            return recipe;
        }

        public bool Update(Recipe recipe)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));

            recipe.Tags ??= new List<string>();
            recipe.Ingredients ??= new List<IngredientLine>();
            recipe.Steps ??= new List<InstructionStep>();

            // Update main recipe
            string sqlUpdateRecipe = @"UPDATE recipes SET Title = @Title, Description = @Description, Servings = @Servings WHERE Id = @Id;";
            _db.SaveData(sqlUpdateRecipe, new { Id = recipe.Id, Title = recipe.Title, Description = recipe.Description, Servings = recipe.Servings }, _connString);

            // Ingredients management
            string sqlGetIngredientIds = @"SELECT Id FROM ingredients WHERE RecipeId = @RecipeId;";
            var dbIngredientIds = _db.LoadData<int, dynamic>(sqlGetIngredientIds, new { RecipeId = recipe.Id }, _connString);
            var clientIngredientIds = recipe.Ingredients.Where(i => i.Id > 0).Select(i => i.Id).ToList();
            var ingredientIdsToDelete = dbIngredientIds.Except(clientIngredientIds).ToList();

            string sqlDeleteIngredient = @"DELETE FROM ingredients WHERE Id = @Id;";
            foreach (var delId in ingredientIdsToDelete)
            {
                _db.SaveData(sqlDeleteIngredient, new { Id = delId }, _connString);
            }

            string sqlUpdateIngredient = @"UPDATE ingredients SET Name = @Name, Amount = @Amount, Unit = @Unit WHERE Id = @Id AND RecipeId = @RecipeId;";
            string sqlInsertIngredient = @"INSERT INTO ingredients (RecipeId, Name, Amount, Unit) VALUES (@RecipeId, @Name, @Amount, @Unit);";

            foreach (var ing in recipe.Ingredients)
            {
                if (ing.Id > 0 && dbIngredientIds.Contains(ing.Id))
                {
                    _db.SaveData(sqlUpdateIngredient, new
                    {
                        Id = ing.Id,
                        RecipeId = recipe.Id,
                        Name = ing.Name,
                        Amount = ing.Qty.Amount,
                        Unit = ing.Qty.Unit.ToString()
                    }, _connString);
                }
                else
                {
                    _db.SaveData(sqlInsertIngredient, new
                    {
                        RecipeId = recipe.Id,
                        Name = ing.Name,
                        Amount = ing.Qty.Amount,
                        Unit = ing.Qty.Unit.ToString()
                    }, _connString);
                }
            }

            // Steps management
            string sqlGetStepOrders = @"SELECT StepOrder FROM steps WHERE RecipeId = @RecipeId;";
            var dbStepOrders = _db.LoadData<int, dynamic>(sqlGetStepOrders, new { RecipeId = recipe.Id }, _connString);
            var clientStepOrders = recipe.Steps.Select(s => s.Order).Distinct().ToList();
            var stepOrdersToDelete = dbStepOrders.Except(clientStepOrders).ToList();

            string sqlDeleteStep = @"DELETE FROM steps WHERE RecipeId = @RecipeId AND StepOrder = @StepOrder;";
            foreach (var ord in stepOrdersToDelete)
            {
                _db.SaveData(sqlDeleteStep, new { RecipeId = recipe.Id, StepOrder = ord }, _connString);
            }

            string sqlUpdateStep = @"UPDATE steps SET StepText = @StepText WHERE RecipeId = @RecipeId AND StepOrder = @StepOrder;";
            string sqlInsertStep = @"INSERT INTO steps (RecipeId, StepOrder, StepText) VALUES (@RecipeId, @StepOrder, @StepText);";

            foreach (var step in recipe.Steps.OrderBy(s => s.Order))
            {
                if (dbStepOrders.Contains(step.Order))
                {
                    _db.SaveData(sqlUpdateStep, new { RecipeId = recipe.Id, StepOrder = step.Order, StepText = step.Text }, _connString);
                }
                else
                {
                    _db.SaveData(sqlInsertStep, new { RecipeId = recipe.Id, StepOrder = step.Order, StepText = step.Text }, _connString);
                }
            }

            // Tags management
            string sqlGetExistingTags = @"SELECT t.Name FROM recipetags rt INNER JOIN tags t ON t.Id = rt.TagId WHERE rt.RecipeId = @RecipeId;";
            var dbTagNames = _db.LoadData<string, dynamic>(sqlGetExistingTags, new { RecipeId = recipe.Id }, _connString) ?? new List<string>();
            var newTagNames = recipe.Tags.Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            var tagsToAdd = newTagNames.Except(dbTagNames, StringComparer.OrdinalIgnoreCase).ToList();
            var tagsToRemove = dbTagNames.Except(newTagNames, StringComparer.OrdinalIgnoreCase).ToList();

            string sqlSelectTagId = @"SELECT Id FROM tags WHERE Name = @Name;";
            string sqlInsertTag = @"INSERT INTO tags (Name) VALUES (@Name); SELECT LAST_INSERT_ID();";
            string sqlInsertRecipeTag = @"INSERT INTO recipetags (RecipeId, TagId) VALUES (@RecipeId, @TagId);";
            string sqlDeleteRecipeTag = @"DELETE rt FROM recipetags rt INNER JOIN tags t ON t.Id = rt.TagId WHERE rt.RecipeId = @RecipeId AND t.Name = @Name;";

            foreach (var tagName in tagsToRemove)
            {
                _db.SaveData(sqlDeleteRecipeTag, new { RecipeId = recipe.Id, Name = tagName }, _connString);
            }

            foreach (var tagName in tagsToAdd)
            {
                var tagIds = _db.LoadData<int, dynamic>(sqlSelectTagId, new { Name = tagName }, _connString);
                int tagId = tagIds.Count > 0 ? tagIds.First() : _db.LoadData<int, dynamic>(sqlInsertTag, new { Name = tagName }, _connString).First();

                _db.SaveData(sqlInsertRecipeTag, new { RecipeId = recipe.Id, TagId = tagId }, _connString);
            }

            return true;
        }

        public bool Delete(int id)
        {
            var sql = @"DELETE FROM recipes WHERE Id = @Id;";
            _db.SaveData(sql, new { Id = id }, _connString);
            return true;
        }

        private void PopulateRecipe(Recipe recipe)
        {
            string sqlTags = @"SELECT t.Name FROM recipetags rt INNER JOIN tags t ON t.Id = rt.TagId WHERE rt.RecipeId = @Id;";
            string sqlIngredients = @"SELECT Id, Name, Amount, Unit FROM ingredients WHERE RecipeId = @Id;";
            string sqlSteps = @"SELECT StepOrder, StepText FROM steps WHERE RecipeId = @Id ORDER BY StepOrder;";

            recipe.Tags = _db.LoadData<string, dynamic>(sqlTags, new { Id = recipe.Id }, _connString) ?? new List<string>();

            var ingredientRows = _db.LoadData<dynamic, dynamic>(sqlIngredients, new { Id = recipe.Id }, _connString) ?? new List<dynamic>();
            recipe.Ingredients = ingredientRows.Select(i => new IngredientLine
            {
                Id = (int)i.Id,
                Name = (string)i.Name,
                Qty = new Quantity
                {
                    Amount = (decimal)i.Amount,
                    Unit = Enum.TryParse<Unit>((string)i.Unit, out var unitEnum) ? unitEnum : Unit.Gram
                }
            }).ToList();

            var stepRows = _db.LoadData<dynamic, dynamic>(sqlSteps, new { Id = recipe.Id }, _connString) ?? new List<dynamic>();
            recipe.Steps = stepRows.Select(s => new InstructionStep
            {
                Order = (int)s.StepOrder,
                Text = (string)s.StepText
            }).ToList();
        }
    }
}
