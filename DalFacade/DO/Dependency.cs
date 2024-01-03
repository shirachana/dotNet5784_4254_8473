namespace DO;
/// <summary>
/// dependence
/// </summary>
/// <param name="Id"></Unique ID number (automatic runner number)>
/// <param name="DependentTask"></ID number of pending task>
/// <param name="DependsOnTask"></Previous assignment ID number>
public record Dependency
(
    int Id,
    int DependentTask,
    int DependsOnTask
)
{
    public Dependency() : this(0, 0, 0) { }

}
