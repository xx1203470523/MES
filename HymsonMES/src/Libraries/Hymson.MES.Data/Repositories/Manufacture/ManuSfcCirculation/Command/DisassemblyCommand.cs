using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcCirculation.Command
{
    /// <summary>
    /// 在制品拆解更新实体类
    /// </summary>
    public class DisassemblyCommand
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 流转类型
        /// </summary>
        public SfcCirculationTypeEnum CirculationType { get; set; }

        public TrueOrFalseEnum IsDisassemble {get;set;}

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }
}
