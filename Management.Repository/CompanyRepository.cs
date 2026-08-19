using Management.Contracts;
using Management.Entities.Models;

namespace Management.Repository;

public class CompanyRepository(RepositoryContext repositoryContext)
    : RepositoryBase<Company>(repositoryContext), ICompanyRepository
{

}
