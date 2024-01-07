
namespace Dal;

using DalApi;
using DO;
using System.Collections.Generic;

public class TaskImplementation : ITask
{
    /// <summary>
    /// a function that Adding a new object of type task to a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Create(Task item)
    {
        //there is auto id
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }
    /// <summary>
    /// a function that do deletion of an existing object with a certain ID, from the list of objects of type task.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        if (DataSource.Tasks.RemoveAll(item => item.Id == id) == 0)
            throw new Exception($"Task with ID={id} does Not exist");
    }
    /// <summary>
    /// a function that eturning a reference to a single object of type task with a certain ID, if it exists in a database (in a list of data of type task), or null if the object does not exist.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(item => item.Id == id);
    }
    /// <summary>
    /// a function that return a copy of the list of references to all objects of type task
    /// </summary>
    /// <returns></returns>
    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }
    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
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

