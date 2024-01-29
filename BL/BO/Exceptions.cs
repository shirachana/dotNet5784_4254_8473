
namespace BO;

[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}
[Serializable]
public class BlArgumentOutOfRangeException : Exception
{
    public BlArgumentOutOfRangeException(string? message) : base(message) { }
    public BlArgumentOutOfRangeException(string message, Exception innerException)
                : base(message, innerException) { }
}
[Serializable]
public class BlAlreadyExistException : Exception
{
    public BlAlreadyExistException(string? message) : base(message) { }
    public BlAlreadyExistException(string message, Exception innerException)
                : base(message, innerException) { }

}
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}
[Serializable]
public class BlCantBeDeletedException : Exception
{
    public BlCantBeDeletedException(string? message) : base(message) { }
    public BlCantBeDeletedException(string message, Exception innerException)
                : base(message, innerException) { }
}
