namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public void Create(Engineer? item)
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
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistException($"Student with ID={item!.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {

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
        try
        {
            return new BO.Engineer()
            {
                Id = id,
                Cost = doEngineer.Cost,
                Email = doEngineer.Email,
                Name = doEngineer.Name,
                Level = (BO.EngineerExperience?)(doEngineer.Level),
                Task = GetEngineerTask(doEngineer)
            };
        }
        catch (DO.DalDoesNotExistException)
        {
            throw new BlDoesNotExistException($"Engineer with ID={id} does Not exist");
        }
    }
    
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
            IEnumerable<DO.Task?> tasks = _dal.Task.ReadAll();
            if (filter == null)
                return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                        orderby doEngineer.Level
                        //let task = _dal.Task.Read(t => t.Engineerld == doEngineer.Id /*&& t!.StartDate != null && t.CompleteDate == null*/)
                        select new BO.Engineer()
                        {
                            Id = doEngineer.Id,
                            Name = doEngineer.Name,
                            Cost = doEngineer.Cost,
                            Email = doEngineer.Email,
                            Level = (BO.EngineerExperience?)(doEngineer?.Level),
                            Task = GetEngineerTask(doEngineer) /*new BO.TaskInEngineer
                            {
                                Id = task.Id,
                                Alias = task.Alias,
                            }*/
                        });
            else
                return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                        //let tsk = _dal.Task.Read(t => t.Engineerld == doEngineer.Id /*&& t!.StartDate != null && t.CompleteDate == null*/)
                        let boWorker =/*tasks.FirstOrDefault(t => t!.Engineerld == doEngineer.Id)*/
                        /*select*/ new BO.Engineer()
                                   {
                                       Id = doEngineer.Id,
                                       Name = doEngineer.Name,
                                       Cost = doEngineer.Cost,
                                       Email = doEngineer.Email,
                                       Level = (BO.EngineerExperience?)(doEngineer?.Level),
                                       Task = GetEngineerTask(doEngineer)/*new BO.TaskInEngineer
                                       {
                                           Id = tsk.Id,
                                           Alias = tsk.Alias,
                                       }*/
                                   }
                        where filter(boWorker)
                        orderby doEngineer.Level
                        select boWorker);
    }

    public void Update(BO.Engineer item)
    {
        try
        {
            if (Tools.CheckEngineerHealth(item!))
            {
                DO.Engineer engineer = _dal.Engineer.Read(item!.Id)  ?? throw new BlDoesNotExistException("The Engineer to update does not Exist");
                if (engineer.Level < (DO.EngineerExperience)item.Level!)
                {
                    throw new BlArgumentOutOfRangeException($"Engineer level it's not Upwards ");
                }
                engineer = new DO.Engineer
                (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);


                DO.Task existingTask = _dal.Task.Read(item.Task!.Id) ?? throw new BlDoesNotExistException("The Task does not Exist");
                existingTask = existingTask with { Engineerld = item.Id };

                _dal.Engineer.Update(engineer);
                _dal.Task.Update(existingTask);
            }
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BlDoesNotExistException($"Failed to update engineer. {ex.Message}");
        }
    }
    public void UpdateEngineerToTask(int idEng,int idTsk)
    {
        if (Tools.CheckStatusProject() != BO.StatusProject.Execution)
            throw new BlPlanning("the Engineer cant add task becuse the start project and the task date not update yet");
        try
        {
            DO.Engineer eng = _dal.Engineer.Read(idEng) ?? throw new BlDoesNotExistException($"the Engineer with the id={idEng} does not exist"); ;
            DO.Task tsk = _dal.Task.Read(idTsk) ?? throw new BlDoesNotExistException($"the Task with the id={idTsk} does not exist");
            if (eng.Level < tsk.Copmlexity)
                throw new BlArgumentOutOfRangeException("The level of the Task dont match to the Engineer level");
            if (tsk.Engineerld != 0 && tsk.Engineerld != idEng)
                throw new BlArgumentOutOfRangeException("there is another Engineer that work on this task");
            IEnumerable<DO.Task> tasksDep=
              from DO.Dependency d in _dal.Dependency.ReadAll()
              where d.DependentTask == idTsk
              select _dal.Task.Read(d.DependsOnTask);
            tasksDep.Where(p => (p.StartDate + p.RequiredEffortTime) !< DateTime.Today);
            if (tasksDep.Any())
                throw new BlArgumentOutOfRangeException("The Engineer cannot be registered to the task because the pending task has not yet finished ");
            DO.Task uppTask = new DO.Task(tsk.Id,idEng,tsk.Alias,tsk.Description,tsk.CreatedAtDate,tsk.RequiredEffortTime,tsk.IsMilestone,tsk.Copmlexity,tsk.StartDate,tsk.ScheduledDate,tsk.DeadlineDate,tsk.CompleteDate,tsk.Deliverables,tsk.Remarks);
            _dal.Task.Update(uppTask);
        }
        catch(DO.DalDoesNotExistException ex) { throw new BO.BlDoesNotExistException("cant update the Task in The Engineer", ex); }

    }

    public void Clear()
    {
        _dal.Engineer.Clear();
    }

    private BO.TaskInEngineer? GetEngineerTask(DO.Engineer doEng)
    {
        try
        {
            DO.Task? tsk = _dal.Task.Read(t => t.Engineerld == doEng.Id && t.StartDate != null && t.CompleteDate == null);

            return new BO.TaskInEngineer
            {
                Id = tsk!.Id,
                Alias = tsk.Alias
            };
        }
        catch (DO.DalDoesNotExistException) { return null; }
    }


}

