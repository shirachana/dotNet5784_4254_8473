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
    private int idD;
    private double costForHour;
    private string? engineerEmail;
    private string? engineerName;
    private EngineerExperience? engineerLevel;

    public Engineer()
    {
    }

    public Engineer(int idD, double costForHour, string? engineerEmail, string? engineerName, EngineerExperience? engineerLevel)
    {
        this.idD = idD;
        this.costForHour = costForHour;
        this.engineerEmail = engineerEmail;
        this.engineerName = engineerName;
        this.engineerLevel = engineerLevel;
    }

    public int Id { get; set; }
    public double Cost { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public BO.EngineerExperience? Level { get; set; }
    public BO.TaskInEngineer? Task { get; set; }
     public override string ToString() => this.ToStringProperty();
}