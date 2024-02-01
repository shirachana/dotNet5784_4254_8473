namespace BlImplementation;
using BlApi;
using BO;
using System.Threading.Tasks;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();
    static DateTime? StartDateProject { get; set; } = null;
    public DateTime? EndDateProject { get; set; } = null;

}
