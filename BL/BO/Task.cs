

namespace BO;
/// <summary>
/// /// Task to BO
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number) 
/// <param name="Alias"></nickname>
/// <param name="Description"></Description>
/// <param name="CreatedAtDate"></Task creation date>
/// <param name="RequiredEffortTime"></The amount of time required to perform the task>
///// <param name="IsMilestone"></Milestone (Boolean) - will be expressed later>
/// <param name="Copmlexity"></The difficulty level of the taskram>
/// <param name="StartDate"></Planned date for the start of work>
/// <param name="ScheduledDate"></Date of commencement of work on the assignment>
/// <param name="DeadlineDate"></Last possible end date (deadline)>
/// <param name="CompleteDate"></Actual end date>
/// <param name="Deliverables"></product>
/// <param name="Remarks"></Remarks>
/// <param name="Engineerld"></The engineer ID assigned to the task>
/// <param name="Status"></Enum the status of the task>
/// <param name="Dependencies"><</list>list of dependencies
///// <param name="Milestone"><</Milestone>
/// <param name="ForecastDate"><</Date>
///  <param name="Engineer"><</EngineerInTask>engineer that do the task
/// </summary>
public class Task
{

    public int Id { get; set;}
    public string? Description { get; set; }
    public string? Alias {  get; set; }    
    public DateTime? CreatedAtDate { get; set; }
    public BO.Status Status { get; set; }
    public List<BO.TaskInList>? Dependencies{ get; set; }
    //public BO.MilestonelnList? Milestone{ get; init; }
    public TimeSpan? RequiredEffortTime{ get; set; }
    public DateTime? StartDate{ get; set; }
    public DateTime? ScheduledDate{ get; set; }
    public DateTime? ForecastDate { get; set; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; set; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public BO.EngineerInTask? Engineer { get; set; }
    public BO.EngineerExperience Copmlexity { get; set; }
    public override string ToString() => this.ToStringProperty();
}
