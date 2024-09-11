namespace TaskManagement.DAL.Data;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskManagement.DAL.Models;

public class TaskManagementDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public TaskManagementDbContext(DbContextOptions options)
        : base(options)
    {
        this.Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        // User
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Task
        modelBuilder.Entity<Task>()
              .Property(e => e.Status)
              .HasConversion<string>();
        modelBuilder.Entity<Task>()
              .Property(e => e.Priority)
              .HasConversion<string>();
    }
}