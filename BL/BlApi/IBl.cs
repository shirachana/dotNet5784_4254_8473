namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    //public IMilestone Milestone { get; }
    public ITask Task { get; }
    public IUser User { get; }
    public DateTime? StartDate { set; get; }
    public DateTime? EndDate { set; get; }
    public void InitializeDB();
    public void ResetDB();

}

