namespace MyPlanner.Domain.Common
{
    public enum ErrorType
    {
        None,
        NotFound,
        Validation,
        Conflict,
        Unauthorized,
        Forbidden,
        Failure,
    }
}
