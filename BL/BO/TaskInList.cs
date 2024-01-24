namespace BO;
/// <summary>
/// /// Task in list to BO
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number) 
/// <param name="Alias"></nickname>
/// <param name="Description"></Description>
/// <param name="Status"></Enum the status of the task>
/// </summary>
public class TaskInList
{
    public int Id {  get; init; } 
    public string? Description {  get; set; }
    public string? Alias {  get; set; }
    public BO.Status Status {  get; set; }
    //public override string ToString() => this.ToStringProperty();
}
