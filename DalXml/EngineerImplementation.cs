namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";
    readonly string s_users_xml = "users";

    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        List<User> users = XMLTools.LoadListFromXMLSerializer<User>("users");

        if (engineers.Any(e => e.Id == item.Id))
        {
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        }

        
        string code = item.Id.ToString();
        User user = new(item.Name, code, false);
        users.Add(user);
        XMLTools.SaveListToXMLSerializer(users, "users");

        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
        return item.Id;
        
    }
    public void Delete(int id)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        Engineer engineerToDelete = engineers.FirstOrDefault(e => e.Id == id)!;

        if (engineerToDelete != null)
        {
            engineers.Remove(engineerToDelete);
            XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
        }
        else
        {
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");
        }
    }

    public Engineer? Read(int id)
    {
        List<Engineer> EngineerElem = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        return EngineerElem.FirstOrDefault(it => it.Id == id) ?? throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");

    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> EngineerElem = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        return EngineerElem.FirstOrDefault(filter) ?? throw new DalDoesNotExistException($"Engineer with ID={filter} does Not exist");

    }



    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> EngineerElem = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (filter == null)
        {
            return EngineerElem.Select(item => item);
        }
        else
        {
            return EngineerElem.Where(filter);
        }

    }


    public void Update(Engineer item)
    {
            List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
            Engineer existingEngineer = engineers.FirstOrDefault(x => x.Id == item.Id)!;
            if (existingEngineer == null)
            {
                throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");
            }
            else
            {
                engineers.Remove(existingEngineer);
                engineers.Add(item);
                XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
            }
    }

    public void Clear()
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        Engineers.Clear();
        XMLTools.SaveListToXMLSerializer(Engineers, s_engineers_xml);
    }
}
