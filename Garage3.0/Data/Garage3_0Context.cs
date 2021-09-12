using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Garage3._0.Data
{
    public class Garage3_0Context : DbContext
    {
        public Garage3_0Context(DbContextOptions<Garage3_0Context> options)
            : base(options)
        {
        }

        public DbSet<Garage3._0.Models.Vehicle> Vehicle { get; set; }
        public DbSet<Garage3._0.Models.Member> Member { get; set; }
        public DbSet<Garage3._0.Models.Parked> Parked { get; set; }
        public DbSet<Garage3._0.Models.ParkingSpot> ParkingSpot { get; set; }
        public DbSet<Garage3._0.Models.VehicleType> VehicleType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}