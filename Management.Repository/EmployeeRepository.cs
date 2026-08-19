using Management.Contracts;
using Management.Entities.Models;

namespace Management.Repository;

public class EmployeeRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Employee>(repositoryContext), IEmployeeRepository
{ }
