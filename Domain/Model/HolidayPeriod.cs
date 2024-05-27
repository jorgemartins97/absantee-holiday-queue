namespace Domain.Model;

public class HolidayPeriod
{
    public long Id { get; set; }
    private DateOnly _startDate;
    private DateOnly _endDate;

    public DateOnly StartDate
    {
        get { return _startDate; }
    }

    public DateOnly EndDate
    {
        get { return _endDate; }
    }

    // EF Core requires a parameterless constructor for mapping purposes.
    private HolidayPeriod() { }

    // This constructor can be used by your code when creating new instances.
    public HolidayPeriod(DateOnly startDate, DateOnly endDate)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException("invalid arguments: start date >= end date.");
        }

        this._startDate = startDate;
        this._endDate = endDate;
    }



	public bool IsStartDateIsValid(DateOnly startDate, DateOnly endDate)
	{
		if( startDate >= endDate ) 
		{
			return false;
		}
		return true;
	}

	public int GetNumberOfDays()
	{
		int startDateDays = _startDate.DayNumber;
		int endDateDays = _endDate.DayNumber;
		return endDateDays - startDateDays;
	}
}

