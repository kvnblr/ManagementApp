namespace Management.Entities.Exceptions;

public sealed class RefereshTokenBadRequestException()
    : BadRequestException("Invalid client request. The tokenDto has some invalid values.")
{ }


