namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

internal class TaskImplementation:ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task item)
    {
        List<Task> Tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        int nextId = Config.NextTaskId;
        Task copy = item with { Id=nextId };
        Tasks.Add(copy);
        XMLTools.SaveListToXMLSerializer(Tasks, s_tasks_xml);
        return nextId;
    }
    
    public void Delete(int id)
    {
    
    }
   
    public Task? Read(int id)
    {
        XElement? taskElem = XMLTools.LoadListFromXMLElement(s_tasks_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        return taskElem is null? null : new Task
        {
            Id = (int)taskElem!.Element("Id")!,
            Engineerld = (int)taskElem!.Element("EngineerId")!,
            Alias = (string?)taskElem.Element("Alias"),
            Description = (string?)taskElem.Element("Description"),
            CreatedAtDate = (DateTime?)taskElem.Element("CreatedAtDate"),
            RequiredEffortTime = (TimeSpan?)taskElem.Element("RequiredEffortTime"),
            IsMilestone = (bool)taskElem!.Element("IsMilestone")!,
            Copmlexity = (DO.EngineerExperience?)Enum.Parse(typeof(DO.EngineerExperience), (string)taskElem!.Element("Complexity")!),
            StartDate = (DateTime?)taskElem.Element("StartDate"),
            ScheduledDate = (DateTime?)taskElem.Element("ScheduledDate"),
            DeadlineDate = (DateTime?)taskElem.Element("DeadlineDate"),
            CompleteDate = (DateTime?)taskElem.Element("CompleteDate"),
            Deliverables = (string?)taskElem.Element("Deliverables"),
            Remarks = (string?)taskElem.Element("Remarks")
        };
    }

    public Task? Read(Func<Task, bool> filter)
    {
       
            XElement? taskElem = XMLTools.LoadListFromXMLElement(s_tasks_xml)
                .Elements()
                .Where(s => filter(new Task
                {
                    Id = (int)s.Element("Id")!,
                    Engineerld = (int)s.Element("EngineerId")!,
                    Alias = (string?)s.Element("Alias"),
                    Description = (string?)s.Element("Description"),
                    CreatedAtDate = (DateTime?)s.Element("CreatedAtDate"),
                    RequiredEffortTime = (TimeSpan?)s.Element("RequiredEffortTime"),
                    IsMilestone = (bool)s.Element("IsMilestone")!,
                    Copmlexity = (DO.EngineerExperience?)Enum.Parse(typeof(DO.EngineerExperience), (string)s.Element("Complexity")!),
                    StartDate = (DateTime?)s.Element("StartDate"),
                    ScheduledDate = (DateTime?)s.Element("ScheduledDate"),
                    DeadlineDate = (DateTime?)s.Element("DeadlineDate"),
                    CompleteDate = (DateTime?)s.Element("CompleteDate"),
                    Deliverables = (string?)s.Element("Deliverables"),
                    Remarks = (string?)s.Element("Remarks")

                }))
                .FirstOrDefault();

            return taskElem != null
                ? new Task
                {
                    Id = (int)taskElem!.Element("Id")!,
                    Engineerld = (int)taskElem!.Element("EngineerId")!,
                    Alias = (string?)taskElem.Element("Alias"),
                    Description = (string?)taskElem.Element("Description"),
                    CreatedAtDate = (DateTime?)taskElem.Element("CreatedAtDate"),
                    RequiredEffortTime = (TimeSpan?)taskElem.Element("RequiredEffortTime"),
                    IsMilestone = (bool)taskElem!.Element("IsMilestone")!,
                    Copmlexity = (DO.EngineerExperience?)Enum.Parse(typeof(DO.EngineerExperience), (string)taskElem!.Element("Complexity")!),
                    StartDate = (DateTime?)taskElem.Element("StartDate"),
                    ScheduledDate = (DateTime?)taskElem.Element("ScheduledDate"),
                    DeadlineDate = (DateTime?)taskElem.Element("DeadlineDate"),
                    CompleteDate = (DateTime?)taskElem.Element("CompleteDate"),
                    Deliverables = (string?)taskElem.Element("Deliverables"),
                    Remarks = (string?)taskElem.Element("Remarks")
                }
         : null;
        

    }



    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
            var tasks = XMLTools.LoadListFromXMLElement(s_tasks_xml).Elements();

            if (filter == null)
            {
                return tasks.Select(s => new Task
                {
                    Id = (int)s.Element("Id")!,
                    Engineerld = (int)s.Element("EngineerId")!,
                    Alias = (string?)s.Element("Alias"),
                    Description = (string?)s.Element("Description"),
                    CreatedAtDate = (DateTime?)s.Element("CreatedAtDate"),
                    RequiredEffortTime = (TimeSpan?)s.Element("RequiredEffortTime"),
                    IsMilestone = (bool)s.Element("IsMilestone")!,
                    Copmlexity = (DO.EngineerExperience?)Enum.Parse(typeof(DO.EngineerExperience), (string)s.Element("Complexity")!),
                    StartDate = (DateTime?)s.Element("StartDate"),
                    ScheduledDate = (DateTime?)s.Element("ScheduledDate"),
                    DeadlineDate = (DateTime?)s.Element("DeadlineDate"),
                    CompleteDate = (DateTime?)s.Element("CompleteDate"),
                    Deliverables = (string?)s.Element("Deliverables"),
                    Remarks = (string?)s.Element("Remarks")
                });
            }
            else
            {
            return tasks.Select(s => new Task
            {
                Id = (int)s.Element("Id")!,
                Engineerld = (int)s.Element("EngineerId")!,
                Alias = (string?)s.Element("Alias"),
                Description = (string?)s.Element("Description"),
                CreatedAtDate = (DateTime?)s.Element("CreatedAtDate"),
                RequiredEffortTime = (TimeSpan?)s.Element("RequiredEffortTime"),
                IsMilestone = (bool)s.Element("IsMilestone")!,
                Copmlexity = (DO.EngineerExperience?)Enum.Parse(typeof(DO.EngineerExperience), (string)s.Element("Complexity")!),
                StartDate = (DateTime?)s.Element("StartDate"),
                ScheduledDate = (DateTime?)s.Element("ScheduledDate"),
                DeadlineDate = (DateTime?)s.Element("DeadlineDate"),
                CompleteDate = (DateTime?)s.Element("CompleteDate"),
                Deliverables = (string?)s.Element("Deliverables"),
                Remarks = (string?)s.Element("Remarks")
            }).Where(filter) ;
        }
    }

    public void Update(Task item)
    {
        List<Task> Tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        if(Tasks.RemoveAll(item=>item.Id==item.Id)==0)
            throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");
        Tasks.Add(item);
        XMLTools.SaveListToXMLSerializer(Tasks,s_tasks_xml);
    }
}
