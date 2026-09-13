using TaskBoard.Application;
using TaskBoard.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddSingleton<ITodoService, TodoService>();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.UseExceptionHandler();
app.UseCors();

var todos = app.MapGroup("/api/todos");

todos.MapGet("/", async (ITodoService service, CancellationToken cancellationToken) =>
    Results.Ok(await service.ListAsync(cancellationToken)));

todos.MapPost("/", async (CreateTodoRequest request, ITodoService service, CancellationToken cancellationToken) =>
{
    try
    {
        var item = await service.CreateAsync(request.Title, cancellationToken);
        return Results.Created($"/api/todos/{item.Id}", item);
    }
    catch (ArgumentException exception)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]> { ["title"] = [exception.Message] });
    }
});

todos.MapPut("/{id:guid}", async (Guid id, RenameTodoRequest request, ITodoService service, CancellationToken cancellationToken) =>
{
    try
    {
        var item = await service.RenameAsync(id, request.Title, cancellationToken);
        return item is null ? Results.NotFound() : Results.Ok(item);
    }
    catch (ArgumentException exception)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]> { ["title"] = [exception.Message] });
    }
});

todos.MapPatch("/{id:guid}/toggle", async (Guid id, ITodoService service, CancellationToken cancellationToken) =>
{
    var item = await service.ToggleAsync(id, cancellationToken);
    return item is null ? Results.NotFound() : Results.Ok(item);
});

todos.MapDelete("/{id:guid}", async (Guid id, ITodoService service, CancellationToken cancellationToken) =>
    await service.DeleteAsync(id, cancellationToken) ? Results.NoContent() : Results.NotFound());

app.MapHealthChecks("/health");
app.Run();

public sealed record CreateTodoRequest(string Title);
public sealed record RenameTodoRequest(string Title);

