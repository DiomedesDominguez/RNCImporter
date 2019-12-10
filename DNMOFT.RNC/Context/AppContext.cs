using Microsoft.EntityFrameworkCore;

namespace DNMOFT.RNC.Context
{
    public class AppContext : DbContext
    {
        public AppContext(DbContextOptions<AppContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<mContribuyente> mContribuyentes { get; set; }
    }
}
