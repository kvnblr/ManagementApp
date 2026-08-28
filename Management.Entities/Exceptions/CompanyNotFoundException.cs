namespace Management.Entities.Exceptions;

public sealed class CompanyNotFoundException(Guid companyId)
    : NotFoundException($"The company with id: {companyId} doesn't exists in the database")
{ }

