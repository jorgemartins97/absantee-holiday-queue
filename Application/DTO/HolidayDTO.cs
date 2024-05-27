namespace Application.DTO;

using Domain.Factory;
using Domain.Model;

public class HolidayDTO
{
	public long Id { get; set; }
	public long _colabId{ get; set; }

	public HolidayPeriodDTO _holidayPeriod { get; set; }

    public HolidayDTO() {
	}

	public HolidayDTO(long colabId,long id,HolidayPeriodDTO holidayPeriod)
	{
		Id = id;
		_colabId = colabId;
		_holidayPeriod = holidayPeriod;
	}

	static public HolidayDTO ToDTO(Holiday holiday) {
		long idColab = holiday.GetColaborator();
		long id = holiday.Id;
		HolidayPeriodDTO holidayPeriodDTO = HolidayPeriodDTO.ToDTO(holiday.HolidayPeriod);
		HolidayDTO holidayDTO = new HolidayDTO(idColab,id,holidayPeriodDTO);

		return holidayDTO;
	}

	static public IEnumerable<HolidayDTO> ToDTO(IEnumerable<Holiday> holidays)
	{
		List<HolidayDTO> holidaysDTO = new List<HolidayDTO>();

		foreach( Holiday holiday in holidays ) {
			HolidayDTO holidayDTO = ToDTO(holiday);

			holidaysDTO.Add(holidayDTO);
		}

		return holidaysDTO;
	}

	static public Holiday ToDomain(HolidayDTO holidayDTO) 
	{
		if (holidayDTO == null) 
		{
			throw new ArgumentException("holidayDTO must not be null");
		}

		HolidayPeriod holidayPeriod = HolidayPeriodDTO.ToDomain(holidayDTO._holidayPeriod);

		Holiday holiday = new Holiday(holidayDTO.Id,holidayDTO._colabId,holidayPeriod);

		
		// foreach (var periodDTO in holidayDTO.HolidayPeriods)
		// {
		//     HolidayPeriod period = HolidayPeriodDTO.ToDomain(periodDTO);
		//     holiday.AddHolidayPeriod(period);
		// }

		return holiday;
	}
}