namespace BO;
/// <summary>
///  Task in Engineer to BO
/// </summary>
/// <param name="Id">Unique ID number (automatic runner number) 
/// <param name="Alias"></nickname>
/// </summary>
public class TaskInEngineer
{
   public int Id {  get; init; } 
   public string? Alias {  get; set; }
   public override string ToString() => this.ToStringProperty();
}
