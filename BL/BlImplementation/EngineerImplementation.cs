namespace BlImplementation;
using BlApi;
using BO;
using System.Collections.Generic;
using System.Reflection.Emit;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer? item)
    {
        DO.Engineer doEngineer = new DO.Engineer
    (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);

        try
        {
            if (item?.Id < 0)
                throw new Exception("Engineer ID must be a positive number");
            if (!string.IsNullOrEmpty(item?.Name))
                throw new Exception("Engineer name cannot be empty");
            if (item?.Cost < 0)
                throw new Exception("Engineer cost must be a positive number");
            if (!string.IsNullOrEmpty(item?.Email))
                throw new Exception("Engineer email cannot be empty");
            int idEng = _dal.Engineer.Create(doEngineer);
            return idEng;

        }
        catch(Exception ex)
        {
            throw new Exception($"Student with ID={item.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
       DO.Engineer engineerToDelete = _dal.Engineer.Read(id)!;
        if (engineerToDelete == null)
        {
            throw new Exception($"Engineer with ID={id} does not exist");
        }
        //if (/*_dal.Task.Equals(engineerToDelete.Id)*/engineerToDelete.Id==)
        //{
        //    throw new Exception($"Engineer with ID={id} cannot be deleted because they are assigned to a task");
        //}
        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new Exception($"Failed to delete engineer with ID={id}. {ex.Message}");
        }
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
        
    }
}

