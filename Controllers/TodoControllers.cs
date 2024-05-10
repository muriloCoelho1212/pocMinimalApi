using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Entities;

namespace WebApplication1.Controllers
{
    public class TodoControllers
    {
        public static async Task<IResult> GetAllTodos(TodoDbContext db)
        {
            return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDto(x)).ToArrayAsync());
        }

        public static async Task<IResult> GetCompleteTodos(TodoDbContext db)
        {
            return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDto(x)).ToListAsync());
        }

        public static async Task<IResult> GetTodo(int id, TodoDbContext db)
        {
            return await db.Todos.FindAsync(id)
                is Todo todo
                    ? TypedResults.Ok(new TodoItemDto(todo))
                    : TypedResults.NotFound();
        }

        public static async Task<IResult> CreateTodo(TodoItemDto todoItemDto, TodoDbContext db)
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

        public static async Task<IResult> UpdateTodo(int id, TodoItemDto todoItemDto, TodoDbContext db)
        {
            var todo = await db.Todos.FindAsync(id);

            if (todo is null) return TypedResults.NotFound();

            todo.Name = todoItemDto.Name;
            todo.IsComplete = todoItemDto.IsComplete;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        public static async Task<IResult> DeleteTodo(int id, TodoDbContext db)
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            return TypedResults.NotFound();
        }
    }
}
