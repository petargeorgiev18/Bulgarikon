using Bulgarikon.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulgarikon.Data
{
    public class BulgarikonDbContext : DbContext
    {
        public BulgarikonDbContext(DbContextOptions<BulgarikonDbContext> options)
        : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Civilization> Civilizations { get; set; }
        public DbSet<Figure> Figures { get; set; }
        public DbSet<Era> Eras { get; set; }
    }
}
