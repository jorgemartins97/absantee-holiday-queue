using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;

namespace Domain.IRepository
{
    public interface IColaboratorsIdRepository
    {
        Task<IEnumerable<ColaboratorId>> GetColaboratorsIdAsync();

        Task<ColaboratorId> Add(ColaboratorId id);

         Task<bool> ColaboratorExists(long id);
        
    }
}