using Microsoft.EntityFrameworkCore;
using WebAPI.Controllers;
using WebAPI.Models;

namespace WebAPI.DB;

public sealed class ApplicationContext: DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    
    public DbSet<Message> Messages { get; set; }
    
    public DbSet<Topic> Topics { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    { 
        Database.EnsureCreated();
    } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseLazyLoadingProxies();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Contact>()
            .HasIndex(c => new { c.Email, c.PhoneNumber })
            .IsUnique();
        // One topic to many messages
        builder.Entity<Message>()
            .HasOne(m => m.Topic)
            .WithMany();
        // One contact to many messages
        builder.Entity<Message>()
            .HasOne(m => m.Contact)
            .WithMany();
    }
}