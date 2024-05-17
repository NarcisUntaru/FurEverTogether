﻿using Microsoft.EntityFrameworkCore;
using FurEver_Together.DataModels;

namespace FurEver_Together.Models;

public partial class FurEverTogetherDbContext : DbContext
{
    public FurEverTogetherDbContext()
    {
    }

    public FurEverTogetherDbContext(DbContextOptions<FurEverTogetherDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cat> Cats { get; set; }
    public virtual DbSet<Adoption> Adoptions { get; set; }

    public virtual DbSet<ContactUs> ContactUs { get; set; }
    public virtual DbSet<Dog> Dogs { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Volunteer> Volunteers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     => optionsBuilder.UseSqlServer("Name=ConnectionStrings:FurEverTogetherDb");

    public DbSet<FurEver_Together.Models.DogViewModel> DogViewModel { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adoption>()
            .HasOne(a => a.User)
            .WithMany(u => u.Adoptions)
            .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<ContactUs>()
            .HasOne(c => c.User)
            .WithMany(u => u.ContactMessages)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Volunteer>()
            .HasOne(v => v.User)
            .WithOne(u => u.Volunteer)
            .HasForeignKey<Volunteer>(v => v.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Adoption>()
            .HasOne(a => a.Cat)
            .WithOne(c => c.Adoption)
            .HasForeignKey<Cat>(c => c.Id);

        modelBuilder.Entity<Adoption>()
            .HasOne(a => a.Dog)
            .WithOne(d => d.Adoption)
            .HasForeignKey<Dog>(d => d.Id);
    }
}