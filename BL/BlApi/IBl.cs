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

    #region Time Management

    public DateTime Clock { get; }

    public void AdvanceTimeByYear(int years);

    public void AdvanceTimeByDay(int days);

    public void AdvanceTimeByHour(int hours);

    public void InitializeTime(DateTime initialDateTime);

    #endregion
}

