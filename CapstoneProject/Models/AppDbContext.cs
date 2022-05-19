using Microsoft.EntityFrameworkCore;

namespace CapstoneProject.Models {
    public class AppDbContext : DbContext {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Vendor> Vendors{ get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Request> Requests{ get; set; }
        public virtual DbSet<RequestLine> RequestLines { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) { }
    }
}
