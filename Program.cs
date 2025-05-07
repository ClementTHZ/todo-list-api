using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Data.Sqlite;


var builder = WebApplication.CreateBuilder(args);

/*Add services to the container.
builder.Services.AddOpenApi();
*/

builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(5000));
var app = builder.Build();

/*Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
 {
     app.MapOpenApi();
 }
/app.UseHttpsRedirection();
*/

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
    var tasks = TaskServices.GetAllTask();
    var response = new StringBuilder();
    foreach (DataRow row in tasks.Rows)
    {
        response.AppendLine($"{row["id"]} - {row["title"]} || {row["created_at"]}");
    }
    httpContext.Response.Headers.Append("content-type", "text");
    await httpContext.Response.WriteAsync(response.ToString());
});

app.MapGet("/tasks/{id}", async (httpContext) =>
{
    int id = 0;
    var taskId = httpContext.Request.RouteValues["id"]?.ToString();
    var response = new StringBuilder();
    if (taskId != null) id = Convert.ToInt32(taskId);
    var task = TaskServices.GetTaskById(id);
    if (task == null || task.Rows.Count == 0) response.AppendLine("Task not found !");
    else
    {
        foreach (DataRow row in task.Rows) response.AppendLine($"{row["id"]} - {row["title"]} || {row["created_at"]}");
    }

    httpContext.Response.Headers.Append("content-type", "text");
    await httpContext.Response.WriteAsync(response.ToString());
});

app.Run();

