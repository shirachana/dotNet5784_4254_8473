using BO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;


namespace PL;

public class ConvertIdToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return true;
        return (int)value == 0 ? "Add" : "Update";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ConvertIdToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return true;
        bool isAddMode = (int)value == 0;
        return isAddMode;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ConvertIdToVisibilityDel : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return false; // Return false to hide the button if value is null or not an integer

        int id = (int)value;
        return id != 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class ConvertDateToContent : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || !(value is DateTime))
            return "Not Set";

        DateTime date = (DateTime)value;
        return date != DateTime.MinValue ? "Set" : "Not Set";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class ConvertDateStatusToVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return true;
        if (Tools.CheckStatusProject() == BO.StatusProject.Mid)
            return true;
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class IsDependToCheckConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {

        TaskInList taskInList = (TaskInList)values[0];
        List<TaskInList> dependList = (List<TaskInList>)values[1];
        if (taskInList is null || dependList is null)
            return false;
        return dependList.Any(x => x.Id == taskInList.Id);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        return null!;
    }
}
public class TaskWidthConverterA : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        // Assuming value is the Task object
        if (value is BO.Task task)
        {

            task = s_bl.Task.Read(task.Id)!;
            if (task.StartDate == null)
            {

                // Calculate the width of the rectangle based on task duration
                // You need to implement the logic for calculating task duration
                TimeSpan? duration = (task.ScheduledDate!.Value - s_bl.GetProjectStartDate());
                if (duration.HasValue)
                {
                    int durationInHours = (int)duration.Value.TotalHours;
                    return durationInHours;
                }
            }
            // Return the calculated width
            else
            {
                TimeSpan? duration = (task.StartDate!.Value-s_bl.GetProjectStartDate()) ;
                if (duration.HasValue)
                {
                    int durationInHours = (int)duration.Value.TotalHours;
                    return durationInHours;
                }
            }

        }

        return 0; // Return default width if value is not a Task object
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class TaskWidthConverterB : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        // Assuming value is the Task object
        if (value is BO.Task task)
        {

            task = s_bl.Task.Read(task.Id)!;
            //if (task.StartDate == null)
            //{

                // Calculate the width of the rectangle based on task duration
                // You need to implement the logic for calculating task duration
                TimeSpan? duration = (/*(task.ScheduledDate!.Value -task.ScheduledDate) +*/ task.RequiredEffortTime);
                if (duration.HasValue)
                {
                    int durationInHours = (int)duration.Value.TotalHours;
                    return durationInHours;
                }
            //}
            // Return the calculated width
            //else
            //{
            //    TimeSpan? duration =(/*(task.StartDate!.Value -task.StartDate) +*/ task.RequiredEffortTime);
            //    if (duration.HasValue)
            //    {
            //        int durationInHours = (int)duration.Value.TotalHours;
            //        return durationInHours;
            //    }
            //}
                
        }

        return 0; // Return default width if value is not a Task object
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
public class TaskWidthConverterC : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        // Assuming value is the Task object
        if (value is BO.Task task)
        {
            Debug.WriteLine($"TaskWidthConverter: Converting task with ID={task.Id}");

            task = s_bl.Task.Read(task.Id) ?? new();
            //if (task.CompleteDate == null)
            //{
            //    if (task.StartDate == null)
            //    {
            //        TimeSpan? duration = (task.ForecastDate!.Value - (task.ScheduledDate + task.RequiredEffortTime));
            //        if (duration.HasValue)
            //        {
            //            int durationInHours = (int)duration.Value.TotalHours;
            //            return durationInHours;
            //        }
            //    }
            //    else
            //    {
            //        TimeSpan? duration = (task.ForecastDate!.Value - (task.StartDate + task.RequiredEffortTime));
            //        if (duration.HasValue)
            //        {
            //            int durationInHours = (int)duration.Value.TotalHours;
            //            return durationInHours;
            //        }
            //    }
            //}
            //else 
            //{
                if (task.StartDate == null)
                {
                TimeSpan? duration = DateTime.Now- (task.ScheduledDate + task.RequiredEffortTime);

                if (duration.HasValue)
                    {
                        int durationInHours = (int)duration.Value.TotalHours;
                        return durationInHours;
                    }
                }
                else
                {
                    TimeSpan? duration = DateTime.Now - (task.StartDate + task.RequiredEffortTime);
                    if (duration.HasValue)
                    {
                        int durationInHours = (int)duration.Value.TotalHours;
                        return durationInHours;
                    }
                }
            //}
        }

        return 0; // Return default width if value is not a Task object
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


public class TaskColorConverter : IValueConverter
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is BO.Task task)
        {
            task = s_bl.Task.Read(task.Id) ?? new();

            Debug.WriteLine($"TaskColorConverter: Converting task with ID={task.Id}");

            // Check if the task is completed
            if (task.Status == BO.Status.Done)
            {
                return Brushes.Green; // Task completed, so green color
            }
            // Check if the task is overdue
            else if (task.ForecastDate < DateTime.Today)
            {
                return Brushes.Red; // Task overdue, so red color
            }
            else
            {
                // Task is ongoing, apply logic to determine if it's delayed based on start time and dependencies
                // Example: Check if the task start time is later than expected, and if its dependencies are completed
                // If delayed, return red, else return blue
                if (task.StartDate > task.ScheduledDate || !task.Dependencies!.All(dep => dep.Status == BO.Status.Done))
                {
                    return Brushes.Red; // Task delayed, so red color
                }
                else
                {
                    return Brushes.Blue; // Task ongoing, so blue color
                }
            }
        }
        // Default color if value is not a Task or other criteria not met
        return Brushes.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}




