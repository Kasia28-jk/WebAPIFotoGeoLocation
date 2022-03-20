using FotoGeoLocationWebApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace FotoGeoLocationWebApplication.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        //    public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>().ToTable("Picture");
        }
    }
}
