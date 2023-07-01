using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NOOKX_Project.Models;
using System.Reflection;

namespace NOOKX_Project.NOOKXDataBase;

public class NookxDbContext : IdentityDbContext<AppUser>
{
    public NookxDbContext(DbContextOptions<NookxDbContext> options ) : base (options)
    {
        
    }

    public DbSet<Catagory> Catagories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<MailSetting> MailSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<MailSetting>()
            .HasKey(m => m.Id);
    }
}
