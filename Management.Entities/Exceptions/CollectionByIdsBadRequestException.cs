namespace Management.Entities.Exceptions;

public sealed class CollectionByIdsBadRequestException()
    : BadRequestException("Collection count mismatch comparing to ids.")
{ }
