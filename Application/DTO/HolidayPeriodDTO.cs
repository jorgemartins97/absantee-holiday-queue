namespace Application.DTO;

using Domain.Model;

public class HolidayPeriodDTO
{
	//pretende-se não exteriorizar o id de persistência
	//public long Id { get; set; }

	// atenção: embora possa ser chave única, email não deve servir de chave primária para foreign keys; está assim para servir de exemplo.
	public DateOnly StartDate { get; set; }
	public DateOnly EndDate { get; set; }
	
	public HolidayPeriodDTO() {
	}

	public HolidayPeriodDTO(DateOnly startDate, DateOnly endDate)
	{
		//Id = id;
		StartDate = startDate;
		EndDate = endDate;
	}

	static public HolidayPeriodDTO ToDTO(HolidayPeriod holidayPeriod) {

		HolidayPeriodDTO holidayPeriodDTO = new HolidayPeriodDTO(holidayPeriod.StartDate, holidayPeriod.EndDate);

		return holidayPeriodDTO;
	}

	static public IEnumerable<HolidayPeriodDTO> ToDTO(IEnumerable<HolidayPeriod> holidayPeriods)
	{
		List<HolidayPeriodDTO> holidayPeriodDTOs = new List<HolidayPeriodDTO>();

		foreach( HolidayPeriod holidayPeriod in holidayPeriods ) {
			HolidayPeriodDTO holidayPeriodDTO = ToDTO(holidayPeriod);

			holidayPeriodDTOs.Add(holidayPeriodDTO);
		}

		return holidayPeriodDTOs;
	}

	static public HolidayPeriod ToDomain(HolidayPeriodDTO holidayPeriodDTO) {
		
		HolidayPeriod holidayPeriod = new HolidayPeriod(holidayPeriodDTO.StartDate, holidayPeriodDTO.EndDate);

		return holidayPeriod;
	}

}