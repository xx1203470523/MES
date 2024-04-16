using Hymson.MES.CoreServices.Bos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 产出确认
    /// </summary>
    public class OutputConfirmBo : CoreBaseBo
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 条码信息
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal QualifiedQty { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal UnQualifiedQty { get; set; }

        /// <summary>
        /// Ng代码
        /// </summary>
        public UnqualifiedBo[]? Unqualifieds { get; set; }
    }

    /// <summary>
    /// 不合格信息
    /// </summary>
    public record UnqualifiedBo
    {
        /// <summary>
        /// NG代码
        /// </summary>
        public long UnqualifiedId { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal UnqualifiedQty { get; set; }
    }
}
