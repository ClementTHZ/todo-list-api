using Microsoft.Data.Sqlite;
using System.Data;
using System.Text;
public class TaskServices
{
    public static void CreatedTask(string title)
    {
        ExecuteNonQuery("INSERT INTO tasks (title, created_at, is_done) VALUES (@title, @created_at, @is_done)", new Dictionary<string, object> { ["@title"] = title, ["@created_at"] = DateTime.Now, ["@is_done"] = 0 });
    }
    public static void DeleteTask(int id)
    {
        ExecuteNonQuery("DELETE FROM tasks WHERE id=@id", new Dictionary<string, object> { { "@id", id } });
    }
    public static DataTable GetAllTask()
    {
        var tasks = GetDataTable("SELECT * FROM tasks");
        return tasks;
    }
    public static DataTable GetTaskById(int id) // TODO Finir la méthode
    {
        var task = GetDataTable("SELECT * FROM tasks WHERE id = @id", new Dictionary<string, object> { { "@id", id } });
        foreach (DataRow row in task.Rows)
        {
        }
        return task;
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

    private static DataTable GetDataTable(string sql, Dictionary<string, object> parameters = null)
    {
        var dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\SQLITE-TodoList\BDD\todolist.db";
        using (var dbConnection = new SqliteConnection(dbfile))
        {
            dbConnection.Open();
            using (var commande = dbConnection.CreateCommand())
            {
                commande.CommandText = sql;
                if (parameters != null)
                {
                    foreach (var parameter in parameters) commande.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                using (var reader = commande.ExecuteReader())
                {
                    var response = new StringBuilder();
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }
    }
}