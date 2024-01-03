namespace DO;
/// <summary>
/// engineer
/// </summary>
/// <param name="Id"></Unique ID number>
/// <param name="cost"></cost per hour>
/// <param name="Email"></Email>
/// <param name="Name"></Engineer's name (full name)>
/// <param name="Level"></Engineer level>
public record Engineer
(
    int Id,
    double cost,
    string? Email = null,
    string? Name = null,
    DO.EngineerExperience? Level = null
)
{
    public Engineer() : this(0, 0) { }
}
