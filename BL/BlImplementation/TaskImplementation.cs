namespace BlImplementation;
using BlApi;
using BO;
using System;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(Task? item)
    {
        // Check if the task object is null
        if (item == null)
        {
            throw new Exception("Task object cannot be null");
        }
        // Check the validity of the task's data
        if (item.Id <= 0)
        {
            throw new Exception("Task ID must be a positive number");
        }
        if (!string.IsNullOrEmpty(item?.Alias))
        {
            throw new Exception("Task alias cannot be empty");
        }

        try
        {
            // Add dependencies for previous tasks from the existing task list
            List<TaskInList> dependencies = new List<TaskInList>();
            if (item?.Dependencies != null)
            {
                foreach (var dependency in item.Dependencies)
                {
                    var existingDependency = _dal.Task.Read(dependency.Id);
                    if (existingDependency == null)
                    {
                        throw new Exception($"Dependency with ID={dependency.Id} does not exist");
                    }
                    dependencies.Add(dependency);
                }
            }

            // Create a new DO.Task object
            DO.Task doTask = new DO.Task
            {
                Id = item!.Id,
                Alias = item.Alias,
                Description = item.Description,
                CreatedAtDate = item.CreatedAtDate,
                /*Dependencies = dependencies,*/
                IsMilestone=false,
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
            int taskId = _dal.Task.Create(doTask);
            return taskId;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new Exception($"Failed to create task. {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to create task. {ex.Message}");
        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new Exception($"Task with ID={id} does Not exist");

        return new BO.Task()
        {
            Id = id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            CreatedAtDate = doTask.DeadlineDate,
            /*Status=(BO.Status)doTask.Copmlexity!,*/
            /*Dependencies=*/
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            /*ForecastDate=*/
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask?.CompleteDate,
            Deliverables = doTask?.Deliverables,
            Remarks = doTask?.Remarks,
            /*Engineer=*/
            Copmlexity = (BO.EngineerExperience)doTask?.Copmlexity!,
        };
    }

    public IEnumerable<TaskInList> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        return (from DO.Task doTask in _dal.Task.ReadAll((Func<DO.Task, bool>?)filter)
                select new BO.TaskInList
                {
                    Id = doTask.Id,
                    Description = doTask.Description,
                    Alias = doTask.Alias,
                    Status= (BO.Status)doTask.Copmlexity!
                });
    }

    public void Update(Task item)
    {
        throw new NotImplementedException();
    }

    public void UpdateStartDate(int id, DateTime startDate)
    {
        throw new NotImplementedException();
    }
}

