using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FurEver_Together.Models;
using FurEver_Together.Infrastructure;
using FurEver_Together.Repository;
using FurEver_Together.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<FurEverTogetherDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FurEverTogetherDb")));
builder.Services.AddScoped(typeof(IAdoptionRepository), typeof(AdoptionRepository));
builder.Services.AddScoped(typeof(ICatRepository), typeof(CatRepository));
builder.Services.AddScoped(typeof(IContactUsRepository), typeof(ContactUsRepository));
builder.Services.AddScoped(typeof(IDogRepository), typeof(DogRepository));
builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
builder.Services.AddScoped(typeof(IVolunteerRepository), typeof(VolunteerRepository));
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();