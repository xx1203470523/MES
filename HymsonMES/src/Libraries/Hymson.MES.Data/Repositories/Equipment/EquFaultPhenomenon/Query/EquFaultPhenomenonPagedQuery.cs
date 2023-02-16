using Hymson.Infrastructure;

namespace Hymson.MES.Data.Repositories.Equipment.EquFaultPhenomenon.Query
{
    /// <summary>
    /// 分页参数（设备故障现象）
    /// </summary>
    public class EquFaultPhenomenonPagedQuery : PagerInfo
    {
        /// <summary>
        /// 站点编码 
        /// </summary>
        public string SiteCode { get; set; } = "";

        /// <summary>
        /// 编号（设备故障现象）
        /// </summary>
        public string FaultPhenomenonCode { get; set; } = "";

        /// <summary>
        /// 名称（设备故障现象）
        /// </summary>
        public string FaultPhenomenonName { get; set; } = "";

        /// <summary>
        /// 设备组名称
        /// </summary>
        public string EquipmentGroupName { get; set; } = "";

        /// <summary>
        /// 使用状态 0-禁用 1-启用（设备故障现象）
        /// </summary>
        public int UseStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = "";
    }
}
