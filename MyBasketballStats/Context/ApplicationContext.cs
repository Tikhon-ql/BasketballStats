using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyBasketballStats.Models;
using MyBasketballStats.Models.Identity;
using MyBasketballStats.Models.ManyToMany;
using System;
using System.Collections.Generic;

namespace MyBasketballStats.Context
{
    public class ApplicationContext : IdentityDbContext<User, Role, string>
    {
        public DbSet<Person> People { get; set; }
        public DbSet<BasketballGamePerfomance> Perfomances { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Geoposition> Geopositions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Friend>().HasKey(f => new { f.UserId, f.FriendId });
            //modelBuilder.Entity<Friend>()
            //    .HasOne(fr => fr.Person)
            //    .WithMany(p => p.Friends)
            //    .HasForeignKey(fr => fr.FriendId);
            //modelBuilder.Entity<Friend>()
            //    .HasOne(fr => fr.Person)
            //    .WithMany()


            List<Training> trainings = new List<Training>();
            trainings.Add(new Training
            {
                Id = Guid.NewGuid().ToString(),
                Title = "САМЫЕ ЭФФЕКТИВНЫЕ УПРАЖНЕНИЯ ДЛЯ ТРЕНИРОВКИ ДРИБЛИНГА В БАСКЕТБОЛЕ! / DRIBBLING WORKOUT",
                Type = Enums.TrainingType.Dribbling,
                Url = "https://www.youtube.com/embed/fFNXbVgZ2bE"
            });
            trainings.Add(new Training
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Тренировка Броска от Игрока СБОРНОЙ | Smoove",
                Type = Enums.TrainingType.GameThrowing,
                Url = "https://www.youtube.com/embed/H5uVc0WGUNs"
            });
            trainings.Add(new Training
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Трёхочковый бросок в баскетболе. Как научиться забивать?",
                Type = Enums.TrainingType.ThreePointer,
                Url = "https://www.youtube.com/embed/kvFpgiNYzgo"
            });

            trainings.Add(new Training
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Трёхочковый бросок в баскетболе. Как научиться забивать?",
                Type = Enums.TrainingType.Passing,
                Url = "https://www.youtube.com/embed/kvFpgiNYzgo"
            });

            trainings.Add(new Training
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Трёхочковый бросок в баскетболе. Как научиться забивать?",
                Type = Enums.TrainingType.Warmup,
                Url = "https://www.youtube.com/embed/kvFpgiNYzgo"
            });
            trainings.Add(new Training
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Трёхочковый бросок в баскетболе. Как научиться забивать?",
                Type = Enums.TrainingType.Dribbling,
                Url = "https://www.youtube.com/embed/kvFpgiNYzgo"
            });
            trainings.Add(new Training
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Трёхочковый бросок в баскетболе. Как научиться забивать?",
                Type = Enums.TrainingType.Dribbling,
                Url = "https://www.youtube.com/embed/kvFpgiNYzgo"
            });
            trainings.Add(new Training
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Трёхочковый бросок в баскетболе. Как научиться забивать?",
                Type = Enums.TrainingType.Dribbling,
                Url = "https://www.youtube.com/embed/kvFpgiNYzgo"
            });
            List<Role> roles = new List<Role>();
            roles.Add(new Role {Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "Admin" });
            roles.Add(new Role { Id = Guid.NewGuid().ToString(), Name = "Guest", NormalizedName = "Guest" });
            roles.Add(new Role { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "User" });

            modelBuilder.Entity<Role>().HasData(roles);
            modelBuilder.Entity<Training>().HasData(trainings);
            base.OnModelCreating(modelBuilder);
        }
    }
}
