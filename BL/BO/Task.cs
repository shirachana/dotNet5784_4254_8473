

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
    private int v1;
    private int idT;
    private string? aliasTask;
    private string? discTask;
    private object value1;
    private object value2;
    private bool v2;
    private DO.EngineerExperience taskLevel;
    private object value3;
    private object value4;
    private object value5;
    private object value6;
    private string? deliTask;
    private string? taskRemark;

    public Task()
    {
        this.v1 = 0;
        this.idT = 0;
        this.aliasTask = null;
        this.discTask = null;
        this.value1 = "";
        this.value2 = "";
        this.v2 = false;
        this.taskLevel = 0;
        this.value3 = "";
        this.value4 = "";
        this.value5 = "";
        this.value6 = "";
        this.deliTask = null;
        this.taskRemark = null;
    }

    public Task(int v1, int idT, string? aliasTask, string? discTask, object value1, object value2, bool v2, 
        DO.EngineerExperience taskLevel, object value3, object value4, object value5, object value6, string? deliTask, string? taskRemark)
    {
        this.v1 = v1;
        this.idT = idT;
        this.aliasTask = aliasTask;
        this.discTask = discTask;
        this.value1 = value1;
        this.value2 = value2;
        this.v2 = v2;
        this.taskLevel = taskLevel;
        this.value3 = value3;
        this.value4 = value4;
        this.value5 = value5;
        this.value6 = value6;
        this.deliTask = deliTask;
        this.taskRemark = taskRemark;
    }

    public int Id { get; init;}
    public string? Description { get; set; }
    public string? Alias {  get; set; }    
    public DateTime? CreatedAtDate { get; init; }
    public BO.Status Status { get; set; }
    public List<BO.TaskInList>? Dependencies{ get; init; }
    //public BO.MilestonelnList? Milestone{ get; init; }
    public TimeSpan? RequiredEffortTime{ get; init; }
    public DateTime? StartDate{ get; set; }
    public DateTime? ScheduledDate{ get; init; }
    public DateTime? ForecastDate { get; init; }
    public DateTime? DeadlineDate { get; set; }
    public DateTime? CompleteDate { get; init; }
    public string? Deliverables { get; set; }
    public string? Remarks { get; set; }
    public BO.EngineerInTask? Engineer { get; init; }
    public BO.EngineerExperience Copmlexity { get; set; }
    public override string ToString() => this.ToStringProperty();
}
