
namespace Dal;

using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        //there is auto id
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        if (DataSource.Tasks.RemoveAll(item => item.Id == id) == 0)
            throw new Exception($"Task with ID={id} does Not exist");
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(item => item.Id == id);
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        // Check if there is an object with the received ID
        Task? existingItem = DataSource.Tasks.FirstOrDefault(x => x.Id == item.Id);
        if (existingItem == null)
        {
            // If not present, throw an exception
            throw new Exception($"Task with ID={item.Id} does Not exist");
        }
        else
        {
            // If exists, remove the existing object and add the updated object
            DataSource.Tasks.Remove(existingItem);
            DataSource.Tasks.Add(item);
        }
    }
}

