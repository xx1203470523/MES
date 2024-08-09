using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Mavel;

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
        public string? Remark { get; set; } = "";

        /// <summary>
        /// 是否是CC项
        /// </summary>
        public TrueOrFalseEnum? IsCc { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 是否是Sc项
        /// </summary>
        public TrueOrFalseEnum? IsSc { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 是否是SPC项目
        /// </summary>
        public TrueOrFalseEnum? IsSpc { get; set; } = TrueOrFalseEnum.No;

        /// <summary>
        /// 未维护
        /// </summary>
        public Category01Enum? Category01 { get; set; } = Category01Enum.No;

        /// <summary>
        /// 是否推送
        /// </summary>
        public TrueOrFalseEnum IsPush { get; set; } = TrueOrFalseEnum.No;
    }
}