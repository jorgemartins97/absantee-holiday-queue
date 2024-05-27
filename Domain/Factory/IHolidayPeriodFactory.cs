namespace Domain.Factory;

using Domain.Model;

public interface IHolidayPeriodFactory
{
    HolidayPeriod NewHolidayPeriod(DateOnly startDate, DateOnly endDate);
}