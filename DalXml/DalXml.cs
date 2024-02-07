using DalApi;
using System.Xml.Linq;
namespace Dal;
sealed internal class DalXml : IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public DateTime? GetStartDay()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        return root.ToDateTimeNullable("StartDate");
    }
    public void setStartDay(DateTime? startDate)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("StartDate")!.ReplaceWith(new XElement("StartDate", startDate.ToString()!));
        XMLTools.SaveListToXMLElement(root, "data-config");
    }
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DependencyImplementation();
}

