using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PersonalBlog.Web.Models;

namespace PersonalBlog.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Article> Articles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Article>()
                .Property(a => a.PublishedAt)
                .HasDefaultValueSql("GETDATE()") // Для SQL Server
                .IsRequired(); // указываем, что поле обязательно
        }
    }
}
