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
        List<User> admin = new();
        foreach(User user in users)
        {
            if(user.IsAdmin) 
                admin.Add(user);
        }
        XMLTools.SaveListToXMLSerializer(admin, s_users_xml);
    }

    public string Create(User item)
    {
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);
        foreach (User tempUser in users) 
        {
            if(tempUser.UserName==item.UserName)
            {
                throw new DalAlreadyExistsException($"User with ID={item.Id} already exists");
            }
        }


        //string code = item.Id!.ToString();
        //User user = new(item.userName, code, false);
        users.Add(item);
        XMLTools.SaveListToXMLSerializer(users, s_users_xml);
        return item.UserName!;
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
            throw new DalDoesNotExistException($" User name or password is incorrect");
        }
    }

    public User? Read(string id)
    {
        List<User> UserElem = XMLTools.LoadListFromXMLSerializer<User>(s_users_xml);

        return UserElem.FirstOrDefault(it => it.Id == id) ?? throw new DalDoesNotExistException($" User name or password is incorrect");
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
