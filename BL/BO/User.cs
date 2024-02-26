
namespace BO;

public record User()
{
    public string? userName { get; init; }
    public string? Id { get; set; }
    public bool isAdmin { get; set; }

    public override string ToString() => this.ToStringProperty();
}
