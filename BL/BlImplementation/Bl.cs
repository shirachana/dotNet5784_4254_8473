namespace BlImplementation;
using BlApi;
internal class Bl : IBl
{
    public IEngineer Engineer => throw new NotImplementedException();

    public ITask Task => throw new NotImplementedException();

    public IEngineer engineer => new EngineerImplementation(); 
    public ITask task => new TaskImplementation();
}
