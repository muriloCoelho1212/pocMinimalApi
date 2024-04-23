namespace WebApplication1.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}
