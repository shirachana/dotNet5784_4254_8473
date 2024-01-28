namespace BlApi;
/// <summary>
/// Task in BLAPi
/// < func name="ReadAll"></read all the list of the tasks >
/// < func name="Read"></read  the task details>
/// < func name="Create"></add another task>
/// < func name="Update"></update task that the function get>
/// < func name="Delete"></delete  the task details with the id>
/// <func name="UpdateStartDate"> </func> that get id and date to start the task and update the tsaks id
/// </summary>
public interface ITask
{
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.Task, bool>? filter = null);//not sure
    public BO.Task? Read(int id);
    public int Create(BO.Task? item);
    public void Update(BO.Task item);
    public void Delete(int id);
    public void UpdateStartDate(int id, DateTime startDate);
}
