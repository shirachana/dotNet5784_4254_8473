namespace DalApi;

public interface IDal
{
    IDependency Dependency { get; }
    IEngineer Engineer { get; }
    ITask Task { get; }

    DateTime? StartDate { set; get; }
    DateTime? EndDate { set; get; }

}
