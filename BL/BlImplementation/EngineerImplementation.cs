namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;
using System.Reflection.Emit;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(BO.Engineer? item)
    {
        try
        {
            if (Tools.CheckEngineerHealth(item!))
            {
                DO.Engineer doEngineer = new DO.Engineer
                (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);
                _dal.Engineer.Create(doEngineer);
                // Proceed with other operations if the engineer is healthy
            }        
        }
        catch(DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistException($"Student with ID={item!.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        
        BO.Engineer engineerToDelete = Read(id)!;
        IEnumerable<DO.Task> tsk = from DO.Task task in _dal.Task.ReadAll(task => task.Engineerld == id)
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
        if(filter == null)
              return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                      orderby doEngineer.Level
                      select new BO.Engineer()
                      {
                          Id= doEngineer.Id,
                          Name = doEngineer.Name,
                          Cost = doEngineer.Cost,
                          Email = doEngineer.Email, 
                          Level= (BO.EngineerExperience?)(doEngineer?.Level),
                          Task = new BO.EngineerInTask
                          {
                              Id = doEngineer.Id,
                              Name= doEngineer.Name,
                          }
                      });
        else
            return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                    let tsk = /*tasks.FirstOrDefault(t => t!.Engineerld == doEngineer.Id)*/
                    /*select*/ new BO.Engineer()
                      {
                        Id = doEngineer.Id,
                        Name = doEngineer.Name,
                        Cost = doEngineer.Cost,
                        Email = doEngineer.Email,
                        Level = (BO.EngineerExperience?)(doEngineer?.Level),
                        Task = new BO.EngineerInTask
                        {
                            Id = doEngineer.Id,
                            Name = doEngineer.Name,
                        }
                      }
                    where filter(tsk)
                    orderby doEngineer.Level
                    select tsk);
    }

    public void Update(BO.Engineer item)
    {
        try
        {
            if (Tools.CheckEngineerHealth(item!))
            {
                DO.Engineer existingEngineer = _dal.Engineer.Read(item!.Id)!;
                if (existingEngineer!.Level < (DO.EngineerExperience)item.Level!)
                {
                    throw new BlArgumentOutOfRangeException($"Engineer level it's not Upwards ");
                }
                existingEngineer = new DO.Engineer
                (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);
                _dal.Engineer.Update(existingEngineer);
                DO.Task? existingTask = _dal.Task.Read(item.Task!.Id)!;
                DO.Task newTask = new DO.Task(existingTask.Id, item.Id, existingTask.Alias, existingTask.Description, existingTask.CreatedAtDate, existingTask.RequiredEffortTime, existingTask.IsMilestone, existingTask.Copmlexity, existingTask.StartDate, existingTask.ScheduledDate, existingTask.DeadlineDate, existingTask.CompleteDate, existingTask.Deliverables, existingTask.Remarks);
                _dal.Task.Update(newTask);
            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to update engineer. {ex.Message}");
        }
    }
}

