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
        if (item!.Id < 0)
            throw new BlArgumentOutOfRangeException("Engineer ID must be a positive number");
        if (string.IsNullOrEmpty(item.Name))
            throw new BlArgumentOutOfRangeException("Engineer name cannot be empty");
        if (item.Cost < 0)
            throw new BlArgumentOutOfRangeException("Engineer cost must be a positive number");
        if (string.IsNullOrEmpty(item.Email))
            throw new BlArgumentOutOfRangeException("Engineer email cannot be empty");
        try
        {
            DO.Engineer doEngineer = new DO.Engineer
            (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);
            int idCrt= _dal.Engineer.Create(doEngineer);
            return idCrt;   
        }
        catch(DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistException($"Student with ID={item!.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        
       BO.Engineer engineerToDelete = Read(id)!;
        //if(engineerToDelete.Task?.Id is int idTask)
        //{ 
        //    var engineerTask =_dal.Task.Read(idTask);
        //    if (idTask is not 0)
        //    {
        //        if (getStatus(engineerTask!) == BO.Status.OnTrack || getStatus(engineerTask!) == BO.Status.Done)
        //            throw new Exception($"Engineer with ID={id} can't be deleted");
        //        _dal.Task.Update(engineerTask! with { Id = 0 });
        //    }
        //}  
        if(_dal.Task.ReadAll(e=> e.Engineerld==id).Where(e=>e!.StartDate is not null).Any())
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
    private  BO.Status getStatus(DO.Task delTask)
    {
        var dateTimeNow = DateTime.Now;

        BO.Status status;

        switch (delTask)
        {
            case DO.Task tmp when tmp.Id is 0:
                status = BO.Status.Unscheduled;
                break;

            case DO.Task tmp when tmp.StartDate > dateTimeNow:
                status = BO.Status.Scheduled;
                break;

            case DO.Task tmp when tmp.CompleteDate > dateTimeNow:
                status = BO.Status.OnTrack;
                break;

            default:
                status = BO.Status.Done;
                break;
        }

        return status;
    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        DO.Task? tsk= _dal.Task.ReadAll(t=> t.Engineerld==id).Where(t=> t!.StartDate!= null && t.CompleteDate == null).FirstOrDefault();
        return new BO.Engineer()
        {
            Id = id,
            Cost = doEngineer.Cost,
            Email = doEngineer.Email,
            Name = doEngineer.Name,
            Level = (BO.EngineerExperience?)(doEngineer.Level),
            Task = new BO.EngineerInTask
            {
                Id = tsk!.Id,
                Name = tsk.Alias,
            }
        };

    }

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();
        //IEnumerable<DO.Engineer?> engineers = _dal.Engineer.ReadAll();
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                let tsk=tasks.FirstOrDefault(t => t!.Engineerld== doEngineer.Id)
                select new BO.Engineer()
                {
                  Id= doEngineer.Id,
                  Name= doEngineer.Name,
                  Cost = doEngineer.Cost,
                  Email = doEngineer.Email, 
                  Level= (BO.EngineerExperience?)(doEngineer?.Level),
                  Task = new BO.EngineerInTask
                  {
                      Id=doEngineer.Id,
                      Name= doEngineer.Name,
                  }
                });
    }

    public void Update(BO.Engineer item)
    {
        if (item!.Id < 0)
            throw new BlArgumentOutOfRangeException("Engineer ID must be a positive number");
        if (string.IsNullOrEmpty(item.Name))
            throw new BlArgumentOutOfRangeException("Engineer name cannot be empty");
        if (item.Cost < 0)
            throw new BlArgumentOutOfRangeException("Engineer cost must be a positive number");
        if (string.IsNullOrEmpty(item.Email))
            throw new BlArgumentOutOfRangeException("Engineer email cannot be empty");
        DO.Engineer existingEngineer = _dal.Engineer.Read(item!.Id)!;
        if (existingEngineer!.Level < (DO.EngineerExperience)item.Level!)
        {
                throw new BlArgumentOutOfRangeException($"Engineer level it's not Upwards ");
        }
        try 
        { 
         existingEngineer = new DO.Engineer
         (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);
         _dal.Engineer.Update(existingEngineer);
         DO.Task? existingTask = _dal.Task.Read(item.Task!.Id)!;
         DO.Task newTask = new DO.Task(existingTask.Id, item.Id, existingTask.Alias, existingTask.Description, existingTask.CreatedAtDate, existingTask.RequiredEffortTime, existingTask.IsMilestone, existingTask.Copmlexity, existingTask.StartDate, existingTask.ScheduledDate, existingTask.DeadlineDate, existingTask.CompleteDate, existingTask.Deliverables, existingTask.Remarks);
         _dal.Task.Update(newTask);  
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to update engineer. {ex.Message}");
        }
    }
}

