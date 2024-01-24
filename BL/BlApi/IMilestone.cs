namespace BlApi;
/// <summary>
/// Milestone in BLAPi
/// < func name="CreateSchedule"></creat the schedule of the project >
/// < func name="Read"></read  the Milestone details>
/// < func name="Update"></update Milestone that the function get>
/// </summary>
public interface IMilestone
{
    public int CreateSchedule();//not finish yet
    public BO.Milestone? Read(int id);
    public void Update(BO.Milestone item);
}
