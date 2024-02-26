using BlApi;
using BO;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BlImplementation;

internal class Bl : IBl
{
    private readonly DalApi.IDal dal = DalApi.Factory.Get;
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation(this);
    public IUser User => new UserImplementation();

    public DateTime? StartDate
    {
        set
        {
            if (value is null)
                value = null;

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
                value = null;

            if (dal.Task.ReadAll(x => x.DeadlineDate > value || x.DeadlineDate is not null).Any())
                throw new BlDoesNotExistException("there is no daedline or the deadline is bigger than value");

            dal.EndDate = value;
        }
        get => dal.EndDate;
    }
    private static DateTime s_Clock = DateTime.Now.Date;

    public DateTime Clock
    {
        get { return s_Clock; }
        private set { s_Clock = value; }
    }

    public void AdvanceTimeByDay(int days)
    {
        s_Clock = s_Clock.AddDays(days);
    }

    public void AdvanceTimeByHour(int hours)
    {
        s_Clock = s_Clock.AddHours(hours);
    }

    public void AdvanceTimeByYear(int years)
    {
        s_Clock = s_Clock.AddYears(years);
    }

    public void InitializeDB() => DalTest.Initialization.Do();

    public void InitializeTime(DateTime initialDateTime)
    {
        s_Clock = initialDateTime;
    }

    public void ResetDB()
    {
        dal.Engineer.Clear();
        dal.User.Clear();
        dal.Task.Clear();
        dal.Dependency.Clear();
    }

}



