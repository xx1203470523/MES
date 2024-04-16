using Hymson.MES.CoreServices.Bos.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Services.Dtos.Manufacture.ManuSfcOperateDto
{
    public  class OutputConfirmDto
    {
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
}
