using Hymson.Infrastructure;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process.Resource
{
    /// <summary>
    /// 资源维护表查询对象
    /// </summary>
    public class ProcResourcePagedQuery : PagerInfo
    {
        /// <summary>
        /// 描述 :工序编码 
        /// 空值 : false  
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 描述 :资源代码 
        /// 空值 : false  
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 描述 :资源名称 
        /// 空值 : false  
        /// </summary>
        public string ResName { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public string ResType { get; set; }

        /// <summary>
        /// 资源类型id
        /// </summary>
        public long? ResTypeId { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 产线ID
        /// </summary>
        public long? WorkCenterLineId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProcResourceQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public long[] IdsArr { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; }

        /// <summary>
        /// 资源类型Id
        /// </summary>
        public long? ResTypeId { get; set; }

        /// <summary>
        /// 资源编码列表
        /// </summary>
        public string[]? ResCodes { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ProcResourceByEquipmentIdsQuery
    {
        /// <summary>
        /// 站点
        /// </summary>
        public long SiteId { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public long[] EquipmentIds { get; set; }
    }

    /// <summary>
    /// 资源维护表：根据工序查询资源列表
    /// </summary>
    public class ProcResourceProcedurePagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点id
        /// </summary>
        public long? SiteId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string? ProcedureCode { get; set; }

        /// <summary>
        /// 描述 :状态 
        /// 空值 : false  
        /// </summary>
        public SysDataStatusEnum? Status { get; set; }

        /// <summary>
        /// 描述 :资源代码 
        /// 空值 : false  
        /// </summary>
        public string? ResCode { get; set; }

        /// <summary>
        /// 描述 :资源名称 
        /// 空值 : false  
        /// </summary>
        public string? ResName { get; set; }

        /// <summary>
        /// 资源类型编码
        /// </summary>
        public string? ResType { get; set; }
    }

    /// <summary>
    /// 资源维护表查询对象
    /// </summary>
    public class ProcResourcePagedlineIdAndProcProcedureIdQuery : PagerInfo
    {
        /// <summary>
        /// 描述 :资源代码 
        /// 空值 : false  
        /// </summary>
        public string? ResCode { get; set; }

        /// <summary>
        /// 描述 :资源名称 
        /// 空值 : false  
        /// </summary>
        public string? ResName { get; set; }

        /// <summary>
        /// 产线ID
        /// </summary>
        public long? WorkCenterLineId { get; set; }

        /// <summary>
        /// 产线ID
        /// </summary>
        public  IEnumerable<long?>  WorkCenterLineIds { get; set; }

        /// <summary>
        /// 产线ID
        /// </summary>
        public long? ResTypeId { get; set; }
    }
}
