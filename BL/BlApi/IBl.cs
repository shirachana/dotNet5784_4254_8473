namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    //public IMilestone Milestone { get; }
    public ITask Task { get; }
    public DateTime? GetStartDay();
    public void SetStartDay(DateTime dateTime);
}

