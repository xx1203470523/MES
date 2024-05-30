using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 数据实体（配方操作）   
    /// proc_formula_operation
    /// @author hjy
    /// @date 2023-12-15 02:08:48
    /// </summary>
    public class ProcFormulaOperationEntity : BaseEntity
    {
        /// <summary>
        /// 配方操作编码
        /// </summary>
        public string Code { get; set; }

       /// <summary>
        /// 配方操作名称
        /// </summary>
        public string Name { get; set; }

       /// <summary>
        /// 类型 0、新建 1、启用 2、保留 3、废除
        /// </summary>
        public SysDataStatusEnum Status { get; set; }

       /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

       /// <summary>
        /// 操作类型 1、物料 2、物料组 3、固定值 4、设定值 5、参数值
        /// </summary>
        public FormulaOperationTypeEnum Type { get; set; }

       /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

       
    }
}
