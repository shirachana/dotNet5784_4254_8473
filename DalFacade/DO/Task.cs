namespace DO;
/// <summary>
/// Task to do
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number) 
/// <param name="Alias"></nickname>
/// <param name="Description"></Description>
/// <param name="CreatedAtDate"></Task creation date>
/// <param name="RequiredEffortTime"></The amount of time required to perform the task>
/// <param name="IsMilestone"></Milestone (Boolean) - will be expressed later>
/// <param name="Copmlexity"></The difficulty level of the taskram>
/// <param name="StartDate"></Planned date for the start of work>
/// <param name="ScheduledDate"></Date of commencement of work on the assignment>
/// <param name="DeadlineDate"></Last possible end date (deadline)>
/// <param name="CompleteDate"></Actual end date>
/// <param name="Deliverables"></product>
/// <param name="Remarks"></Remarks>
/// <param name="Engineerld"></The engineer ID assigned to the task>
public record Task
(
    int Id,
    int Engineerld,
    string? Alias = null,
    string? Description = null,
    DateTime? CreatedAtDate = null,
    TimeSpan? RequiredEffortTime = null,
    bool IsMilestone = false,
    DO.EngineerExperience? Copmlexity = null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null
)
{
    public Task() : this(0, 0/* null, null, null, null, false, null, null, null, null, null, null, null*/) { }
  
}