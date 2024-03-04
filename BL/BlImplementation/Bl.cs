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
    
    public void SetProjectStartDate(DateTime? startDate)
    {
        dal.setStart(startDate);
    }
    public DateTime? GetProjectStartDate()
    {
        return dal.getStart();
    }
    public void SetProjectEndDate(DateTime? endDate)
    {
        //dal.setEnd(endDate);
        dal.EndDate = endDate;
    }
    public DateTime? GetProjectEndDate()
    {
        //return dal.getEnd();
        return dal.EndDate;
    }


    //public DateTime? StartDate
    //{
    //    set
    //    {
    //        if (value is null)
    //            value = null;

    //        //if (dal.Task.ReadAll(x => x.StartDate < value || x.StartDate is null).Any())
    //        //    throw new ArgumentException();

    //        dal. . value;
    //    }
    //    get => dal.StartDate;
    //}
    //public DateTime? EndDate
    //{
    //    set
    //    {
    //        if (value is null)
    //            value = null;

    //        if (dal.Task.ReadAll(x => x.DeadlineDate > value || x.DeadlineDate is not null).Any())
    //            throw new BlDoesNotExistException("there is no daedline or the deadline is bigger than value");

    //        dal.EndDate = value;
    //    }
    //    get => dal.EndDate;
    //}
    private static DateTime s_Clock = DateTime.Now;

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
        SetProjectEndDate(null);
        SetProjectStartDate(null);
    }
    public DateTime? Schedule(int taskId, DateTime? date)
    {
        DO.Task? task = dal.Task.Read(taskId);
        if (task == null)
        {
            throw new BlDoesNotExistException($"Task with id={taskId} doesn't exist");
        }

        ValidateSchedule(task, date);

        DO.Task newTask = task with { ScheduledDate = date };
        dal.Task.Update(newTask);
        return date;
    }

    private void ValidateSchedule(DO.Task task, DateTime? date)
    {
        IEnumerable<BO.TaskInList> dependencies = _taskImplementation.GetDependencies(task);

        ValidateStartDates(dependencies);
        ValidateEndDates(dependencies, date);
        ValidateProjectStartDate(dependencies, date);
    }

    private void ValidateStartDates(IEnumerable<BO.TaskInList> dependencies)
    {
        if (dependencies != null && dependencies.Any(dep => dal.Task.Read(dep.Id)?.ScheduledDate == null))
        {
            throw new BlCantScheduleTheProject("Can't update start day. Previous tasks don't have start days");
        }
    }
    private void ValidateEndDates(IEnumerable<BO.TaskInList> dependencies, DateTime? date)
    {
        if (dependencies != null && dependencies.Any(dep => dal.Task.Read(dep.Id)?.CompleteDate > date))
        {
            throw new BlCantScheduleTheProject("Can't update start day. This date is sooner than the previous tasks end dates");
        }
    }

    private void ValidateProjectStartDate(IEnumerable<BO.TaskInList> dependencies, DateTime? date)
    {
        if (dependencies == null && date < dal.getStart())
        {
            throw new BlCantScheduleTheProject("Can't update start day. This date is sooner than the project start day");
        }
    }


    public void AutoScedule()
    {
        IEnumerable<DO.Task> allTasks = dal.Task.ReadAll();

        foreach (var task in allTasks.Where(t => _taskImplementation.GetDependencies(t).Count() == 0))
        {
            Schedule(task.Id, GetProjectStartDate());
        }

        allTasks = dal.Task.ReadAll();

        while (allTasks.Any(t => t.ScheduledDate == null))
        {
            foreach (DO.Task task in allTasks)
            {
                IEnumerable<BO.TaskInList> tasksDependencies = _taskImplementation.GetDependencies(task);

                List<BO.TaskInList> tasksWithoutStartDate = tasksDependencies
                    .Where(taskList => dal.Task.Read(taskList.Id)?.ScheduledDate == null)
                    .ToList();

                List<DO.Task> tasksWithStartDate = tasksDependencies
                    .Select(taskList => dal.Task.Read(taskList.Id)!)
                    .Where(dep => dep.ScheduledDate != null)
                    .ToList();

                if (tasksWithoutStartDate.Count == 0 && task.ScheduledDate == null)
                {
                    DateTime? calculatedScheduledDate = tasksWithStartDate.Max(dep => dep.ScheduledDate + dep.RequiredEffortTime);
                    Schedule(task.Id, calculatedScheduledDate);

                    allTasks = dal.Task.ReadAll();
                }
            }
        }
    }
 
}



