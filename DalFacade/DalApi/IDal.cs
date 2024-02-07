namespace DalApi;

public interface IDal
{
    IDependency Dependency { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }
    public DateTime? GetStartDay();
    public void setStartDay(DateTime? startDate);
}
