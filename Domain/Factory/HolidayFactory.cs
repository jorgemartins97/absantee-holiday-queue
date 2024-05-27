namespace Domain.Factory;

using Domain.Model;

public class HolidayFactory: IHolidayFactory
{
    public Holiday NewHoliday(long id, long colaboratorId,HolidayPeriod holidayPeriod)
    {
        return new Holiday(id,colaboratorId,holidayPeriod);
    }
}