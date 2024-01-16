namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    public int Create(Engineer item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Engineer? Read(int id)
    {
        XElement? engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements().FirstOrDefault(eng => (int?)eng.Element("Id") == id);

        return engineerElem != null
            ? new Engineer
            {
                Id = (int)engineerElem.Element("Id")!,
                cost = (double)engineerElem.Element("Cost")!,
                Email = (string?)engineerElem.Element("Email"),
                Name = (string?)engineerElem.Element("Name"),
                Level = engineerElem.Element("Level") != null
                    ? (DO.EngineerExperience?)Enum.Parse(typeof(DO.EngineerExperience), (string)engineerElem.Element("Level")!)
                    : null
            }
     : null;

    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        XElement? engineerElem = XMLTools.LoadListFromXMLElement(s_engineers_xml)
       .Elements()
       .Where(s => filter(new Engineer
       {
           Id = (int)s.Element("Id")!,
           cost = (double)s.Element("cost")!,
           Email = (string?)s.Element("Email"),
           Name = (string?)s.Element("Name"),
           Level = (DO.EngineerExperience?)Enum.Parse(typeof(DO.EngineerExperience), (string)s.Element("Level")!)
       }))
       .FirstOrDefault();

        return engineerElem != null
            ? new Engineer
            {
                Id = (int)engineerElem.Element("Id")!,
                cost = (double)engineerElem.Element("cost")!,
                Email = (string?)engineerElem.Element("Email"),
                Name = (string?)engineerElem.Element("Name"),
                Level = (DO.EngineerExperience?)Enum.Parse(typeof(DO.EngineerExperience), (string)engineerElem.Element("Level")!)
            }
     : null;

    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
            var engineers = XMLTools.LoadListFromXMLElement(s_engineers_xml).Elements();

            if (filter == null)
            {
                return engineers.Select(s => new Engineer
                {
                    Id = (int)s.Element("Id")!,
                    cost = (double)s.Element("Cost")!,
                    Email = (string?)s.Element("Email"),
                    Name = (string?)s.Element("Name"),
                    Level = (DO.EngineerExperience?)Enum.Parse(typeof(DO.EngineerExperience), (string)s.Element("Level")!)
                });
            }
            else
            {
                return engineers.Select(s => new Engineer
                {
                    Id = (int)s.Element("Id")!,
                    cost = (double)s.Element("Cost")!,
                    Email = (string?)s.Element("Email"),
                    Name = (string?)s.Element("Name"),
                    Level = (DO.EngineerExperience?)Enum.Parse(typeof(DO.EngineerExperience), (string)s.Element("Level")!)
                }).Where(filter);
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
