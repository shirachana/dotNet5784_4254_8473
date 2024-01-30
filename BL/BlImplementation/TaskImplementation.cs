namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(Task? item)
    {
        try
        {
              if (Tools.IsProjectStartDateSet(item!)&&Tools.CheckTaskHealth(item!))
              {
                List<TaskInList> dependencies = item!.Dependencies!
               .Select(dependency => _dal.Task.Read(dependency.Id))
               .Where(existingDependency => existingDependency != null)
               .Select(existingDependency =>
                {
                    // Call getStatus outside the LINQ query
                    Status status = Tools.GetStatus(existingDependency!);

                    // Create TaskInList using the mapped properties and the status obtained from getStatus
                    return new TaskInList
                    {
                        Id = existingDependency!.Id,
                        Description = existingDependency.Description,
                        Alias = existingDependency.Alias,
                        Status = status,
                    };
                }).ToList();
                if (dependencies.Count != item?.Dependencies?.Count)
                {
                    throw new BlDoesNotExistException("One or more dependencies do not exist");
                }
                // Create a new DO.Task object
                DO.Task doTask = new DO.Task
                {
                    Id = item!.Id,
                    Alias = item.Alias,
                    Description = item.Description,
                    CreatedAtDate = item.CreatedAtDate,
                    /*Dependencies = dependencies,*/
                    IsMilestone = false,
                    RequiredEffortTime = item.RequiredEffortTime,
                    StartDate = item.StartDate,
                    ScheduledDate = item.ScheduledDate,
                    DeadlineDate = item.DeadlineDate,
                    CompleteDate = item.CompleteDate,
                    Deliverables = item.Deliverables,
                    Remarks = item.Remarks,
                    Engineerld = item.Engineer!.Id, // Assuming EngineerId is the ID of the assigned engineer
                    Copmlexity = (DO.EngineerExperience)item.Copmlexity
                };

                // Attempt to add the task to the data layer
                _dal.Task.Create(doTask);
              }
        }
        catch (DO.DalAlreadyExistsException ex)
        {
         throw new BlAlreadyExistException($"Failed to create task. {ex.Message}");
        }
        

    }

    public void Delete(int id)
    {
        // Check if the task exists
        try
        {

            BO.Task? task = Read(id);
            if (Tools.IsProjectStartDateSet(task!))
            {

                if (task == null)
                {
                    throw new BlDoesNotExistException($"Task with ID={id} does not exist");
                }

                // Check if the task has dependencies
                if (task.Dependencies != null && task.Dependencies.Any())
                {
                    throw new BlCantBeDeletedException($"Task with ID={id} has dependencies and cannot be deleted");
                }

                // Check if the task has been scheduled
                if (task.Status != BO.Status.Unscheduled)
                {
                    throw new BlCantBeDeletedException($"Task with ID={id} has been scheduled and cannot be deleted");
                }

                bool canDelete = _dal.Dependency.ReadAll(x => x.DependsOnTask == task.Id).Any();

                // Attempt to delete the task
                if (!canDelete)
                    _dal.Task.Delete(id);
                throw new Exception();
            }
        }

        catch (DO.DalDeleteionImpossible ex)
        {
            throw new BlDoesNotExistException($"Failed to delete task with ID={id}. {ex.Message}");
        }

    }

    public Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        DO.Engineer? eng = _dal.Engineer.Read(doTask!.Engineerld);
        if (doTask is null || eng is null)
            throw new Exception($"Task with ID={id} does Not exist");

        var dependencies = from item in _dal.Dependency.ReadAll()
                           where item.DependentTask==id
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

            /*Dependencies=*/
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            /*ForecastDate=*/
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask?.CompleteDate,
            Deliverables = doTask?.Deliverables,
            Remarks = doTask?.Remarks,
            Engineer = new EngineerInTask()
            {
                Id = eng.Id,
                Name = eng.Name
            },
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
            if (Tools.IsProjectStartDateSet(item!) && Tools.CheckTaskHealth(item!))
            {
                if (item?.StartDate <= DateTime.MinValue)
                {

                    // Update the task in the data layer
                    var taskToUpdate = new DO.Task
                    {
                        Id = item!.Id,
                        Alias = item.Alias,
                        Description = item.Description,
                        CreatedAtDate = item.CreatedAtDate,
                        /*Dependencies = dependencies,*/
                        IsMilestone = false,
                        RequiredEffortTime = item.RequiredEffortTime,
                        StartDate = item.StartDate,
                        ScheduledDate = item.ScheduledDate,
                        DeadlineDate = item.DeadlineDate,
                        CompleteDate = item.CompleteDate,
                        Deliverables = item.Deliverables,
                        Remarks = item.Remarks,
                        Engineerld = item.Engineer!.Id, // Assuming EngineerId is the ID of the assigned engineer
                        Copmlexity = (DO.EngineerExperience)item.Copmlexity,

                    };

                    _dal.Task.Update(taskToUpdate);
                }
                else
                {
                    DO.Task? dtask = _dal.Task.Read(item!.Id);
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
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new Exception($"Failed to update task with ID={item!.Id}. {ex.Message}");
        }
        
    }

    public void UpdateStartDate(int id, DateTime startDate)
    {
        // Check if the task with the given ID exists
        var task = Read(id);
        if (task == null)
        {
            throw new BlDoesNotExistException($"Task with ID={id} does not exist");
        }

        // Check if the scheduled start date for the task is not earlier than the end dates of its preceding tasks
        if (task.Dependencies != null && task.Dependencies.Any())
        {
            var precedingTasks = task.Dependencies.Select(dependency => Read(dependency.Id)).ToList();
            if (precedingTasks.Any(t => t!.CompleteDate != null && startDate < t.CompleteDate))
            {
                throw new BlArgumentOutOfRangeException($"Scheduled start date cannot be earlier than the end dates of preceding tasks");
            }
        }

        // Update or add the scheduled start date for the task
        task.StartDate = startDate;

        // Attempt to update the task in the data layer
        try
        {
            Update(task);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new Exception($"Failed to update scheduled start date for task with ID={id}. {ex.Message}");
        }
    }
}

