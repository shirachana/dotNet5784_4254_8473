using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

internal class UserImplementation : IUser
{
    readonly string s_users_xml = "users";
    public void Clear()
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        users.Clear();
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);
    }

    public string Create(User item)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);


        if (users.Any(e => e.Id == item.Id))
        {
            throw new DalAlreadyExistsException($"User with ID={item.Id} already exists");
        }

        users.Add(item);
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);
        

        return item.Id!;
    }

    public void Delete(string id)
    {

        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);

        User userToDelete = users.FirstOrDefault(e => e.Id == id)!;

        if (userToDelete != null)
        {
            users.Remove(userToDelete);
            XMLTools.SaveListToXMLSerializer(users, s_users_xml);
        }
        else
        {
            throw new DalDoesNotExistException($"User with ID={id} does not exist");
        }
    }

    public User? Read(string id)
    {
        List<User> UserElem = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        return UserElem.FirstOrDefault(it => it.Id == id) ?? throw new DalDoesNotExistException($"User with ID={id} does Not exist");
    }

    public IEnumerable<User> ReadAll(Func<User, bool>? filter = null)
    {
        List<User> UserElem = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        if (filter == null)
        {
            return UserElem.Select(item => item);
        }
        else
        {
            return UserElem.Where(filter);
        }
    }
}
