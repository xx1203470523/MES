using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Dtos.Process.LabelTemplate.Utility
{
    /// <summary>
    /// 
    /// </summary>
    public class LabelTemplateSourceDto
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public IEnumerable<LabelTemplateBarCodeDto> BarCodes { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string UserName { get; set; }
    }

    public class LabelTemplateBarCodeDto
    {

        /// <summary>
        /// 物料
        /// </summary>
        public long MateriaId { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string BarCode { get; set; }
    }
}
