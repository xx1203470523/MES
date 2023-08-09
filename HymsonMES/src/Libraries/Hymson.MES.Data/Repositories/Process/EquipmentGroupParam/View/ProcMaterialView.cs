using Hymson.MES.Core.Domain.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    public class ProcEquipmentGroupParamView : ProcEquipmentGroupParamEntity
    {
        public string MaterialCode { get; set; }

        public string MaterialName { get; set; }

        public string ProcedureCode { get; set; }

        public string ProcedureName { get; set; }
    }
}
