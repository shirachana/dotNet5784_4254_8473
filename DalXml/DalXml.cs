using DalApi;
using System.Xml.Linq;
namespace Dal;
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
  
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public DateTime? StartDate { set=> Config.SetDate("StartDate", value); get => Config.GetDate("StartDate"); }
    public DateTime? EndDate { set => Config.SetDate("EndDate", value); get => Config.GetDate("EndDate"); }
}

