
namespace BO;

public record User()
{
    public string? Name { get; init; }
    public string? Id { get; set; }
    public string? UserName { get; init; }
    public string? Password { get; set; }
    public bool IsAdmin { get; set; }

    public override string ToString() => this.ToStringProperty();
}
