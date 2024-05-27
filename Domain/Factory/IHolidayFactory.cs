namespace Domain.Factory;

using Domain.Model;

public interface IHolidayFactory
{
    Holiday NewHoliday(long id,long colaboratorId, HolidayPeriod holidayPeriod);
}