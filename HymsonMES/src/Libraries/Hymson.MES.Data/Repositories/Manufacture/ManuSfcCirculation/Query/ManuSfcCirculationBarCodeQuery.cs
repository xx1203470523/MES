using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Query
{
    public class ManuSfcCirculationBarCodeQuery
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 流转前产品条码
        /// </summary>
        public string[]? Sfcs { get; set; }

        /// <summary>
        /// 流转后条码
        /// </summary>
        public string[]? CirculationBarCodes { get; set; }

        /// <summary>
        /// 流转类型;1：拆分；2：合并；3：转换;4：消耗;5：拆解;6：组件添加 7.换件
        /// </summary>
        public SfcCirculationTypeEnum? CirculationType { get; set; }

        /// <summary>
        /// 是否拆解(0:未拆解，1：拆解)
        /// </summary>
        public TrueOrFalseEnum? IsDisassemble { get; set; }

        /// <summary>
        /// 当前状态;1：排队；2：激活；3：完工；
        /// </summary>
        public SfcProduceStatusEnum? CurrentStatus { get; set; }
    }
}
