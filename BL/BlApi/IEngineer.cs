namespace BlApi;
/// <summary>
/// Engineer in BLAPi
/// < func name="ReadAll"></read all the engineers details>
/// < func name="Read"></read  the engineer details>
/// < func name="Create"></add another engineer>
/// < func name="Update"></update engineer that the function get>
/// < func name="Delete"></delete  the engineer details with the id>
/// </summary>
public interface IEngineer
{
  public IEnumerable<BO.EngineerInTask> ReadAll();//not sure
  public BO.Engineer? Read(int id);
  public int Create(BO.Engineer? item);
  public void Update(BO.Engineer item);
  public void Delete(int id);

}
