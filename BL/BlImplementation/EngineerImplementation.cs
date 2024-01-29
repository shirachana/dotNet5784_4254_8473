namespace BlImplementation;
using BlApi;
using BO;
using DO;
using System.Collections.Generic;
using System.Reflection.Emit;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer? item)
    {
        DO.Engineer doEngineer = new DO.Engineer
        (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);

        
            if (item.Id < 0)
                throw new Exception("Engineer ID must be a positive number");
            if (!string.IsNullOrEmpty(item.Name))
                throw new Exception("Engineer name cannot be empty");
            if (item.Cost < 0)
                throw new Exception("Engineer cost must be a positive number");
            if (!string.IsNullOrEmpty(item.Email))
                throw new Exception("Engineer email cannot be empty");
        try
        {
        int idEng = _dal.Engineer.Create(doEngineer);
        return idEng;
           
        }
        catch(DO.DalAlreadyExistsException ex)
        {
            throw new BlAlreadyExistException($"Student with ID={item.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        
       BO.Engineer engineerToDelete = Read(id)!;
        if(engineerToDelete.Task?.Id is int idTask)
        { 
            var engineerTask =_dal.Task.Read(idTask);
            if (idTask is not 0)
            {
                if (getStatus(engineerTask!) == BO.Status.OnTrack || getStatus(engineerTask!) == BO.Status.Done)
                    throw new Exception($"Engineer with ID={id} can't be deleted");
                _dal.Task.Update(engineerTask! with { Id = 0 });
            }
        }
        if (engineerToDelete == null)
        {
            throw new Exception($"Engineer with ID={id} does not exist");
        }
        
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to delete engineer with ID={id}", ex);
        }
    }
    public static BO.Status getStatus(DO.Task delTask)
    {
        var dateTimeNow=DateTime.Now;
        if(dateTimeNow> delTask.CompleteDate)//done with doing the task
            return Status.Done;
        if (dateTimeNow < delTask.CompleteDate)
            return Status.OnTrack;
        return Status.Unscheduled;
    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new Exception($"Engineer with ID={id} does Not exist");

        return new BO.Engineer()
        {
            Id = id,
            Cost = doEngineer.Cost,
            Email = doEngineer.Email,
            Name = doEngineer.Name,
            Level = (BO.EngineerExperience?)(doEngineer.Level),
            Task = (BO.TaskInEngineer)(_dal.Task)
        };

    }

    public IEnumerable<EngineerInTask> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll((Func<DO.Engineer, bool>?)filter)
                select new BO.EngineerInTask
                {
                  Id= doEngineer.Id,
                  Name= doEngineer.Name,
                });
    }

    public void Update(BO.Engineer item)
    {
        //    DO.Engineer doEngineer = new DO.Engineer(item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);
        //    try
        //    {
        //        if (item?.Id < 0)
        //            throw new Exception("Engineer ID must be a positive number");
        //        if (!string.IsNullOrEmpty(item?.Name))
        //            throw new Exception("Engineer name cannot be empty");
        //        if (item?.Cost < 0)
        //            throw new Exception("Engineer cost must be a positive number");
        //        if (!string.IsNullOrEmpty(item?.Email))
        //            throw new Exception("Engineer email cannot be empty");
        //        _dal.Engineer.Update(doEngineer);
        //    }
        //    catch (DO.DalDoesNotExistException ex)
        //    {
        //        throw new Exception($"Failed to update engineer with ID={item.Id}. {ex.Message}");
        //    }
        // Check if the engineer object is null
        if (item == null)
        {
            throw new Exception("Engineer object cannot be null");
        }

        if (item.Id <= 0)
        {
            throw new Exception("Engineer ID must be a positive number");
        }
        if (!string.IsNullOrEmpty(item?.Name))
        {
            throw new Exception("Engineer name cannot be empty");
        }
        if (item?.Cost < 0)
        {
            throw new Exception("Engineer cost must be a non-negative number");
        }
        if (!string.IsNullOrEmpty(item?.Email))
        {
            throw new Exception("Engineer email cannot be empty");
        }

       
            DO.Engineer existingEngineer = _dal.Engineer.Read(item!.Id)!;
            if (existingEngineer == null)
            {
                throw new Exception($"Engineer with ID={item.Id} does not exist");
            }

             existingEngineer = new DO.Engineer
            (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);
            if (item.Task != null)
            {
                DO.Task existingTask = _dal.Task.Read(item.Task.Id)!;
                if (existingTask == null)
                {
                    throw new Exception($"Task with ID={item.Task.Id} does not exist");
                }

                existingTask = new DO.Task(item.Task.Id,item.Id,item.Task.Alias);
              
                _dal.Task.Update(existingTask);
            }
        try
        {
            _dal.Engineer.Update(existingEngineer);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to update engineer. {ex.Message}");
        }
    }
}

