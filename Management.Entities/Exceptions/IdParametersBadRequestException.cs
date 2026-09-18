namespace Management.Entities.Exceptions;

public sealed class IdParametersBadRequestException()
    : BadRequestException("Parameter ids is null")
{ }

