using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<TodoDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build();

var todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/{id:int}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id:int}", UpdateTodo);
todoItems.MapDelete("/{id:int}", DeleteTodo);

app.Run();
return;

static async Task<IResult> GetAllTodos(TodoDbContext db)
{
    return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDto(x)).ToArrayAsync());
}

static async Task<IResult> GetCompleteTodos(TodoDbContext db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDto(x)).ToListAsync());
}

static async Task<IResult> GetTodo(int id, TodoDbContext db)
{
    return await db.Todos.FindAsync(id)
        is Todo todo
            ? TypedResults.Ok(new TodoItemDto(todo))
            : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(TodoItemDto todoItemDto, TodoDbContext db)
{
    var todoItem = new Todo
    {
        Name = todoItemDto.Name,
        IsComplete = todoItemDto.IsComplete,
    };

    db.Todos.Add(todoItem);
    await db.SaveChangesAsync();

    todoItemDto = new TodoItemDto(todoItem);

    return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItemDto);
}

static async Task<IResult> UpdateTodo(int id, TodoItemDto todoItemDto, TodoDbContext db)
{
    var todo = await db.Todos.FindAsync(id);
    
    if (todo is null) return TypedResults.NotFound();

    todo.Name = todoItemDto.Name;
    todo.IsComplete = todoItemDto.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id, TodoDbContext db)
{
    if (await db.Todos.FindAsync(id) is Todo todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound();
}
