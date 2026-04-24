
using Microsoft.EntityFrameworkCore;
using DrawingBoardApp.Models;

namespace DrawingBoardApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Board> Boards => Set<Board>();
        public DbSet<BoardElement> BoardElements => Set<BoardElement>();
    }
}
