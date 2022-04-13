using FotoGeoLocationWebApplication.Entities;
using Microsoft.EntityFrameworkCore;

namespace FotoGeoLocationWebApplication.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {}

        public DbSet<User> Users { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>().ToTable("Picture");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Session>().ToTable("Sessions");
        }
    }
}
