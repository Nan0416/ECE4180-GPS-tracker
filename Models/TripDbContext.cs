
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace ece4180.gpstracker.Models{
    public class TripContext : DbContext
    {
        /* public TripContext(){

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=trips.db");
        }*/
        public TripContext(DbContextOptions<TripContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasKey(loc => new {
                    loc.tripId, 
                    loc.timeStamp
                });
        }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Trip> Trips { get; set; }
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=trips.db");
        }*/
    }
}