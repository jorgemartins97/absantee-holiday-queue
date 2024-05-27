namespace Application.Services;

using Domain.Model;
using Application.DTO;

using Microsoft.EntityFrameworkCore;
using DataModel.Repository;
using Domain.IRepository;
using Domain.Factory;
using DataModel.Model;
using Gateway;
using System;
using System.Collections;
using Microsoft.IdentityModel.Tokens;


public class HolidayService {

    private readonly AbsanteeContext _context;
    private readonly IHolidayRepository _holidayRepository;
    private readonly IColaboratorsIdRepository _colaboratorsIdRepository;
    private readonly IHolidayPeriodFactory _holidayPeriodFactory;
    private readonly HolidayAmpqGateway _holidayAmqpGateway;


    
    public HolidayService(IHolidayRepository holidayRepository, IHolidayPeriodFactory holidayPeriodFactory, HolidayAmpqGateway holidayAmqpGateway,IColaboratorsIdRepository colaboratorsIdRepository) {
        _holidayRepository = holidayRepository;
        _holidayPeriodFactory = holidayPeriodFactory;
        _holidayAmqpGateway=holidayAmqpGateway;
        _colaboratorsIdRepository = colaboratorsIdRepository;
    }

    public async Task<IEnumerable<HolidayDTO>> GetAll()
    {    
        IEnumerable<Holiday> holidays = await _holidayRepository.GetHolidaysAsync();

        IEnumerable<HolidayDTO> holidaysDTO = HolidayDTO.ToDTO(holidays);

        return holidaysDTO;
    }

    public async Task<IEnumerable<HolidayDTO>> GetHolidayById(long id)
    {    
        IEnumerable<Holiday> holidays = await _holidayRepository.GetHolidaysByIdAsync(id);

        IEnumerable<HolidayDTO> holidaysDTO = HolidayDTO.ToDTO(holidays);

        return holidaysDTO;
    }

    public async Task<HolidayDTO> Add(HolidayDTO holidayDto, List<string> errorMessages)
    {
        bool bExists = await _holidayRepository.HolidayExists(holidayDto.Id);
        bool colabExists = await _colaboratorsIdRepository.ColaboratorExists(holidayDto._colabId);
        Console.WriteLine($"add entered");
        if(bExists) {
            errorMessages.Add("Holiday already exists");
             Console.WriteLine($"add entered1");
            return null;
        }
        if(!colabExists) {
            errorMessages.Add("Colab doesn't exist");
            Console.WriteLine($"add entered2");
            return null;
             
        }

        Holiday holiday = HolidayDTO.ToDomain(holidayDto);

        holiday = await _holidayRepository.AddHoliday(holiday);

        HolidayDTO holidayDTO = HolidayDTO.ToDTO(holiday);

        return holidayDTO;
    }


    public async Task<IEnumerable<HolidayPeriodDTO>> GetHolidayPeriodsOnHolidayById(long colabId, DateOnly startDate, DateOnly endDate,List<string> errorMessages)
    {

        IEnumerable<Holiday> holidays = await _holidayRepository.GetHolidaysByColabIdAsync(colabId);

        List<HolidayPeriod> holidayPeriods = new List<HolidayPeriod>();
        
        if(holidays.IsNullOrEmpty()) {
            errorMessages.Add("No holidays found for this colaborator.");
            return null;
        }

        foreach(Holiday holiday in holidays){
            HolidayPeriod holidayPeriod = holiday.HolidayPeriod;
            if(holidayPeriod.EndDate > startDate && holidayPeriod.StartDate< endDate){
                holidayPeriods.Add(holidayPeriod);
            }
        }
        
        return HolidayPeriodDTO.ToDTO(holidayPeriods);


    }
    //fazer em vez disto, um get do repositório dos HolidayPeriods, getHolidayPeriodsByColabId no repo?,linha 133
    //fazer o foreach todo no repo, passar tudo para o repositório da HolidayPeriod?
    public async Task<List<long>> GetColabsComFeriasSuperioresAXDias(long xDias,List<string> errorMessages)
    {
        IEnumerable<ColaboratorId> lista = await _colaboratorsIdRepository.GetColaboratorsIdAsync();

        List<long> colabsComFeriasSuperioresAXDias = new List<long>();

        foreach(ColaboratorId colabId in lista){
            IEnumerable<Holiday> holidays = await _holidayRepository.GetHolidaysByColabIdAsync(colabId.colabId);

            foreach(Holiday holiday in holidays){
                long days = holiday.HolidayPeriod.GetNumberOfDays();
                if(days>xDias){
                    colabsComFeriasSuperioresAXDias.Add(colabId.colabId);
                    break;
                }
            }
        }
        return colabsComFeriasSuperioresAXDias;

    }

    

    

}