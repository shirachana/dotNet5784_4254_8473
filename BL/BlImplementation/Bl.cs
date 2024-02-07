using BlApi;
using BO;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BlImplementation;

internal class Bl : IBl
{
    public IEngineer Engineer => new EngineerImplementation();

    public ITask Task => new TaskImplementation();
    public DateTime? EndDateProject { get; set; } = null;

    //public void ScheduleTasks()
    //{
    //    var tasks = Factory.Get().Task;

    //    foreach (var task in tasks.ReadAll())
    //    {
    //        var currentTask = tasks.Read(task.Id)!;

    //        if (currentTask.Dependencies == null)
    //        {
    //            currentTask.StartDate = IBl.StartDateProject;
    //        }
    //        else
    //        {
    //            var maxDeadlineDate = currentTask.Dependencies.Max(dep => tasks.Read(dep.Id)!.DeadlineDate);
    //            currentTask.StartDate = maxDeadlineDate > currentTask.DeadlineDate ? maxDeadlineDate : IBl.StartDateProject;
    //        }
    //    }
    //}

}



