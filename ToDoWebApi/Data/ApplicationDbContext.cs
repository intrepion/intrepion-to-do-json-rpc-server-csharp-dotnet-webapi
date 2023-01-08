using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoWebApi.Entities;

namespace ToDoWebApi.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
        DbInitializer.Initialize(this);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ToDoListEntity>()
            .HasMany(toDoList => toDoList.ToDoItems)
            .WithOne(toDoItem => toDoItem.ToDoList)
            .IsRequired();

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(applicationUser => applicationUser.ToDoLists)
            .WithOne(toDoList => toDoList.ApplicationUser)
            .IsRequired();
    }

    public DbSet<ToDoItemEntity>? ToDoItems { get; set; }
    public DbSet<ToDoListEntity>? ToDoLists { get; set; }
}
