using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MyDpprProject.Models
{
    public class Context
    {
        public static string connectionString =
            @"Server=(localdb)\MSSQLLocalDB;Database=RealEstateDb;Integrated Security=true;";

        // Insert - Update - Delete
        public static void ExecuteReturn(string procedureName, DynamicParameters parameters = null)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                db.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        // Listeleme
        public static IEnumerable<T> List<T>(string procedureName, DynamicParameters parameters = null)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                db.Open();
                return db.Query<T>(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}   