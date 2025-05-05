var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return "Hello Home Page";
});

app.MapGet("/tasks", () =>
{
    return "Hello Tasks";
});

app.MapGet("/tasks/id", () =>
{
    return "Hello Task";
});

app.Run();

