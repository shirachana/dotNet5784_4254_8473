namespace DO;

public record User(string? Name, string? Id, string? UserName, string? Password, bool IsAdmin)
{
    User() : this(null, null, null, null, false){ }
}
