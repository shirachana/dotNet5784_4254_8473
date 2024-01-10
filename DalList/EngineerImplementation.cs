
using DalApi;
using DO;

namespace Dal;
internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// a function that Adding a new object of type Engineer to a database
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public int Create(Engineer item)
    {
        foreach (var engineer in DataSource.Engineers)
        {
            if (engineer.Id == item.Id)
            {
                throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exist");
            }
        }
        DataSource.Engineers.Add(item);
        return item.Id;
    }
    /// <summary>
    /// a function that do deletion of an existing object with a certain ID, from the list of objects of type Engineer.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="Exception"></exception>

    public void Delete(int id)
    {
        if (DataSource.Engineers.RemoveAll(item => item.Id == id) == 0)
            throw new DalDoesNotExistException($"Engineers with ID={id} does Not exist");
    }

    /// <summary>
    /// a function that eturning a reference to a single object of type Engineer with a certain ID, if it exists in a database (in a list of data of type Engineer), or null if the object does not exist.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(item => item.Id == id) ?? throw new DalDoesNotExistException($"Engineer with {id} does Not exist");
    }
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        return DataSource.Engineers.FirstOrDefault(filter) ?? throw new DalDoesNotExistException($"Engineers with {filter} does Not exist"); ;
    }

    /// <summary>
    /// a function that return a copy of the list of references to all objects of type Engineer
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item);
        else
            return DataSource.Engineers.Where(filter);
    }
    /// <summary>
    /// Update of an existing object. The update will consist of deleting the existing object with the same ID number and replacing it with a new object with the same ID number and updated fields.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="Exception"></exception>
    public void Update(Engineer item)
    {
        // Check if there is an object with the received ID
        Engineer? existingItem = DataSource.Engineers.FirstOrDefault(x => x.Id == item.Id);
        if (existingItem == null)
        {
            // If not present, throw an exception
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
        }
        else
        {
            // If exists, remove the existing object and add the updated object
            DataSource.Engineers.Remove(existingItem);
            DataSource.Engineers.Add(item);
        }
    }
}

