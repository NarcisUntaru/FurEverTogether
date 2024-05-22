using Microsoft.EntityFrameworkCore;
using FurEver_Together.DataModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FurEver_Together.ViewModels;

namespace FurEver_Together.Data;

public partial class FurEverTogetherDbContext : IdentityDbContext<User>
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
    public virtual DbSet<Volunteer> Volunteers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     => optionsBuilder.UseSqlServer("Name=ConnectionStrings:FurEverTogetherDb");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<User>()
        .HasOne(u => u.Adoption)
        .WithOne(a => a.User)
        .HasForeignKey<User>(u => u.AdoptionId);

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

        //modelBuilder.Entity<Adoption>()
        //    .HasOne(a => a.Cat)
        //    .WithOne(c => c.Adoption)
        //    .HasForeignKey<Cat>(c => c.Id);

        //modelBuilder.Entity<Adoption>()
        //    .HasOne(a => a.Dog)
        //    .WithOne(d => d.Adoption)
        //    .HasForeignKey<Dog>(d => d.Id);

        modelBuilder.Entity<Cat>()
        .HasOne(c => c.Adoption)
        .WithOne(a => a.Cat)
        .HasForeignKey<Adoption>(a => a.CatId)
        .IsRequired(false);

        modelBuilder.Entity<Dog>()
        .HasOne(c => c.Adoption)
        .WithOne(a => a.Dog)
        .HasForeignKey<Adoption>(a => a.DogId)
        .IsRequired(false);
    }


}