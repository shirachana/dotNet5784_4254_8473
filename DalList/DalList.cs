namespace Dal;
using DalApi;
sealed internal class DalList : IDal
{
    public static IDal Instance { get; } = new DalList();
    private DalList() { }

    public DateTime? GetStartDay()
    {
        return DataSource.Config.StartDateProject;
    }
    public void setStartDay(DateTime? startDate)
    {
        DataSource.Config.StartDateProject = startDate;
    }

    public IDependency Dependency => new DependencyImplementation();

    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task =>  new TaskImplementation();
    
}
	


