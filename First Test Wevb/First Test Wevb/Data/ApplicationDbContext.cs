using Microsoft.EntityFrameworkCore;
using First_Test_Wevb.Models;

namespace First_Test_Wevb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Members> Members { get; set; } // 確保正確定義 DbSet
    }
}
