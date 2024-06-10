using CategoryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CategoryAPI.Data
{
    public class CategoryDb : DbContext
    {
        public CategoryDb(DbContextOptions opt) : base(opt)
        {

        }

        public DbSet<Category> Categories { get; set; }
    }
}
