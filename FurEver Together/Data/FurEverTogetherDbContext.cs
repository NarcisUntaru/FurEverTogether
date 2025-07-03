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

    public virtual DbSet<Adoption> Adoptions { get; set; }
    public virtual DbSet<ContactUs> ContactUs { get; set; }
    public virtual DbSet<Pet> Pets { get; set; }
    public virtual DbSet<Volunteer> Volunteers { get; set; }
    public DbSet<PersonalityProfile> PersonalityProfiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     => optionsBuilder.UseMySQL("Name=ConnectionStrings:FurEverTogetherDb");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
    .HasOne(a => a.User)
    .WithMany(u => u.Adoptions)
    .HasForeignKey(a => a.UserId)
    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pet>().OwnsOne(p => p.Personality);
        modelBuilder.Entity<User>().OwnsOne(a => a.Preferences);

    }


}