using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InventoryAPI.Persistence
{
    public class InventoryDbContext : DbContext
    {
        public IConfiguration Configuration { get; }

        public InventoryDbContext() { }

        public InventoryDbContext(DbContextOptions<InventoryDbContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }

        public virtual DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Configuration.GetConnectionString("DbConnection"));
        }
    }
}
