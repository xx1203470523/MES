using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Domain.Process
{
    /// <summary>
    /// 标准参数表数据实体对象
    /// @author admin
    /// @date 2023-02-08
    /// </summary>
    public class ProcParameterEntity : BaseEntity
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 描述 :参数代码 
        /// 空值 : false  
        /// </summary>
        public string ParameterCode { get; set; }

        /// <summary>
        /// 描述 :参数名称 
        /// 空值 : false  
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// 描述 :参数单位（字典定义） 
        /// 空值 : false  
        /// </summary>
        public string? ParameterUnit { get; set; }

        /// <summary>
        /// 数据类型（字典定义） 
        /// </summary>
        public DataTypeEnum DataType { get; set; } = DataTypeEnum.Text;

        /// <summary>
        /// 描述 :说明 
        /// 空值 : true  
        /// </summary>
        public string? Remark { get; set; }

    }
}