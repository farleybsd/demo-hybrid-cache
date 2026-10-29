using hybrid_cache.Api.Database.Seeder;
using hybrid_cache.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace hybrid_cache.Api.Database.Context
{
    internal sealed class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options, IHostEnvironment env) : base(options)
        {
            if (env.IsDevelopment())
                Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>()
                .HasOne<Category>().WithMany()
                .HasForeignKey(a => a.CategoryId);

            modelBuilder.Seed();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
