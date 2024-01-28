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
        
       BO.Engineer engineerToDelete = Read(id)!;
        if (engineerToDelete == null)
        {
            throw new Exception($"Engineer with ID={id} does not exist");
        }
        
        
        var eng = from BO.TaskInEngineer? tmp in engineerToDelete.Task
                  where tmp != null && (tmp.Status == BO.Status.Unscheduled || tmp.Status == BO.Status.Done)
                  select tmp;


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

        // Check the validity of the engineer's data
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

        try
        {
            // Check if the engineer exists in the data layer
            DO.Engineer existingEngineer = _dal.Engineer.Read(item!.Id)!;
            if (existingEngineer == null)
            {
                throw new Exception($"Engineer with ID={item.Id} does not exist");
            }

            // Update the properties of the engineer
             existingEngineer = new DO.Engineer
     (item!.Id, item.Cost, item.Email, item.Name, (DO.EngineerExperience?)item.Level);
            // Update the task if it's selected
            if (item.Task != null)
            {
                // Check if the task exists in the data layer
                DO.Task existingTask = _dal.Task.Read(item.Task.Id)!;
                if (existingTask == null)
                {
                    throw new Exception($"Task with ID={item.Task.Id} does not exist");
                }

                // Update the task details
                existingTask = new DO.Task(item.Task.Id,item.Id,item.Task.Alias);
                // Update other task properties if necessary

                // Save the updated task to the data layer
                _dal.Task.Update(existingTask);
            }

            // Save the updated engineer to the data layer
            _dal.Engineer.Update(existingEngineer);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new Exception($"Failed to update engineer. {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to update engineer. {ex.Message}");
        }
    }
}

