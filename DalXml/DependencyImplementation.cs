namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class DependencyImplementation : IDependency
{
    readonly string s_dependencys_xml = "dependencys";

    public int Create(Dependency item)
    {
        XElement? dependencyElem = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        int nextId = Config.NextDependencyId;
       
        var copy = new XElement ( "Dependency",
                                 new XElement ( "Id",nextId),
                                new XElement ( "DependentTask", item.DependentTask ),
                                new XElement ( "DependsOnTask", item.DependsOnTask ) );
        dependencyElem.Add(copy);
        XMLTools.SaveListToXMLElement(dependencyElem, s_dependencys_xml);
        return nextId;
    }
    public void Delete(int id)
    {
        XElement dependenciesRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement dependencyToRemove = dependenciesRoot.Elements("Dependency").FirstOrDefault(elem => elem.Element("Id")?.Value == id.ToString())!;
        if (dependencyToRemove == null)
            throw new DalDoesNotExistException($"Dependency with ID={id} does not exist");
        dependencyToRemove.Remove();
        XMLTools.SaveListToXMLElement(dependenciesRoot, s_dependencys_xml);
    }



    public Dependency? Read(int id)
    {
        XElement? dependencyElem = XMLTools.LoadListFromXMLElement(s_dependencys_xml).Elements().FirstOrDefault(dep => (int?)dep.Element("Id") == id) ?? throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");

        return dependencyElem != null
            ? new Dependency
            {
                Id = (int)dependencyElem.Element("Id")!,
                DependentTask = (int)dependencyElem.Element("DependentTask")!,
                DependsOnTask = (int)dependencyElem.Element("DependsOnTask")!
            }
     : null;
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        XElement? dependencyElem = XMLTools.LoadListFromXMLElement(s_dependencys_xml)
      .Elements()
      .Where(s => filter(new Dependency
      {
          Id = (int)s.Element("Id")!,
          DependentTask = (int)s.Element("DependentTask")!,
          DependsOnTask = (int)s.Element("DependsOnTask")!
      }))
      .FirstOrDefault() ?? throw new DalDoesNotExistException($"Dependency with ID={filter} does Not exist"); ;

        return dependencyElem != null
            ? new Dependency
            {
                Id = (int)dependencyElem.Element("Id")!,
                DependentTask = (int)dependencyElem.Element("DependentTask")!,
                DependsOnTask = (int)dependencyElem.Element("DependsOnTask")!
            }
     : null;

    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {

        var x = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        var dependencys = x.Elements();

        if (filter == null)
        {
            return dependencys.Select(s => new Dependency
            {
                Id = (int)s.Element("Id")!,
                DependentTask = (int)s.Element("DependentTask")!,
                DependsOnTask = (int)s.Element("DependsOnTask")!
            });
        }
        else
        {
            return dependencys.Select(s => new Dependency
            {
                Id = (int)s.Element("Id")!,
                DependentTask = (int)s.Element("DependentTask")!,
                DependsOnTask = (int)s.Element("DependsOnTask")!
            }).Where(filter);
        }
        

    }

    public void Update(Dependency item)
    {

        XElement dependenciesRoot = XMLTools.LoadListFromXMLElement(s_dependencys_xml);
        XElement existingDependency = dependenciesRoot.Elements("Dependency")
            .FirstOrDefault(elem => elem.Element("Id")?.Value == item.Id.ToString())!;
        if (existingDependency == null)
        {
            throw new DalDoesNotExistException($"Dependency with ID={item.Id} does not exist");
        }
        else
        {
            existingDependency.Element("DependentTask")?.SetValue(item.DependentTask.ToString());
            existingDependency.Element("DependsOnTask")?.SetValue(item.DependsOnTask.ToString());
            XMLTools.SaveListToXMLElement(dependenciesRoot, s_dependencys_xml);
        }


    }

    public void Clear()
    {
        List<Dependency> Dependencys = XMLTools.LoadListFromXMLSerializer<Dependency>(s_dependencys_xml);
        Dependencys.Clear();
        XMLTools.SaveListToXMLSerializer(Dependencys, s_dependencys_xml);
    }
}
