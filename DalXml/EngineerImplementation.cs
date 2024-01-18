namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        // Check if an engineer with the same ID already exists
        if (engineers.Any(e => e.Id == item.Id))
        {
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        }

        engineers.Add(item);
        XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);

        return item.Id;
    }

    public void Delete(int id)
    {
         List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        foreach (Engineer EngineerToDelete in engineers)
        {
             if (EngineerToDelete.Id == id)
             {
                engineers.Remove(EngineerToDelete);
                XMLTools.SaveListToXMLSerializer(engineers, s_engineers_xml);
                return;
             }
        }
        throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
        
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

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
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
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (Engineers.RemoveAll(item => item.Id == item.Id) == 0)
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
        Engineers.Add(item);
        XMLTools.SaveListToXMLSerializer(Engineers, s_engineers_xml);
    }
}
