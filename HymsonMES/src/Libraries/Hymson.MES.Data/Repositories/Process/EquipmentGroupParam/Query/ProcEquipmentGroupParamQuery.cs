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

    #region 顷刻

    /// <summary>
    /// 根据产品型号和设备ID查询
    /// </summary>
    public class ProcEquipmentGroupParamEquProductQuery
    {
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long EquipmentId { get; set; }

        /// <summary>
        /// 类型 1-开机参数 2-配方参数
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// 根据编码查询激活的详情
    /// </summary>
    public class ProcEquipmentGroupParamCodeDetailQuery
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long SiteId { get; set; }
    }

    /// <summary>
    /// 开机参数校验查询
    /// </summary>
    public class ProcEquipmentGroupCheckQuery
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long SiteId { get; set; }
    }

    #endregion
}
