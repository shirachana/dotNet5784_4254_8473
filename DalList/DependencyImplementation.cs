
using DalApi;
using DO;

namespace Dal;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    /// <summary>
    /// a function that Adding a new object of type Dependency to a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Create(Dependency item)
    {
        //there is auto id
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencys.Add(copy);
        return id;
    }
    /// <summary>
    /// a function that do deletion of an existing object with a certain ID, from the list of objects of type Dependency.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>
    public void Delete(int id)
    {
        if(DataSource.Dependencys.RemoveAll(item => item.Id == id) == 0)
            throw new Exception($"Dependency with ID={id} does Not exist");
    }
    /// <summary>
    /// a function that eturning a reference to a single object of type Dependency with a certain ID, if it exists in a database (in a list of data of type Dependency), or null if the object does not exist.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Dependency? Read(int id)
    {
            return DataSource.Dependencys.Find(item => item.Id == id)??throw new Exception($"Dependency with ID={id} does Not exist");
    }
    /// <summary>
    /// a function that return a copy of the list of references to all objects of type dependency
    /// </summary>
    /// <returns></returns>
    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }
    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Dependency item)
    {
        // Check if there is an object with the received ID
        Dependency? existingItem = DataSource.Dependencys.FirstOrDefault(x => x.Id == item.Id);
        if (existingItem == null)
        {
            // If not present, throw an exception
            throw new Exception($"Dependency with ID={item.Id} does Not exist");
        }
        else
        {
            // If exists, remove the existing object and add the updated object
            DataSource.Dependencys.Remove(existingItem);
            DataSource.Dependencys.Add(item);
        }
    }
}
