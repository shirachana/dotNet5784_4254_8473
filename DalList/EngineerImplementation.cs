
using DalApi;
using DO;

namespace Dal;
public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        //for entities with normal id (not auto id)
        if (Read(item.Id) is not null)
            throw new Exception($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        if (DataSource.Engineers.RemoveAll(item => item.Id == id) == 0)
            throw new Exception($"Engineers with ID={id} does Not exist");
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.Find(item => item.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        // Check if there is an object with the received ID
        Engineer? existingItem = DataSource.Engineers.FirstOrDefault(x => x.Id == item.Id);
        if (existingItem == null)
        {
            // If not present, throw an exception
            throw new Exception($"Engineer with ID={item.Id} does Not exist");
        }
        else
        {
            // If exists, remove the existing object and add the updated object
            DataSource.Engineers.Remove(existingItem);
            DataSource.Engineers.Add(item);
        }
    }
}

