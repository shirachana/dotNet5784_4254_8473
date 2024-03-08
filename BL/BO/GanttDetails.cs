

namespace BO;

public class GanttDetails
{
    public int TaskId {  get; set; }
    public string?TaskName { get; set;}
    public List<BO.TaskInList>? Dependencies { get; set; }
    public int DaysFromProjectStart {  get; set; }
    public int TasksDays { get; set; }
    public int DaysUntilProjectEnd { get; set; }

}
