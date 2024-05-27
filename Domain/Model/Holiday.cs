namespace Domain.Model;

using Domain.Factory;

public class Holiday : IHoliday
{

	public long Id {get; set;}
	private long colaboratorId{get; set;}

	private HolidayPeriod _holidayPeriod;

	public HolidayPeriod HolidayPeriod
	{
		get { return _holidayPeriod; }
	}

	public Holiday(long ColabId)
	{
		if (ColabId != null)
		{
			colaboratorId = ColabId;
		}
		else
			throw new ArgumentException("Invalid argument: colaboratorId must be non null");
	}

	public Holiday(long id, long ColabId)
	{
		if (ColabId != null)
		{
			colaboratorId = ColabId;
			Id = id;
		}
		else
			throw new ArgumentException("Invalid argument: colaboratorId must be non null");
	}

	public Holiday(long id, long ColabId,HolidayPeriod holidayPeriod)
	{
		if (ColabId != null)
		{
			colaboratorId = ColabId;
			Id = id;
			_holidayPeriod = holidayPeriod;
		}
		else
			throw new ArgumentException("Invalid argument: colaboratorId must be non null");
	}

	public long GetColaborator()
	{
		return colaboratorId;
	}

	public bool HasColaborador(long colabId)
	{
		return colaboratorId == colabId;
	}
}