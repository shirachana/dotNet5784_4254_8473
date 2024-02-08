namespace BlImplementation;
using BlApi;
using BO;
using DalApi;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

internal class TaskImplementation : BlApi.ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(Task? item)
    {

        try
        {

            if (Tools.IsStatusProject(item!) && Tools.CheckTaskHealth(item!))
            {
                // Create a new DO.Task object

                DO.Task doTask = new DO.Task
                {
                    Alias = item!.Alias,
                    Description = item.Description,
                    CreatedAtDate = item.CreatedAtDate,
                    RequiredEffortTime = item.RequiredEffortTime,
                    Deliverables = item.Deliverables,
                    Remarks = item.Remarks,
                    Copmlexity = (DO.EngineerExperience)item.Copmlexity
                };
                int IdT = _dal.Task.Create(doTask);

                // Attempt to add the task to the data layer
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
    public void Delete(int id)
    {
        // Check if the task exists
        try
        {

            BO.Task? task = Read(id);
            if (Tools.IsStatusProject(task!))
            {
               
                if (task!.Status == BO.Status.Done|| task!.Status == BO.Status.OnTrack)
                {
                    throw new BlCantBeDeletedException($"Task with ID={id} cannot be deleted");
                }
                bool canDelete = _dal.Dependency.ReadAll(x => x.DependsOnTask == task.Id).Any();

                // Attempt to delete the task Check if the task has dependencies
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

    public Task? Read(int id)
    {
        try
        {
            DO.Task? doTask = _dal.Task.Read(id);
            if (doTask is null)
                throw new Exception($"Task with ID={id} does Not exist");
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
                CreatedAtDate = doTask.DeadlineDate,
                Status = status,
                RequiredEffortTime = doTask.RequiredEffortTime,
                StartDate = doTask.StartDate,
                ScheduledDate = doTask.ScheduledDate,
                ForecastDate = doTask.StartDate > doTask.ScheduledDate ?
                    doTask.StartDate + doTask.RequiredEffortTime :
                    doTask.ScheduledDate + doTask.RequiredEffortTime,
                DeadlineDate = doTask.DeadlineDate,
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

    public IEnumerable<TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        return from DO.Task doTask in _dal.Task.ReadAll((Func<DO.Task, bool>?)filter)
               select new BO.TaskInList
               {
                   Id = doTask.Id,
                   Description = doTask.Description,
                   Alias = doTask.Alias,
                   Status = Tools.GetStatus(doTask)
               };
    }

    public void Update(Task? item)
    {
        try
        {
            Tools.CheckTaskHealth(item!);
            DO.Task task = _dal.Task.Read(item!.Id) ?? throw new BlDoesNotExistException("the task dosnt exist");

            if (_dal.GetStartDay() is null && item?.StartDate is null)
            {

                /*DO.Engineer? eng = _dal.Engineer.Read(item!.Engineer!.Id);*///to check if the id of the engieer is exist already
                                                                          // Update the task in the data layer
                task = task with
                {
                    Id = item!.Id,
                    Alias = item.Alias,
                    Description = item.Description,
                    CreatedAtDate = item.CreatedAtDate,
                    IsMilestone = false,
                    RequiredEffortTime = item.RequiredEffortTime,
                    //ScheduledDate = item.ScheduledDate,// יש פונקציה אחרת שעושה את העדכון
                    //DeadlineDate = item.DeadlineDate > item.ForecastDate ? item.DeadlineDate : throw new Exception(),
                    //CompleteDate = item.CompleteDate,// יש לבדוק אם זה שייך למסך של מהנדס
                    Deliverables = item.Deliverables,
                    Remarks = item.Remarks,
                    //Engineerld = item.Engineer!.Id, // Assuming EngineerId is the ID of the assigned engineer
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
                DO.Engineer? eng = _dal.Engineer.Read(item.Engineer!.Id);//to check if the id of the engieer is exist already
                DO.Task? dtask = _dal.Task.Read(item!.Id);
                //if (Tools.CheckStatusProject() != BO.StatusProject.Execution)
                //    throw new BlArgumentOutOfRangeException("You cannot add Engineer to a task when the program is in the middle of planning");
                if ((int)eng!.Level! < (int)item!.Copmlexity)
                    throw new BlArgumentOutOfRangeException("The level of the Task dont match to the Engineer level");
                if (item.Engineer.Id != 0 && item.Engineer.Id != eng.Id)
                    throw new BlArgumentOutOfRangeException("there is another Engineer that work on this task");
                IEnumerable<DO.Task> tasksDep =
                  from DO.Dependency d in _dal.Dependency.ReadAll()
                  where d.DependentTask == item.Id
                  select _dal.Task.Read(d.DependsOnTask);
                tasksDep.Where(p => (p.StartDate + p.RequiredEffortTime)! < DateTime.Today);
                if (tasksDep.Any())
                    throw new BlArgumentOutOfRangeException("The Engineer cannot be registered to the task because the pending task has not yet finished ");
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

    public void UpdateStartDate(int id, DateTime startDate)
    {
        try
        {
            if (Tools.CheckStatusProject() == StatusProject.Mid)
            {
                // Check if the task with the given ID exists
                Task task = Read(id)!;

                // Check if all scheduled start dates of preceding tasks are updated
                if (task?.Dependencies != null && task.Dependencies.Any())
                {
                    var precedingTasks = task.Dependencies.Select(dependency => Read(dependency.Id)).ToList();
                    if (precedingTasks.Any(t => t!.StartDate == null || t!.ForecastDate < startDate))
                    {
                        throw new BlArgumentOutOfRangeException($"Cannot update start date. All preceding tasks must have their start dates updated.");
                    }
                }
                // Update or add the scheduled start date for the task
                // Attempt to update the task in the data layer
                DO.Task dtask = _dal.Task.Read(id)!;
                if (_dal.GetStartDay() <= startDate) 
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

    public void Clear()
    {
        _dal.Task.Clear();
        _dal.Dependency.Clear();
    }

    private EngineerInTask? CalculateEngineerForTask(int engineerId)
    {
        try
        {
            //Console.WriteLine($"Trying to read engineer with ID: {engineerId}");
            DO.Engineer? eng = _dal.Engineer.Read(engineerId)?? throw new BlDoesNotExistException($"There is no Engineer in the task");

            if (eng == null)
            {
                //Console.WriteLine($"Engineer with ID {engineerId} not found");
                 return null;// If engineer doesn't exist, return null
            }

            return new EngineerInTask()
            {
                Id = eng.Id,
                Name = eng.Name
            };
        }
        catch (DO.DalDoesNotExistException )
        {
           //Console.WriteLine($"Error reading engineer: {ex.Message}");
            return null; // If engineer doesn't exist, return null
        }
    }


}

