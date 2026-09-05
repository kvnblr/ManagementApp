namespace Management.Entities.ErrorModels.Exceptions;

public sealed class CompanyCollectionBadRequest()
    : BadRequestException("Company collection sent from a client is null")
{ }
