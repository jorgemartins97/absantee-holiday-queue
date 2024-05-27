namespace Domain.Factory;

using Domain.Model;

public class HolidayPeriodFactory:IHolidayPeriodFactory
{
    public HolidayPeriod NewHolidayPeriod(DateOnly startDate, DateOnly endDate)
    {
        return new HolidayPeriod(startDate, endDate);
    }
}
