using DataModel.Model;
using DataModel.Repository;
using Domain.Model;

namespace WebApi.IntegrationTests.Helpers;

public static class Utilities
{
    public static void InitializeDbForTests(AbsanteeContext db)
    {
        var colaborators = GetSeedingColaboratorsDataModel();

        db.ColaboratorsId.AddRange(colaborators);
        db.SaveChanges();

        db.Holidays.AddRange(GetSeedingHolidaysDataModel(colaborators));
        db.SaveChanges();
    }

    public static void ReinitializeDbForTests(AbsanteeContext db)
    {
        db.Holidays.RemoveRange(db.Holidays);
        db.ColaboratorsId.RemoveRange(db.ColaboratorsId);
        InitializeDbForTests(db);
    }

    public static List<ColaboratorsIdDataModel> GetSeedingColaboratorsDataModel()
    {
        return new List<ColaboratorsIdDataModel>()
        {
            new ColaboratorsIdDataModel(new ColaboratorId(1)),
            new ColaboratorsIdDataModel(new ColaboratorId(2)),
            new ColaboratorsIdDataModel(new ColaboratorId(3)),
            new ColaboratorsIdDataModel(new ColaboratorId(4)),
        };
    }

    public static List<HolidayDataModel> GetSeedingHolidaysDataModel(List<ColaboratorsIdDataModel> colabs)
    {
        var holiday1 = new Holiday(1, 1, new HolidayPeriod(new DateOnly(2024, 1, 2), new DateOnly(2024, 1, 31)));
        holiday1.Id = 1;

        var holiday2 = new Holiday(2, 1, new HolidayPeriod(new DateOnly(2024, 3, 2), new DateOnly(2024, 3, 3)));
        holiday2.Id = 2;

        var holiday3 = new Holiday(3, 2, new HolidayPeriod(new DateOnly(2024, 2, 1), new DateOnly(2024, 3, 31)));
        holiday3.Id = 3;

        var holiday4 = new Holiday(4, 3, new HolidayPeriod(new DateOnly(2024, 1, 1), new DateOnly(2024, 1, 2)));
        holiday4.Id = 4;

         var holiday5 = new Holiday(4, 4, new HolidayPeriod(new DateOnly(2024, 1, 1), new DateOnly(2024, 1,30)));
        holiday4.Id = 5;

        return new List<HolidayDataModel>()
        {
            new HolidayDataModel(holiday1, colabs[0]),
            new HolidayDataModel(holiday2, colabs[0]),
            new HolidayDataModel(holiday3, colabs[1]),
            new HolidayDataModel(holiday4, colabs[2]),
            new HolidayDataModel(holiday5, colabs[3]),
        };
    }
}
