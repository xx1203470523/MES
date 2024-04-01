using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcConversionFactorEntity : BaseEntity
    {
        /// <summary>
        /// 描述 :所属站点代码 
        /// 空值 : false  
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 描述 :工序ID
        /// 空值 : false  
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 描述 :物料ID
        /// 空值 : false  
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 转换系数
        /// 空值 : false  
        /// </summary>
        public string ConversionFactor { get; set; }

        /// <summary>
        /// 转换系数状态
        /// 空值 : false  
        /// </summary>
        public ManuSfcRepairDetailIsIsCloseEnum OpenStatus { get; set; }

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }
    }

}