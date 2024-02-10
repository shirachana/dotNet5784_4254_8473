using BlApi;
using BO;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BlImplementation;

internal class Bl : IBl
{
    private readonly DalApi.IDal dal = DalApi.Factory.Get;
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public DateTime? StartDate
    {
        set
        {
            if (value is null)
                throw new BlDoesNotExistException("there is no start date");

            //if (dal.Task.ReadAll(x => x.StartDate < value || x.StartDate is null).Any())
            //    throw new ArgumentException();

            dal.StartDate = value;
        }
        get => dal.StartDate;
    }
    public DateTime? EndDate {
        set
        {
            if (value is null)
                throw new BlDoesNotExistException("there is no end date");

            if (dal.Task.ReadAll(x => x.DeadlineDate > value || x.DeadlineDate is not null).Any())
                throw new BlDoesNotExistException("there is no daedline or the deadline is bigger than value");

            dal.EndDate = value;
        }
        get => dal.EndDate;
    }

}



