using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FurEver_Together.Infrastructure;
using FurEver_Together.Repository;
using Microsoft.AspNetCore.Identity;
using FurEver_Together.DataModels;
using FurEver_Together.Data;
using FurEver_Together.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FurEverTogetherDbContext>();
builder.Services.AddDbContext<FurEverTogetherDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FurEverTogetherDb")));


builder.Services.AddScoped(typeof(IAdoptionRepository), typeof(AdoptionRepository));
builder.Services.AddScoped(typeof(ICatRepository), typeof(CatRepository));
builder.Services.AddScoped(typeof(IContactUsRepository), typeof(ContactUsRepository));
builder.Services.AddScoped(typeof(IDogRepository), typeof(DogRepository));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(IVolunteerRepository), typeof(VolunteerRepository));
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 10;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
});

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();