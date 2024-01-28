namespace BO;
/// <summary>
/// engineer
/// </summary>
/// <param name="Id"></Unique ID number>
/// <param name="Cost"></cost per hour>
/// <param name="Email"></Email>
/// <param name="Name"></Engineer's name (full name)>
/// <param name="Level"></Engineer level>
/// <param name="Task"></TaskInEngineer tasks engineer>
public class Engineer
{
    public int Id { get; init; }
    public double Cost { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public BO.EngineerExperience? Level { get; set; }
    public BO.TaskInEngineer? Task { get; init; }
    // public override string ToString() => this.ToStringProperty();
}