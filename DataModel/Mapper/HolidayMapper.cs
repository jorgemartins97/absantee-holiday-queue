namespace DataModel.Mapper;

using DataModel.Model;
using Domain.Model;
using Domain.Factory;
using System.Linq;
using System;

public class HolidayMapper
{
    private IHolidayFactory _holidayFactory;

    public HolidayMapper(
        IHolidayFactory holidayFactory)
    {
        _holidayFactory = holidayFactory;
    }


    public Holiday ToDomain(HolidayDataModel holidayDM)
    {
        long id = holidayDM.Id;
        ColaboratorsIdDataModel colabId = holidayDM.colaboratorId;

        long cId = colabId.Id;

        IHolidayPeriodFactory _holidayPeriodFactory = new HolidayPeriodFactory();
        HolidayPeriod holidayPeriod = _holidayPeriodFactory.NewHolidayPeriod(holidayDM._startDate, holidayDM._endDate);
        

        Holiday holidayDomain = _holidayFactory.NewHoliday(id,cId,holidayPeriod);
        
        return holidayDomain;
    }
 
    public IEnumerable<Holiday> ToDomain(IEnumerable<HolidayDataModel> holidaysDM)
    {
        List<Holiday> holidaysDomain = new List<Holiday>();

        foreach(HolidayDataModel holidayDataModel in holidaysDM)
        {
            Holiday holidayDomain = ToDomain(holidayDataModel);

            holidaysDomain.Add(holidayDomain);
        }

        return holidaysDomain.AsEnumerable();
    }

    

    public HolidayDataModel ToDataModel(Holiday holiday, ColaboratorsIdDataModel colaboratorsIdDataModel)
    {
        var holidayDataModel = new HolidayDataModel
        {
            Id = holiday.Id,
            colaboratorId = colaboratorsIdDataModel,
            _startDate = holiday.HolidayPeriod.StartDate,
            _endDate = holiday.HolidayPeriod.EndDate
        };

        return holidayDataModel;
    }
   
}