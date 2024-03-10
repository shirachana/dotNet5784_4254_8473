namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private readonly Bl _bl;
    internal TaskImplementation(Bl bl) => _bl = bl;
    /// <summary>
    /// Adding a task will receive (as a parameter) an object of a task will check for correctness will add dependencies
    /// for previous tasks from the existing task list if the data is correct - 
    /// will attempt to add requests to the data layer will throw its own appropriate exception if adding a task failed.
    /// </summary>
    /// <param name="item"></param>the task to add
    /// <returns></returns>
    /// <exception cref="BlAlreadyExistException"></exception>
    /// <exception cref="BlNullPropertyException"></exception>
    public int Create(Task? item)
    {

        try
        {
            if (item != null && Tools.IsStatusProject(item!) && Tools.CheckTaskHealth(item!))
            {

                DO.Task doTask = new DO.Task
                {
                    Alias = item!.Alias,
                    Description = item.Description,
                    CreatedAtDate = _bl.Clock,
                    RequiredEffortTime = item.RequiredEffortTime,
                    Deliverables = item.Deliverables,
                    Remarks = item.Remarks,
                    Copmlexity = (DO.EngineerExperience)item.Copmlexity
                };
                int IdT = _dal.Task.Create(doTask);

                item!.Dependencies!
              .ForEach(dependency => _dal.Dependency.Create(new DO.Dependency(0, IdT, dependency.Id)));

                return IdT;
            }
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistException($"Failed to create task. {ex.Message}");
        }

        throw new BlNullPropertyException($"Failed to create task");
    }
    /// <summary>
    /// Deleting a task, you will receive a task ID. We will check that the task can be deleted, 
    /// that is, the task exists or the task does not precede other tasks. If the check is successful - 
    /// you will make an attempt to request a deletion from the other data layer
    /// - you will throw an appropriate exception.
    /// </summary>
    /// <param name="id"></param>the task to delete
    /// <exception cref="BlCantBeDeletedException"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    public void Delete(int id)
    {
        try
        {

            BO.Task? task = Read(id);
            if (Tools.IsStatusProject(task!))
            {

                if (task!.Status == BO.Status.Done || task!.Status == BO.Status.OnTrack)
                {
                    throw new BlCantBeDeletedException($"Task with ID={id} cannot be deleted");
                }
                bool canDelete = _dal.Dependency.ReadAll(x => x.DependsOnTask == task.Id).Any();

                if (canDelete == false)
                    _dal.Task.Delete(id);
                else
                    throw new BlCantBeDeletedException($"Task with ID={id} has dependencies and cannot be deleted");
            }
        }

        catch (DO.DalDeleteionImpossible ex)
        {
            throw new BlDoesNotExistException($"Failed to delete task with ID={id}. {ex.Message}");
        }

    }
    /// <summary>
    /// A request for task details will receive a task ID will build a task object
    /// based on the data received from the data layer (DO), and the calculation of 
    /// the additional information will return a constructed task object (logical entity) 
    /// will throw its own appropriate exception if the request fails
    /// </summary>
    /// <param name="id"></param>the tasks id to read
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    public Task? Read(int id)
    {
        try
        {
            DO.Task? doTask = _dal.Task.Read(id);
            if (doTask is null)
                throw new BlDoesNotExistException($"Task with ID={id} does Not exist");
            EngineerInTask? engineerInTask = CalculateEngineerForTask(doTask.Engineerld);
            var dependencies = from item in _dal.Dependency.ReadAll()
                               where item.DependentTask == id
                               group item by item.DependentTask into g
                               select new { DepOnTask = g.Select(x => x.DependsOnTask) };
            var depIds = dependencies.Select(x => x.DepOnTask).FirstOrDefault();
            Status status = Tools.GetStatus(doTask);
            return new BO.Task()
            {
                Id = id,
                Description = doTask.Description,
                Alias = doTask.Alias,
                CreatedAtDate = doTask.CreatedAtDate,
                Status = status,
                RequiredEffortTime = doTask.RequiredEffortTime,
                StartDate = doTask.StartDate,
                ScheduledDate = doTask.ScheduledDate,
                ForecastDate = doTask.StartDate > doTask.ScheduledDate ?
                    doTask.StartDate + doTask.RequiredEffortTime :
                    doTask.ScheduledDate + doTask.RequiredEffortTime,
                CompleteDate = doTask?.CompleteDate,
                Deliverables = doTask?.Deliverables,
                Remarks = doTask?.Remarks,
                Engineer = engineerInTask,
                Copmlexity = (BO.EngineerExperience)doTask?.Copmlexity!,
                Dependencies = (from dep in dependencies
                                from taskId in dep.DepOnTask
                                let task = _dal.Task.Read(taskId)
                                select new TaskInList()
                                {
                                    Id = task.Id,
                                    Alias = task.Alias,
                                    Description = task.Description,
                                    Status = status
                                }).ToList(),
            };
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to Read Task. {ex.Message}");
        }
    }
    /// <summary>
    /// Task list request Request the list of tasks from the data layer.
    /// </summary>
    /// <param name="filter"></param>the filter if there
    /// <returns></returns>
    public IEnumerable<TaskInList> ReadAll(Func<BO.TaskInList, bool>? filter = null)
    {
        var tasks = _dal.Task.ReadAll()
                             .OrderBy(doTask => doTask.Id)
                             .Select(doTask => new BO.TaskInList
                             {
                                 Id = doTask.Id,
                                 Description = doTask.Description,
                                 Alias = doTask.Alias,
                                 Status = Tools.GetStatus(doTask)
                             }).Where(task => filter is null ? true : filter(task));

        return tasks;
    }
    /// <summary>
    /// Updating a task We will receive a task object for updating. 
    /// We will check the integrity of the data: if the test was successful
    /// - you will attempt an update request to the data layer will throw its own appropriate exception.
    /// If the update fails in the planning phase, you can update the details of the task,
    /// such as the required duration, level of difficulty, adding/deleting dependencies on another task, etc. After creating a schedule,
    /// only the textual fields and the engineer assigned to the task can be changed.
    /// </summary>
    /// <param name="item"></param>the task to update
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BlArgumentOutOfRangeException"></exception>
    public void Update(Task? item)
    {
        try
        {
            Tools.CheckTaskHealth(item!);
            DO.Task task = _dal.Task.Read(item!.Id) ?? throw new BlDoesNotExistException("the task dosnt exist");

            if (Tools.CheckStatusProject() != BO.StatusProject.Execution)
            {
                task = task with
                {
                    Id = item!.Id,
                    Alias = item.Alias,
                    Description = item.Description,
                    CreatedAtDate = item.CreatedAtDate,
                    IsMilestone = false,
                    RequiredEffortTime = item.RequiredEffortTime,
                    Deliverables = item.Deliverables,
                    Remarks = item.Remarks,
                    Copmlexity = (DO.EngineerExperience)item.Copmlexity,

                };

                _dal.Task.Update(task);

                if (item.Dependencies is null)
                    item.Dependencies = new();

                IEnumerable<DO.Dependency> dependencies = _dal.Dependency.ReadAll(x => x.DependentTask == item.Id);

                item.Dependencies.Where(_new => !dependencies.Any(old => old.DependsOnTask == _new.Id)).
                    ToList().ForEach(_newItem => _dal.Dependency.Create(new DO.Dependency()
                    {
                        DependentTask = item.Id,
                        DependsOnTask = _newItem.Id
                    }));

                dependencies.Where(old => !item.Dependencies.Any(_new => _new.Id == old.DependsOnTask)).
                 ToList().ForEach(old => _dal.Dependency.Delete(old.Id));

            }
            else
            {
                DO.Task? dtask = _dal.Task.Read(item!.Id);
                if (Tools.CheckStatusProject() != BO.StatusProject.Execution && item?.Engineer?.Id != null)
                    throw new BlArgumentOutOfRangeException("You cannot add Engineer to a task when the program is in the middle of planning");
                DO.Engineer? eng = _dal.Engineer.Read(item!.Engineer!.Id);
                if ((int)eng!.Level! < (int)item!.Copmlexity)
                    throw new BlArgumentOutOfRangeException("The level of the Task dont match to the Engineer level");
                if (item.Engineer.Id != 0 && item.Engineer.Id != eng.Id)
                    throw new BlArgumentOutOfRangeException("there is another Engineer that work on this task");
                dtask = dtask! with
                {
                    Engineerld = item.Engineer!.Id,
                    Alias = item.Alias,
                    Description = item.Description,
                    Deliverables = item.Deliverables,
                    Remarks = item.Remarks
                };

                _dal.Task.Update(dtask!);

            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlArgumentOutOfRangeException($"Failed to update task with ID={item!.Id}. {ex.Message}");
        }

    }
    /// <summary>
    /// Updating or adding a task's scheduled start date will give you a task ID 
    /// and dateThe action will check if all the scheduled start dates of the tasks 
    /// preceding it are already updated(exist), otherwise it will throw an appropriate
    /// exception after that, the action will check that the date received as a parameter
    /// is not earlier than all the estimated end dates of all the tasks preceding it.
    /// Otherwise an exception will be thrown.If the data is correct -
    /// you will make update requests to the data layer
    /// </summary>
    /// <param name="id"></param>id of the task to update
    /// <param name="startDate"></param>the date
    /// <exception cref="BlArgumentOutOfRangeException"></exception>
    public void UpdateStartDate(int id, DateTime? startDate)
    {
        try
        {
            if (Tools.CheckStatusProject() == StatusProject.Mid)
            {
                Task task = Read(id)!;

                if (task?.Dependencies != null && task.Dependencies.Any())
                {
                    var precedingTasks = task.Dependencies.Select(dependency => Read(dependency.Id)).ToList();
                    if (precedingTasks.Any(t => t!.StartDate == null || t!.ForecastDate < startDate))
                    {
                        throw new BlArgumentOutOfRangeException($"Cannot update start date. All preceding tasks must have their start dates updated.");
                    }
                }
                DO.Task dtask = _dal.Task.Read(id)!;
                if (_dal.getStart() < startDate)
                {
                    _dal.Task.Update(dtask with { StartDate = startDate });
                }
                else { throw new BlArgumentOutOfRangeException($"This start day is before the start of the project"); }
            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlArgumentOutOfRangeException($"Failed to update scheduled start date for task with ID={id}. {ex.Message}");
        }
    }
    /// <summary>
    /// claer tha data of the task and the dependency
    /// </summary>
    public void Clear()
    {
        _dal.Task.Clear();
        _dal.Dependency.Clear();
    }
    /// <summary>
    /// A helper function to calculate a worker for a task receives an engineer ID and searches for his task
    /// </summary>
    /// <param name="engineerId"></param>the engineeers id
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public EngineerInTask? CalculateEngineerForTask(int engineerId)
    {
        try
        {
            DO.Engineer? eng = _dal.Engineer.Read(engineerId) ?? throw new BlDoesNotExistException($"There is no Engineer in the task");

            if (eng == null)
            {

                return null;
            }

            return new EngineerInTask()
            {
                Id = eng.Id,
                Name = eng.Name
            };
        }
        catch (DO.DalDoesNotExistException)
        {
            return null;
        }
    }
    public IEnumerable<BO.TaskInList> TasksForEngineer(BO.Engineer engineer)
    {

        DO.Engineer eng = _dal.Engineer.Read(engineer.Id) ?? throw new BlDoesNotExistException($"The Engineer with the id={engineer.Id} does not exist");
        var tasks = _dal.Task.ReadAll()
                        .OrderBy(doTask => doTask.Id)
                        .Where(doTask =>
                        {
                            return doTask.Copmlexity <= eng.Level && (doTask.Engineerld == 0 || doTask.Engineerld == eng.Id) && CanAdd(doTask) == true && doTask.CompleteDate == null;
                        })
                        .Select(doTask => new BO.TaskInList
                        {
                            Id = doTask.Id,
                            Description = doTask.Description,
                            Alias = doTask.Alias,
                            Status = Tools.GetStatus(doTask)
                        });

        return tasks;
    }
    public IEnumerable<BO.TaskInList> GetDependencies(DO.Task task)
    {
        var result = (from DO.Dependency dep in _dal.Dependency.ReadAll()
                      where task.Id == dep.DependentTask && _dal.Task.Read(dep.DependsOnTask) != null
                      select new BO.TaskInList
                      {
                          Id = dep.DependsOnTask,
                          Alias = _dal.Task.Read(dep.DependsOnTask)!.Alias,
                          Description = _dal.Task.Read(dep.DependsOnTask)!.Description,
                          Status = Tools.GetStatus(task)
                      });
        return result;

    }
    private bool CanAdd(DO.Task tsk)
    {
        var dependentTasks = (from dependency in _dal.Dependency.ReadAll()
                              where dependency.DependentTask == tsk.Id
                              select _dal.Task.Read(dependency.DependsOnTask))
                              .ToList();
        dependentTasks.Where(task => (task.StartDate + task.RequiredEffortTime) < _bl.Clock);

        if (dependentTasks.Any())
            return false;
        return true;
    }

    public void UpdateCompleteDate(int id, DateTime completeDate)
    {
        try
        {
            Task task = Read(id)!;
            if ( task.StartDate != null)
            {

                DO.Task dtask = _dal.Task.Read(id)!;
    
                _dal.Task.Update(dtask with { CompleteDate = completeDate });
            }
            else { throw new BlArgumentOutOfRangeException($"This complete Date is before the forcastDay of the project or there is no Start Date to the task."); }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlArgumentOutOfRangeException($"Failed to update  complete date for task with ID={id}. {ex.Message}");
        }
    }
    public IEnumerable<BO.GanttDetails> GetDetailsToGantt(Func<BO.GanttDetails, bool>? filter = null)
    {
        return (from task in ReadAll()
                let t = _dal.Task.Read(task.Id)
                let start = (int)(t.ScheduledDate - _bl.GetProjectStartDate())!.Value.TotalHours
                select new BO.GanttDetails()
                {
                    ID = task.Id,
                    Name = task.Alias,
                    TasksDays = (int)t.RequiredEffortTime!.Value.TotalHours * 2,
                    StartOffset = start * 2,
                    EndOffset = (int)(start + t.RequiredEffortTime.Value.TotalHours) * 2,
                    Dependencies = StringDependencies(task.Id),
                    Status = task.Status,
                }).ToList().OrderBy(x => x.StartOffset);
    }

    private string StringDependencies(int id)
    {
        var x = _dal.Dependency.ReadAll(x => x.DependentTask == id)
                                    .Where(dependency => dependency.DependentTask == id)
                                    .Select(dependency => dependency.DependsOnTask.ToString() + " ");
        string dep = "";
        foreach (string tmp in x)
        {
            dep += tmp.ToString();
        }
        return dep;
    }

}

