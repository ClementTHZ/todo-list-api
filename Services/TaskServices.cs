using Microsoft.Data.Sqlite;
public class TaskServices
{
    public static void CreatedTask(string title)
    {
        ExecuteNonQuery("INSERT INTO tasks (title) VALUES (@title)", new Dictionary<string, object> { ["@title"] = title });
    }
    public static void DeleteTask(int id)
    {
        ExecuteNonQuery("DELETE FROM tasks WHERE id=@id", new Dictionary<string, object> { { "@id", id } });
    }
    public static void GetAllTask()
    {

    }
    public static void GetTaskById()
    {

    }

    private static void ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
    {
        var dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\SQLITE-TodoList\BDD\todolist.db";
        using (var dbConnection = new SqliteConnection(dbfile))
        {
            dbConnection.Open();
            using (var commande = dbConnection.CreateCommand())
            {
                commande.CommandText = sql;
                foreach (var parameter in parameters) commande.Parameters.AddWithValue(parameter.Key, parameter.Value);
                commande.ExecuteNonQuery();
            }
        }
    }

    private static void GetDataTable(string sql, Dictionary<string, object> parameters = null)
    {
    }
}