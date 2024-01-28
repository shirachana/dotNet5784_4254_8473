namespace BO;
/// <summary>
/// <param name="Id"></Unique ID number>
/// <param name="Name"></Engineer's name (full name)>
/// </summary>
public class EngineerInTask
{
    public int Id {  get; init; }
    public string? Name { get; set; }
    public override string ToString() => this.ToStringProperty();
}
