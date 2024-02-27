
using DalApi;
using DO;
using System.Linq;

namespace Dal;

internal class UserImplementation : IUser
{
    public void Clear()
    {
        List<User> users = DataSource.Users;
        List<User> admin = new();
        foreach (User user in users)
        {
            if (user.IsAdmin)
                admin.Add(user);
        }
        DataSource.Users.Clear();
        DataSource.Users.AddRange(admin);
    }

    public string Create(User item)
    {
        foreach (var user in DataSource.Users)
        {
            if (user.Id == item.Id)
            {
                throw new DalAlreadyExistsException($"User with ID={item.Id} already exist");
            }
        }
        DataSource.Users.Add(item);
        return item.Id!;
    }

    public void Delete(string id)
    {
        if (DataSource.Users?.RemoveAll(item => item.Id == id) == null)
            throw new DalDoesNotExistException($"Users with ID={id} does Not exist");
    }

    public User? Read(string id)
    {
        return DataSource.Users.FirstOrDefault(item => item.Id == id) ?? throw new DalDoesNotExistException($"User with {id} does Not exist");
    }

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {

        if (filter == null)
            return DataSource.Users.Select(item => item);
        else
            return DataSource.Users.Where(filter);
    }
    public void Update(DO.User item)
    {
        // Check if there is an object with the received ID
        DO.User? existingItem = DataSource.Users.FirstOrDefault(x => x.Id == item.Id);
        if (existingItem == null)
        {
            // If not present, throw an exception
            throw new DalDoesNotExistException($"User with ID={item.Id} does Not exist");
        }
        else
        {
            // If exists, remove the existing object and add the updated object
            DataSource.Users.Remove(existingItem);
            DataSource.Users.Add(item);
        }
    }
}
