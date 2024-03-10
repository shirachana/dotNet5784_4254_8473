using BlApi;
using BlImplementation;
using System.Reflection;
using System.Text;
using System.Collections;

namespace BO;

public static class Tools
{
    private static readonly IBl _bl = Factory.Get();
    private static readonly DalApi.IDal dal = DalApi.Factory.Get;

    public static string ToStringProperty<Item>(this Item t, string str = "")
    {
        IEnumerable<PropertyInfo> propertyInfos = t!.GetType().GetProperties();

        foreach (PropertyInfo item in propertyInfos)
        {
            object value = item.GetValue(t, null)!;

            if (value is not null)
            {
                if (value is IEnumerable objects and not string)
                {
                    str += "\n" + "items " + ":\n";
                    foreach (var obj in objects)
                        return obj.ToStringProperty(str);
                }
                else
                    str += "\n" + item.Name + ": " + value;

            }
        }
        return str;
    }
    private static string GetValueString(object value)
    {
        if (value == null)
            return "null";

        if (value is IEnumerable<object> enumerable)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in enumerable)
            {
                sb.Append($"{GetValueString(item)}, ");
            }
            return $"[{sb.ToString().TrimEnd(',', ' ')}]";
        }
        if (value is List<object> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                sb.Append($"{GetValueString(item)}, ");
            }
            return $"[{sb.ToString().TrimEnd(',', ' ')}]";
        }
        return value.ToString()!;
    }

    public static BO.Status GetStatus(DO.Task delTask)
    {
        if (delTask.StartDate is not null && delTask.ForecastDate < _bl.Clock)
            return Status.Delayed;

        return delTask.ScheduledDate is null ? Status.Unscheduled :
            delTask.StartDate is null ? Status.Scheduled :
            delTask.CompleteDate is null ? Status.OnTrack : Status.Done;

        //if (delTask.CompleteDate != null /*&& delTask.CompleteDate < DateTime.Today */)
        //{
        //    return Status.Done;
        //}

        //return delTask.StartDate is null ? Status.Unscheduled :
        //    delTask.StartDate is not null && delTask.StartDate > DateTime.Today ? Status.Scheduled :
        //    delTask.StartDate < DateTime.Today && delTask.StartDate + delTask.RequiredEffortTime > DateTime.Today ? Status.OnTrack :
        //    delTask.StartDate > delTask.ScheduledDate || delTask.CompleteDate > delTask.ForecastDate ? Status.Delayed :
        //    Status.Done;
    }


    public static bool CheckUserHealth(User item)
    {
        if (item.Id is null)
            throw new BlArgumentOutOfRangeException("User code must be a positive number");
        if ((item.Name) is null)
            throw new BlArgumentOutOfRangeException("User name cannot be empty");

        return true;
    }
    public static bool CheckEngineerHealth(Engineer item)
    {
        if (item.Id <= 0)
            throw new BlArgumentOutOfRangeException("Engineer ID must be a positive number");
        if ((item.Name) == "")
            throw new BlArgumentOutOfRangeException("Engineer name cannot be empty");
        if (item.Cost < 0)
            throw new BlArgumentOutOfRangeException("Engineer cost must be a positive number");
        if ((item.Email!).Contains("@gmail.com") == false)
            throw new BlArgumentOutOfRangeException("Engineer email cannot be empty");

        return true;
    }
    public static bool CheckTaskHealth(Task item)
    {
        if (item == null)
        {
            throw new BlDoesNotExistException("Task object cannot be null");
        }
        if (string.IsNullOrEmpty(item?.Alias))
        {
            throw new BlArgumentOutOfRangeException("Task alias cannot be empty");
        }
        return true;
    }
    public static StatusProject CheckStatusProject()
    {
        var tasks = BlApi.Factory.Get().Task;

        if (Factory.Get().GetProjectStartDate == null)
            return StatusProject.Planning;
        if ((from task in tasks.ReadAll()
             where tasks.Read(task.Id)!.StartDate == null
             select task).Count() > 0)
        {
            return StatusProject.Mid;
        }
        else
        {
            return StatusProject.Execution;
        }
    }

    public static bool IsStatusProject(Task item)
    {
        if (CheckStatusProject() == BO.StatusProject.Planning && item?.Engineer?.Id != null)
        {
            throw new BlPlanning("The program is in the planning");
        }

        if (CheckStatusProject() == BO.StatusProject.Planning && item?.StartDate != null)
        {
            throw new BlPlanning("the project is during planning");
        }

        if (CheckStatusProject() == BO.StatusProject.Execution)
        {
            throw new BlExection("the project is during Exection");
        }
        return true;
    }

    public static void CreatTheSchedule(DateTime startDateP)// ????????????????????????????????????????????
    {
        if (startDateP >= DateTime.Today)
            Factory.Get().SetProjectStartDate(startDateP);
        else
            throw new BlCantScheduleTheProject("the time pass already");
    }

    public static int GetOfset(DO.Task task)
    {
        IEnumerable<DO.Dependency> dependencies = dal.Dependency.ReadAll(x => x.DependentTask == task.Id);
        if (dependencies.Count() == 0)
            return (task.StartDate != null ? (int)(task.StartDate - _bl.GetProjectStartDate())?.TotalDays! :
                                            (int)(task.ScheduledDate - _bl.GetProjectStartDate())?.TotalDays!)*100;

        int x=  ((int)((from dep in dependencies
                let depTask = dal.Task.Read(dep.DependsOnTask)
                select depTask.ScheduledDate + depTask.RequiredEffortTime).Max() - _bl.GetProjectStartDate())?.TotalDays!)*100;
        return x;
    }
}







