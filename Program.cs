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
    var body = await httpContext.Request.ReadFromJsonAsync<Task>();
    if (body != null)
    {
        Task task = body;
        TaskServices.CreatedTask(task.Title);
        httpContext.Response.Headers.Append("content-type", "application/text");
        await httpContext.Response.WriteAsync("Task has been created !");
    }
});

app.MapDelete("/tasks/{id}", async (httpContext) =>
{
    int id = 0;
    var response = httpContext.Request.RouteValues["id"]?.ToString();
    if (response != null) id = Convert.ToInt32(response);
    TaskServices.DeleteTask(id);
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

