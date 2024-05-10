using WebApplication1.Controllers;

namespace WebApplication1.Routes
{
    public static class TodoEndpoints
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/todos", TodoControllers.GetAllTodos);
            app.MapGet("/todos/complete", TodoControllers.GetCompleteTodos);
            app.MapGet("/todos/{id:int}", TodoControllers.GetTodo);
            app.MapPost("/todos", TodoControllers.CreateTodo);
            app.MapPut("/todos/{id:int}", TodoControllers.UpdateTodo);
            app.MapDelete("/todos/{id:int}", TodoControllers.DeleteTodo);
        }
    }
}

