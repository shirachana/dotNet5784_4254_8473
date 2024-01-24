namespace BO;
/// <summary>
/// milestone to BO
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number) 
/// <param name="Alias"></nickname>
/// <param name="Description"></Description>
/// <param name="CreatedAtDate"></Task creation date>
/// <param name="DeadlineDate"></Last possible end date (deadline)>
/// <param name="CompleteDate"></Actual end date>
/// <param name="Remarks"></Remarks>
/// <param name="Status"></Enum the status of the task>
/// <param name="Dependencies"><</list>list of dependencies
/// <param name="ForecastDate"><</Date>
///  <param name="CompletionPercentag"><<precent of completion
/// </summary>
public class Milestone
{
  public int Id {  get; init; }
  public string? Description {  get; set; }
  public string? Alias { get; set; }
  public DateTime CreatedAtDate {  get; init; } 
  public BO.Status Status {  get; set; } 
  public DateTime ForecastDate {  get; init; }
  public DateTime DeadlineDate {  get; set; }
  public DateTime CompleteDate {  get; init; }
  public double CompletionPercentage {  get; set; }
  public string? Remarks {  get; set; }
  public List<BO.TaskInList>? Dependencies {  get; init; }
  //public override string ToString() => this.ToStringProperty();
}
