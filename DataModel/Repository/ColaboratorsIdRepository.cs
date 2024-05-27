namespace DataModel.Repository;

using Microsoft.EntityFrameworkCore;

using DataModel.Model;
using DataModel.Mapper;

using Domain.Model;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
//public class ColaboratorsIdRepository : GenericRepository<Domain?>, IColaboratorsIdRepository
public class ColaboratorsIdRepository : GenericRepository<ColaboratorsIdRepository>, IColaboratorsIdRepository
{    
    ColaboratorsIdMapper _colaboratorsIdMapper;
    public ColaboratorsIdRepository(AbsanteeContext context, ColaboratorsIdMapper mapper) : base(context!)
    {
        _colaboratorsIdMapper = mapper;
    }

    public async Task<IEnumerable<ColaboratorId>> GetColaboratorsIdAsync()
    {
        try {
            IEnumerable<ColaboratorsIdDataModel> colaboratorsIdDataModel = await _context.Set<ColaboratorsIdDataModel>()
                    .ToListAsync();

            IEnumerable<ColaboratorId> colaboratorsId = _colaboratorsIdMapper.ToDomain(colaboratorsIdDataModel);

            return colaboratorsId;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> ColaboratorExists(long id)
    {
        return await _context.Set<ColaboratorsIdDataModel>().AnyAsync(e => e.Id == id);
    }

    public async Task<ColaboratorId> Add(ColaboratorId colaboratorId)
    {
        try {
            ColaboratorsIdDataModel colaboratorsIdDataModel = _colaboratorsIdMapper.ToDataModel(colaboratorId);

            EntityEntry<ColaboratorsIdDataModel> colaboratorIdDataModelEntityEntry = _context.Set<ColaboratorsIdDataModel>().Add(colaboratorsIdDataModel);
            
            await _context.SaveChangesAsync();

            ColaboratorsIdDataModel colaboratorIdDataModelSaved = colaboratorIdDataModelEntityEntry.Entity;

            ColaboratorId colaboratorIdSaved = _colaboratorsIdMapper.ToDomain(colaboratorIdDataModelSaved);

            return colaboratorIdSaved;    
        }
        catch
        {
            throw;
        }
    }
}