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
        try
        {
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
            throw new BlAlreadyExistException($"Failed to create task. {ex.Message}");
        }
       
    }

    public void Delete(int id)
    {
        // Check if the task exists
        var task = Read(id);
        if (task == null)
        {
            throw new Exception($"Task with ID={id} does not exist");
        }

        // Check if the task has dependencies
        if (task.Dependencies != null && task.Dependencies.Any())
        {
            throw new Exception($"Task with ID={id} has dependencies and cannot be deleted");
        }

        // Check if the task has been scheduled
        if (task.Status != BO.Status.Unscheduled)
        {
            throw new Exception($"Task with ID={id} has been scheduled and cannot be deleted");
        }

        // Attempt to delete the task
        try
        {
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to delete task with ID={id}. {ex.Message}");
        }
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
            //Status=(BO.Status)doTask.Copmlexity!,
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
                Copmlexity = (DO.EngineerExperience)item.Copmlexity
            };

            _dal.Task.Update(taskToUpdate);
        }
        catch (Exception ex)
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
            throw new Exception($"Task with ID={id} does not exist");
        }

        // Check if the scheduled start date for the task is not earlier than the end dates of its preceding tasks
        if (task.Dependencies != null && task.Dependencies.Any())
        {
            var precedingTasks = task.Dependencies.Select(dependency => Read(dependency.Id)).ToList();
            if (precedingTasks.Any(t => t!.CompleteDate != null && startDate < t.CompleteDate))
            {
                throw new Exception($"Scheduled start date cannot be earlier than the end dates of preceding tasks");
            }
        }

        // Update or add the scheduled start date for the task
        task.StartDate = startDate;

        // Attempt to update the task in the data layer
        try
        {
            Update(task);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to update scheduled start date for task with ID={id}. {ex.Message}");
        }
    }
}

