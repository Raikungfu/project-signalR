using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UserChatManagement.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        private readonly IConfiguration _configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomUser> RoomUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:DefaultConnection"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<PrivateMessage>()
        .HasOne(pm => pm.FromUser)
        .WithMany(u => u.PrivateMessagesSent)
        .HasForeignKey(pm => pm.FromUserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<PrivateMessage>()
        .HasOne(pm => pm.ToUser)
        .WithMany(u => u.PrivateMessagesReceived)
        .HasForeignKey(pm => pm.ToUserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<RoomUser>()
        .HasKey(ru => new { ru.RoomId, ru.UserId });

    modelBuilder.Entity<RoomUser>()
        .HasOne(ru => ru.Room)
        .WithMany(r => r.RoomUsers)
        .HasForeignKey(ru => ru.RoomId);

    modelBuilder.Entity<RoomUser>()
        .HasOne(ru => ru.User)
        .WithMany(u => u.RoomUsers)
        .HasForeignKey(ru => ru.UserId);

    modelBuilder.Entity<ApplicationUser>()
        .HasMany(u => u.Rooms)
        .WithMany(r => r.Users)
        .UsingEntity<RoomUser>(
            j => j
                .HasOne(ru => ru.Room)
                .WithMany(r => r.RoomUsers)
                .HasForeignKey(ru => ru.RoomId),
            j => j
                .HasOne(ru => ru.User)
                .WithMany(u => u.RoomUsers)
                .HasForeignKey(ru => ru.UserId),
            j =>
            {
                j.HasKey(ru => new { ru.RoomId, ru.UserId });
            });
}

    }
}
