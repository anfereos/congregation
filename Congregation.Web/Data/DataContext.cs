﻿using Congregation.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Congregation.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Church> Churches { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<District> Districts { get; set; }

        public DbSet<Profession> Professions { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<Assistance> Assistances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>(cou =>
            {
                cou.HasIndex("Name").IsUnique();
                cou.HasMany(c => c.Districts).WithOne(d => d.Country).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<District>(dep =>
            {
                dep.HasIndex("Name", "CountryId").IsUnique();
                dep.HasOne(d => d.Country).WithMany(c => c.Districts).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Church>(cit =>
            {
                cit.HasIndex("Name", "DistrictId").IsUnique();
                cit.HasOne(c => c.District).WithMany(d => d.Churches).OnDelete(DeleteBehavior.Cascade);
                cit.HasMany(c => c.Meetings).WithOne(d => d.Church).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Meeting>(mee =>
            {
                mee.HasIndex("Date").IsUnique();
                mee.HasMany(c => c.Assistances).WithOne(d => d.Meeting).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Profession>(prof =>
            {
                prof.HasIndex("Name").IsUnique();
                prof.HasMany(u => u.Users).WithOne(p => p.Profession).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
