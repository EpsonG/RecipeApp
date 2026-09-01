using System.Data;
using MySql.Data.MySqlClient;
using Dapper;

namespace SQLAccess
{
    
    public class MySQLDataAccess : IMySQLDataAccess
    {
        public List<T> LoadData<T, U>(string sql, U parameters, string connectionString)
        {
            using (IDbConnection conn = new MySqlConnection(connectionString))
            {
                var rows = conn.Query<T>(sql, parameters);
                return rows.ToList();
            }
        }
        public void SaveData<T>(string sql, T parameters, string connectionString)
        {
            using (IDbConnection conn = new MySqlConnection(connectionString))
            {
                conn.Execute(sql, parameters);
            }
        }
    }
}
