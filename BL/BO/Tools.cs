using BlApi;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BO;

public static class Tools
{
    // Static extension method for ToStringProperty

    public static string ToStringProperty<T>(this T obj)
    {
        if (obj == null)
            return "null";

        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Type: {type.Name}");

        foreach (PropertyInfo property in properties)

        {
            if(property is IEnumerable)
            {
                IEnumerable<object>? enumerable = property.GetValue(obj) as IEnumerable<object>;
                if (enumerable != null)
                {
                    sb.AppendLine($"{property.Name}:");
                    foreach (var item in enumerable)
                    {
                        sb.AppendLine($"    {GetValueString(item)}");
                    }
                }
            }
            sb.AppendLine($"{property.Name}: {GetValueString(property.GetValue(obj)!)}");
        }

        return sb.ToString();
    }

    // Helper method to get string representation of property value
    private static string GetValueString(object value)
    {
        if (value == null)
            return "null";

        if (value is IEnumerable<object> enumerable)
        {
            // Handle collections
            StringBuilder sb = new StringBuilder();
            foreach (var item in enumerable)
            {
                sb.Append($"{GetValueString(item)}, ");
            }
            // Remove the trailing comma and space
            return $"[{sb.ToString().TrimEnd(',', ' ')}]";
        }

        // For other types, use ToString
        return value.ToString()!;
    }
    public static BO.Status GetStatus(DO.Task delTask)
    {
        return delTask.ScheduledDate is null ? Status.Unscheduled :
            delTask.StartDate is null ? Status.Scheduled :
            delTask.CompleteDate is null ? Status.OnTrack :
            Status.Done;

    }
    public static bool CheckEngineerHealth(Engineer item)
    {
        if (item.Id <= 0)
            throw new BlArgumentOutOfRangeException("Engineer ID must be a positive number");
        if ((item.Name)== "")
            throw new BlArgumentOutOfRangeException("Engineer name cannot be empty");
        if (item.Cost < 0)
            throw new BlArgumentOutOfRangeException("Engineer cost must be a positive number");
        if ((item.Email!).Contains("@gmail.com")==false)
            throw new BlArgumentOutOfRangeException("Engineer email cannot be empty");

        // If all checks pass, return true
        return true;
    }
    public static bool CheckTaskHealth(Task item)
    {
        if (item == null)
        {
            throw new BlDoesNotExistException("Task object cannot be null");
        }
        // Check the validity of the task's data
        //if (item.Id <= 0)
        //{
        //    throw new BlCantBeDeletedException("Task ID must be a positive number");
        //}
        if (string.IsNullOrEmpty(item?.Alias))
        {
            throw new BlArgumentOutOfRangeException("Task alias cannot be empty");
        }
        // If all checks pass, return true
        return true;
    }
    public static StatusProject CheckStatusProject()
    {
        var tasks = BlApi.Factory.Get().Task;

        if (IBl.StartDateProject == null)
            return StatusProject.Planning;

        if ((from task in tasks.ReadAll()
             where tasks.Read(task.Id)!.StartDate == null
             select task).Any())
        {
            return StatusProject.Mid;
        }

        return StatusProject.Execution;
    }
    public static bool IsStatusProject(Task item)
    {
        if (Tools.CheckStatusProject() == BO.StatusProject.Planning && item?.Engineer?.Id != null)
        {
            throw new BlPlanning("the project is during planning");
        }
        if (Tools.CheckStatusProject() == BO.StatusProject.Planning && item?.StartDate != null)
        {
           throw new BlPlanning("the project is during planning");
        }
        if (Tools.CheckStatusProject() == BO.StatusProject.Execution)
        {
            throw new BlExection("the project is during Exection");
        }
        return true;
}






}
