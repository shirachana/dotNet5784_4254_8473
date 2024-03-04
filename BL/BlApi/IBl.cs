namespace BlApi;
public interface IBl
{
    public IEngineer Engineer { get; }
    public ITask Task { get; }
    public IUser User { get; }
    public void SetProjectStartDate(DateTime? startDate);
    public DateTime? GetProjectStartDate();
    public void SetProjectEndDate(DateTime? endDate);
    public DateTime? GetProjectEndDate();

    public void InitializeDB();
    public void ResetDB();
    public DateTime? Schedule(int taskId, DateTime? date);
    public void AutoScedule();

    #region Time Management

    public DateTime Clock { get; }

    public void AdvanceTimeByYear(int years);

    public void AdvanceTimeByDay(int days);

    public void AdvanceTimeByHour(int hours);

    public void InitializeTime(DateTime initialDateTime);

    #endregion
}

