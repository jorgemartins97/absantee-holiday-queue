namespace Domain.IRepository;

using Domain.Model;

public interface IHolidayRepository : IGenericRepository<Holiday>
{
    Task<bool> HolidayExists(long id);
    Task<IEnumerable<Holiday>> GetHolidaysByIdAsync(long id);
    Task<IEnumerable<Holiday>> GetHolidaysAsync();

    Task<Holiday> AddHoliday(Holiday holiday);
    Task<IEnumerable<Holiday>> GetHolidaysByColabIdAsync(long colabId);
    

    

}
