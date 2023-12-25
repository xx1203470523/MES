/*
 *creator: Karl
 *
 *describe: 设备参数组 查询类 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-02 01:48:35
 */

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 设备参数组 查询参数
    /// </summary>
    public class ProcEquipmentGroupParamQuery
    {
        public long SiteId {  get; set; }

        public long? ProductId { get; set; }

        public long? ProcedureId { get; set;}

    }

    public class ProcEquipmentGroupParamCodeQuery 
    {
        public string Code { get; set; }

        public long SiteId { get; set; }
    }

    public class ProcEquipmentGroupParamRelatesInformationQuery
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 设备组ID
        /// </summary>
        public long EquipmentGroupId { get; set; }
    }
}
