using Management.Entities.Models;
using Management.Repository.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Management.Repository;

public class RepositoryContext(DbContextOptions<RepositoryContext> options)
    : DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }

    public DbSet<Company>? Companies { get; set; }
    public DbSet<Employee>? Employees { get; set; }
}
