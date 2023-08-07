using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServicesTests.Dtos
{
    public class EquipmentInfoDto
    {
        public long Id { get; set; }
        public long FactoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
