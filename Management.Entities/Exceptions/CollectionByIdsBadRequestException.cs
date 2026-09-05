namespace Management.Entities.ErrorModels.Exceptions;

public sealed class CollectionByIdsBadRequestException()
    : BadRequestException("Collection count mismatch comparing to ids.")
{ }
