using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;

namespace DataModel.Model
{
    public class ColaboratorsIdDataModel
    {
        public long Id { get; set; }

        public ColaboratorsIdDataModel() {}

        public ColaboratorsIdDataModel(ColaboratorId colaboratorId)
        {
            Id = colaboratorId.colabId;
        }
    }
}