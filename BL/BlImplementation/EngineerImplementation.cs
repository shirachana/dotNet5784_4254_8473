namespace BlImplementation;
using BlApi;
using BO;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// You will receive an engineer's object, check for correctness if the data is correct - 
    /// make an attempt to request an addition to the data layer will throw its own appropriate exception if adding an engineer failed.
    /// </summary>
    /// <param name="item"></param>the Engineer to creat
    /// <exception cref="BO.BlAlreadyExistException"></exception>
    public void Create(Engineer? item)
    {
        try
        {
            if (Tools.CheckEngineerHealth(item!))//its good
            {
                DO.Engineer doEngineer = new DO.Engineer
                (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);
                _dal.Engineer.Create(doEngineer);
                // Proceed with other operations if the engineer is healthy
            }
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistException($"Student with ID={item!.Id} already exists", ex);
        }
    }
    /// <summary>
    /// Deleting an engineer: You will get an engineer ID Check that the engineer is not on a mission
    ///If the test was successful - you will make an attempt to request deletion from the data layer
    ///Throw its own appropriate exception if the engineer cannot be deleted or if no engineer with such an ID exists at all
    /// </summary>
    /// <param name="id"></param>the id of engineer to delete
    /// <exception cref="BlCantBeDeletedException"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    public void Delete(int id)
    {

        IEnumerable<DO.Task> tsk = from DO.Task task in _dal.Task.ReadAll(task => task.Engineerld == id)//check if there task
                                   where task.StartDate is not null
                                   select task;
        if (tsk.Any())
            throw new BlCantBeDeletedException($"Engineer with ID={id} can't be deleted");
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to delete engineer with ID={id}", ex);
        }
    }
    /// <summary>
    /// Engineer details request: You will receive an engineer ID Build an 
    /// engineer object based on the received data and calculate the additional information (current task) Return a constructed engineer object Throw its own appropriate exception if the request fails.
    /// </summary>
    /// <param name="id"></param>the engineers id to read
    /// <returns></returns>
    /// <exception cref="BlDoesNotExistException"></exception>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        try
        {
            return new BO.Engineer()
            {
                Id = id,
                Cost = doEngineer.Cost,
                Email = doEngineer.Email,
                Name = doEngineer.Name,
                Level = (BO.EngineerExperience?)(doEngineer.Level),
                Task = GetEngineerTask(doEngineer.Id)
            };
        }
        catch (DO.DalDoesNotExistException)
        {
            throw new BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        }
    }
    /// <summary>
    /// Request a list of engineers: request the list of engineers from the data layer
    /// for each engineer, build an appropriate entity and return the built list
    /// </summary>
    /// <param name="filter"></param>the filter to read if there
    /// <returns></returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
            IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();
            if (filter == null)//if there no filter
                return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                        orderby doEngineer.Level
                        select new BO.Engineer()
                        {
                            Id = doEngineer.Id,
                            Name = doEngineer.Name,
                            Cost = doEngineer.Cost,
                            Email = doEngineer.Email,
                            Level = (BO.EngineerExperience?)(doEngineer?.Level),
                            Task = GetEngineerTask(doEngineer.Id) /*new BO.TaskInEngineer
                            {
                                Id = task.Id,
                                Alias = task.Alias,
                            }*/
                        });
            else
                return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                        let boWorker =
                         new BO.Engineer()
                                   {
                                       Id = doEngineer.Id,
                                       Name = doEngineer.Name,
                                       Cost = doEngineer.Cost,
                                       Email = doEngineer.Email,
                                       Level = (BO.EngineerExperience?)(doEngineer?.Level),
                                       Task = GetEngineerTask(doEngineer.Id)/*new BO.TaskInEngineer
                                       {
                                           Id = tsk.Id,
                                           Alias = tsk.Alias,
                                       }*/
                                   }
                        where filter(boWorker)
                        orderby doEngineer.Level
                        select boWorker);
    }
    /// <summary>
    /// Update engineer data You will receive an update request engineer
    /// object to the data layer and will throw its own appropriate exception if the update fails.
    /// </summary>
    /// <param name="item"></param>the engineer to update
    /// <exception cref="BlDoesNotExistException"></exception>
    public void Update(BO.Engineer item)
    {
        try
        {
            if (Tools.CheckEngineerHealth(item!))
            {
                DO.Engineer engineer = _dal.Engineer.Read(item!.Id)! ?? throw new BlDoesNotExistException("The Engineer to update does not Exist");
                if (engineer.Level < (DO.EngineerExperience)item.Level!)
                {
                    throw new BlDoesNotExistException($"Engineer level it's not Upwards ");
                }
                engineer = new DO.Engineer
                (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);

                if (item.Task != null && item.Task.Id > 0)
                {
                    // Retrieve the task from the data storage
                    DO.Task existingTask = _dal.Task.Read(item.Task.Id)!;

                    if (existingTask == null)
                    {
                        throw new BlDoesNotExistException("Task not found");
                    }
                    existingTask = existingTask with { Engineerld = item.Id };
                    _dal.Task.Update(existingTask);
                }
                //DO.Task existingTask = _dal.Task.Read(item.Task!.Id) ?? throw new BlDoesNotExistException("The Task does not Exist")

                _dal.Engineer.Update(engineer);
                
            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to update engineer. {ex.Message}");
        }
    }
    /// <summary>
    /// A separate function for updating a worker to a task receives the worker's 
    /// identity and the task's identity checks for normality (that there are no other workers on this task and the status is normal) 
    /// if it does not return a suitable exception
    /// </summary>
    /// <param name="idEng"></param>the enginneer id
    /// <param name="idTsk"></param>the task id
    /// <exception cref="BlPlanning"></exception>
    /// <exception cref="BlDoesNotExistException"></exception>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void UpdateEngineerToTask(int idEng,int idTsk)
    {
        //if (Tools.CheckStatusProject() != BO.StatusProject.Execution)
        //    throw new BlPlanning("the Engineer cant add task becuse the start project and the task date not update yet");
        try
        {
            DO.Engineer eng = _dal.Engineer.Read(idEng) ?? throw new BlDoesNotExistException($"the Engineer with the id={idEng} does not exist");
            DO.Task tsk = _dal.Task.Read(idTsk) ?? throw new BlDoesNotExistException($"the Task with the id={idTsk} does not exist");
            if (eng.Level < tsk.Copmlexity)
                throw new BlDoesNotExistException("The level of the Task dont match to the Engineer level");
            if (tsk.Engineerld != 0 && tsk.Engineerld != idEng)
                throw new BlDoesNotExistException("there is another Engineer that work on this task");
            IEnumerable<DO.Task> tasksDep =
              from DO.Dependency d in _dal.Dependency.ReadAll()
              where d.DependentTask == idTsk
              select _dal.Task.Read(d.DependsOnTask);
            tasksDep.Where(p => (p.StartDate + p.RequiredEffortTime)! <DateTime.Today);
            if (tasksDep.Any())
                throw new BlDoesNotExistException("The Engineer cannot be registered to the task because the pending task has not yet finished ");
            DO.Task uppTask = new DO.Task(tsk.Id, idEng, tsk.Alias, tsk.Description, tsk.CreatedAtDate, tsk.RequiredEffortTime, tsk.IsMilestone, tsk.Copmlexity, tsk.StartDate, tsk.ScheduledDate, tsk.DeadlineDate, tsk.CompleteDate, tsk.Deliverables, tsk.Remarks);
            _dal.Task.Update(uppTask);
        }
        catch (DO.DalDoesNotExistException ex) { throw new BO.BlDoesNotExistException("cant update the Task in The Engineer", ex); }


    }
    /// <summary>
    /// to clear the data
    /// </summary>

    public void Clear()
    {
        _dal.Engineer.Clear();
    }
    /// <summary>
    /// private function that help to get the engineer task
    /// </summary>
    /// <param name="doEng"></param>the engineer
    /// <returns></returns>
    public BO.TaskInEngineer? GetEngineerTask(int idE)
    {
        try
        {
            DO.Task? tsk = _dal.Task.Read(t => t.Engineerld == idE && t.StartDate != null && t.CompleteDate == null);

            return new BO.TaskInEngineer
            {
                Id = tsk!.Id,
                Alias = tsk.Alias
            };
        }
        catch (DO.DalDoesNotExistException) { return null; }
    }
    public void DeleteEngineer(int idE, int idTk)
    {
        try
        {
            // Find the task associated with the engineer
            DO.Task task = _dal.Task.Read(idTk)!;

            if (task == null)
            {
                throw new BlDoesNotExistException($"Task with ID={idTk} does not exist.");
            }

            // Check if the task hasn't started yet
            if (task.Engineerld == idE && task.StartDate > DateTime.Today)
            {
                // Disassociate the engineer from the task
                DO.Task uppTask = new DO.Task(task.Id, 0, task.Alias, task.Description, task.CreatedAtDate, task.RequiredEffortTime, task.IsMilestone, task.Copmlexity, task.StartDate, task.ScheduledDate, task.DeadlineDate, task.CompleteDate, task.Deliverables, task.Remarks);

                // Update the task in the data access layer
                _dal.Task.Update(uppTask);
            }
            else
            {
                throw new BlCantBeDeletedException($"The Engineer with Task ID={idTk} has already started and can't be deleted.");
            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to delete engineer task with ID={idE}", ex);
        }
    }

}

