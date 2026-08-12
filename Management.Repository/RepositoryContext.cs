using Management.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Management.Repository;

public class RepositoryContext(DbContextOptions<RepositoryContext> options)
    : DbContext(options)
{

    public DbSet<Company>? Companies { get; set; }
    public DbSet<Employee>? Employees { get; set; }
}
