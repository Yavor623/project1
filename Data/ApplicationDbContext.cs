using AccountManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Train> Trains { get; set; }
        public DbSet<TrainStation> TrainStations { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<TypeOfTrain> TypeOfTrains { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Schedule>()
                .HasOne(o => o.TrainStation)
                .WithMany(o => o.Schedules)
                .HasForeignKey(o => o.TrainStationId);
            builder.Entity<Schedule>()
                .HasOne(o => o.Train)
                .WithOne(o => o.Schedule)
                .HasForeignKey<Schedule>(o=> o.TrainId)
                .HasPrincipalKey<Train>(o=> o.Id);
            builder.Entity<Train>()
                .HasOne(o => o.TypeOfTrain)
                .WithMany(o => o.Trains)
                .HasForeignKey(o => o.TypeOfTrainId);

            builder.Entity<TypeOfTrain>().HasData(
                new TypeOfTrain { Id = 1, Name = "Скоростен" },
                new TypeOfTrain { Id = 2, Name = "Пътнически" },
                new TypeOfTrain { Id = 3, Name = "Градски" },
                new TypeOfTrain { Id = 4, Name = "Извънградски" }
                );
        }
    }
}
