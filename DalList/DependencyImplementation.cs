
using DalApi;
using DO;

namespace Dal;
using System.Collections.Generic;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        //there is auto id
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencys.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        if(DataSource.Dependencys.RemoveAll(item => item.Id == id) == 0)
            throw new Exception($"Dependency with ID={id} does Not exist");
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencys.Find(item => item.Id == id);
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencys);
    }

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
