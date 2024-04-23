using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Todo>().HasData(
                new object[]
                {
                    new { Id = 1, Name = "Teste", IsComplete = true },
                    new { Id = 2, Name = "Teste", IsComplete = true }
                }
            );

            base.OnModelCreating(builder);
        }
        public DbSet<Todo> Todos { get; set; }
    }
}
