using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddOpenApi();

builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(5000));

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

//app.UseHttpsRedirection();

app.MapPost("/tasks/new", async (httpContext) =>
{
    Task task = await httpContext.Request.ReadFromJsonAsync<Task>();

    var dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\SQLITE-TodoList\BDD\todolist.db";
    using (var db = new SqliteConnection(dbfile))
    {
        db.Open();
        using (var commande = db.CreateCommand())
        {
            commande.CommandText = $"INSERT INTO task (title) VALUES ('{task.title}')";
            commande.ExecuteNonQuery();
        }
    }

    httpContext.Response.Headers.Append("content-type", "application/text");
    await httpContext.Response.WriteAsync("Task has been created !");
});

app.MapDelete("/tasks/{id}", async (httpContext) =>
{
    string id = httpContext.Request.RouteValues["id"]?.ToString();
    var dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\SQLITE-TodoList\BDD\todolist.db";
    using (var db = new SqliteConnection(dbfile))
    {
        db.Open();
        using (var commande = db.CreateCommand())
        {
            commande.CommandText = "DELETE FROM task WHERE id=@id";
            commande.Parameters.AddWithValue("@id", id);
            commande.ExecuteNonQuery();
        }
    }
    httpContext.Response.Headers.Append("content-type", "application/text");
    await httpContext.Response.WriteAsync("Task has been deleted !");
});

app.MapGet("/tasks", async (httpContext) =>
{
    var dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\SQLITE-TodoList\BDD\todolist.db";
    using (var db = new SqliteConnection(dbfile))
    {
        db.Open();
        using (var commande = db.CreateCommand())
        {


            //httpContext.Request.Path["id"]

            //var id = httpContext.Request.Query["id"].ToString();
            // commande.CommandText = $"SELECT * FROM task WHERE id={id}";
            // using (var reader = commande.ExecuteReader())
            // {
            //     var response = new StringBuilder();
            //     while (reader.Read())
            //     {
            //         response.AppendLine(reader["title"].ToString());
            //     }

            //     httpContext.Response.Headers.Append("content-type", "text");
            //     await httpContext.Response.WriteAsync(response.ToString());
            // }
        }
    }
});

//app.MapGet("/tasks/id", () => { return TaskServices.Getall(); });

app.Run();

