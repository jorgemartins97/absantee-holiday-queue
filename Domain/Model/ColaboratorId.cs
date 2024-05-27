using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class ColaboratorId
    {
        public long colabId { get; set; }

        private ColaboratorId() { }

        // This constructor can be used by your code when creating new instances.
        public ColaboratorId(long ColabId)
        {
            this.colabId = ColabId;
        }


    }
}