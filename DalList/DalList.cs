namespace Dal;
using DalApi;
using DO;

sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }
    public DateTime? StartDate
    {
        set => DataSource.Config.StartDateProject = value;
        get => DataSource.Config.StartDateProject;
    }
    public DateTime? EndDate
    {
        set => DataSource.Config.EndDateProject = value;
        get => DataSource.Config.EndDateProject;
    }

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task =>  new TaskImplementation();
    public IUser User => new UserImplementation();
    public void setStart(DateTime? start) => DataSource.Config.StartDateProject = start; 
    public DateTime? getStart() { return DataSource.Config.StartDateProject; }

    public void setEnd(DateTime? end) => DataSource.Config.EndDateProject = end;
    public DateTime? getEnd() { return DataSource.Config.EndDateProject; }



}



