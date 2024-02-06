namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class TaskImplementation : ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task item)
    {
        List<Task> Tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        int nextId = Config.NextTaskId;
        Task copy = item with { Id = nextId };
        Tasks.Add(copy);
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);
        return nextId;
    }

    public void Delete(int id)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);

        foreach (Task taskToDelete in tasks)
        {
            if (taskToDelete.Id == id)
            {
                tasks.Remove(taskToDelete);
                XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
                return;
            }
        }
        throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
    }

    public Task? Read(int id)
    {
        List<Task> taskElem = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return taskElem.FirstOrDefault(it => it.Id == id) ?? throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> taskElem = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        return taskElem.FirstOrDefault(filter) ?? throw new DalDoesNotExistException($"Task with ID={filter} does Not exist");
    }



    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> taskElem = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if (filter == null)
        {
            return taskElem.Select(item => item);
        }
        else
        {
            return taskElem.Where(filter);
        }
    }

    
    public void Update(Task item)
    {
        List<Task> tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        Task existingTask = tasks.FirstOrDefault(x => x.Id == item.Id)!;
        if (existingTask == null)
        {
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");
        }
        else
        {
            tasks.Remove(existingTask);
            tasks.Add(item);
            XMLTools.SaveListToXMLSerializer(tasks, s_tasks_xml);
        }
    }
          
    public void Clear()
    {
        List<Task> Tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        Tasks.Clear();
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);
       Config.NextTaskId = 1; 
    }
}
