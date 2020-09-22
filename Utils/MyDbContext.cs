using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore; 
using MySql.Data.EntityFrameworkCore.DataAnnotations;
using AppRastreamento.Models;

/*
https://insidemysql.com/mysql-connector-net-provider-for-entity-framework-core-3-1/
*/

namespace AppRastreamento.Utils
{
    public partial class MyDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<ObjectRastreio> ObjectRastreio { get; set; }
        public DbSet<ObjectLocation> ObjectLocation { get; set; }
        public DbSet<Notification> Notification { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string server = Environment.GetEnvironmentVariable("SERVER_MYSQL");
            string user = Environment.GetEnvironmentVariable("USER_MYSQL");
            string password = Environment.GetEnvironmentVariable("PASSWORD_MYSQL");
            string database = Environment.GetEnvironmentVariable("DB_MYSQL");

            options.UseMySQL($"Server={server};User Id={user};Password={password};Database={database}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.idUser);
            });

            modelBuilder.Entity<ObjectRastreio>(entity =>
            {
                entity.HasKey(e => e.idObject);
            });

            modelBuilder.Entity<ObjectLocation>(entity =>
            {
                entity.HasKey(e => e.idObjectLocation);
                entity.HasOne(d => d.objectRastreio)
                .WithMany(p => p.objectLocation)
                .HasForeignKey(p => p.idObject);
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.idNotification);
            });

            
        }
        
    }

}