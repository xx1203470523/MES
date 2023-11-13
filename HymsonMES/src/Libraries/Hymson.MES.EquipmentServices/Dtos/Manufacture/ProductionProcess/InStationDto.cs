using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.EquipmentServices.Dtos.Manufacture.ProductionProcess
{
    /// <summary>
    /// 进站
    /// </summary>
    public class InStationDto
    {
        /// <summary>
        /// 资源编码
        /// </summary>
        public string? ResourceCode { get; set; }

        /// <summary>
        /// 条码列表
        /// </summary>
        public IEnumerable<string>? SFCs { get; set; }
    }
}
