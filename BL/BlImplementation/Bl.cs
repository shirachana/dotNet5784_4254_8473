using BlApi;
using BO;
using DalApi;
using DO;
using System.Data.Common;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BlImplementation;

internal class Bl : IBl
{
    private readonly DalApi.IDal dal = DalApi.Factory.Get;
    private readonly TaskImplementation _taskImplementation;
    public Bl()
    {
        _taskImplementation = new TaskImplementation(this);
    }
    public BlApi.IEngineer Engineer => new EngineerImplementation();

    public BlApi.ITask Task => new TaskImplementation(this);
    public BlApi.IUser User => new UserImplementation();
    private DateTime? _startDate;

    //public DateTime? StartDate => _startDate;

    public void SetStartDate(DateTime? startDate)
    {
        if (startDate is null)
            startDate = null;

        // Add any additional validation or logic here

        _startDate = startDate;
    }

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

    public DateTime? CreateSchedule(int id, DateTime? date)
    {
        DO.Task? task = dal.Task.Read(id);
        if (task == null) { throw new BlDoesNotExistException($"Task with is={id} doesn't exist"); }
        var starts = from BO.TaskInList tasks in _taskImplementation.GetDependencies(task)
                     where _taskImplementation.GetDependencies(task) != null && dal.Task.Read(tasks.Id) != null && dal.Task.Read(tasks.Id)!.StartDate == null
                     select tasks;
        if (starts.Count() > 0) { throw new BlCantScheduleTheProject("Can't update start day. Previous tasks don't have start days"); }
        var ends = from BO.TaskInList tasks in _taskImplementation.GetDependencies(task)
                   where _taskImplementation.GetDependencies(task) != null && dal.Task.Read(tasks.Id) != null && dal.Task.Read(tasks.Id)!.CompleteDate > date
                   select tasks;
        if (ends.Count() > 0) { throw new BlCantScheduleTheProject("Can't update start day. This date is sonner then the previous tasks end dates"); }
        if (_taskImplementation.GetDependencies(task) == null && date < dal.StartDate) { throw new BlCantScheduleTheProject("Can't update start day. This date is sonner then the project start day"); }
        DO.Task newTask = task with { StartDate = date };
        dal.Task.Update(newTask);
        return date;
        
    }
        public void ScheduleD()
        {
            List<BO.TaskInList> allNoStartDate = new List<BO.TaskInList>();
            IEnumerable<DO.Task> allTasks = dal.Task.ReadAll();
        int c = 0;

            foreach (var task in allTasks)
            {
                if ((_taskImplementation.GetDependencies(task)).Count() == 0)
                {
                    CreateSchedule(task.Id, dal.StartDate);
                c++;
                }
            }
            allTasks = dal.Task.ReadAll();
            while (allTasks.Any(t => t.StartDate == null))
            {
                foreach (DO.Task task in allTasks)
                {
                    IEnumerable<BO.TaskInList> tasksDependencies = _taskImplementation.GetDependencies(task);
                    List<BO.TaskInList> noStartDate = new List<BO.TaskInList>();
                    List<DO.Task> thereIsStartDate = new List<DO.Task>();

                    foreach (BO.TaskInList taskList in tasksDependencies)
                    {
                        DO.Task dep = dal.Task.Read(taskList.Id)!;
                        if (dep.StartDate == null)
                        {
                            noStartDate.Add(taskList);
                            allNoStartDate.Add(taskList);
                        }
                        else
                        {
                            thereIsStartDate.Add(dep);
                        }
                    }
                    if (noStartDate.Count() == 0 && task.StartDate == null)
                    {
                        var scheduledD= thereIsStartDate.Max(dep => dep.StartDate/*WantedStartDate*/ + dep.RequiredEffortTime/*Time*/);
                        CreateSchedule(task.Id, (DateTime)scheduledD!);
                        allTasks = dal.Task.ReadAll();
                    }
                }
            }
        }




    }



