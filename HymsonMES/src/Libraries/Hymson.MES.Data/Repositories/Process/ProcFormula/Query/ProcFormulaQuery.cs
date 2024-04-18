using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Process.Query
{
    /// <summary>
    /// 配方维护 查询参数
    /// </summary>
    public class ProcFormulaQuery
    {
        public long SiteId { get; set; }

        public long? MaterialId { get; set; }

        public long? ProcedureId { get; set; }

        public SysDataStatusEnum? Status { get; set; }
    }

    public class ProcFormulaByCodeAndVersion 
    {
        public long SiteId { get; set; }

        public string Code {  get; set; }

        public string Version { get; set; }
    }

    #region 顷刻

    /// <summary>
    /// 查询配方列表
    /// </summary>
    public class ProcFormulaListQueryDto
    {
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductCode { get; set; } = string.Empty;

        /// <summary>
        /// 设备id
        /// </summary>
        public long EquipmentId { get; set; }
    }

    /// <summary>
    /// 查询配方详情
    /// </summary>
    public class ProcFormulaDetailQueryDto
    {
        /// <summary>
        /// 产品型号
        /// </summary>
        public string FormulaCode { get; set; } = string.Empty;

        /// <summary>
        /// 设备id
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 根据编码和版本获取配方
    /// </summary>
    public class ProcFormlaGetByCodeVersionDto
    {
        /// <summary>
        /// 配方
        /// </summary>
        public string FormulaCode { get; set; } 

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 站点id
        /// </summary>
        public long SiteId { get; set; }
    }

    #endregion
}
