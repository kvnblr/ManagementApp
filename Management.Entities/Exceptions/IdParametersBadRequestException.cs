namespace Management.Entities.ErrorModels.Exceptions;

public sealed class IdParametersBadRequestException()
    : BadRequestException("Parameter ids is null")
{ }

