namespace DO;

public record User(string? UserName, string? Id, string? Password, bool IsAdmin)
{
    User() : this(null, null, null, false){ }
}
