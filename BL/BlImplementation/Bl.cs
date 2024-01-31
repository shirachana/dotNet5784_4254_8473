namespace BlImplementation;
using BlApi;
using BO;
using System.Threading.Tasks;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

}
