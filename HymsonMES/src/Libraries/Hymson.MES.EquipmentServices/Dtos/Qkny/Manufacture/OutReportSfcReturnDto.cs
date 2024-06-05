namespace Hymson.MES.EquipmentServices.Dtos.Qkny.Manufacture
{
    /// <summary>
    /// 生成条码返回结果
    /// </summary>
    public class OutReportSfcReturnDto
    {
        /// <summary>
        /// 条码列表
        /// </summary>
        public string Sfc { get; set; } = string.Empty;

        /// <summary>
        /// 工单号
        /// </summary>
        public string OrderCode { get; set; } = string.Empty;

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; } = string.Empty;

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; } = string.Empty;

        /// <summary>
        /// 物料批次
        /// </summary>
        public string MaterialBatch { get; set;} = string.Empty;

        /// <summary>
        /// 生产日期
        /// </summary>
        public string ManuDate { get; set;} = string.Empty;

        /// <summary>
        /// 有效日期
        /// </summary>
        public string EffectiveDate { get; set; } = string.Empty;

        ///// <summary>  
        ///// 生产总数  
        ///// </summary>  
        //public string TotalQty { get; set; } = string.Empty;

        ///// <summary>  
        ///// 合格数  
        ///// </summary>  
        //public string OkQty { get; set; } = string.Empty;

        ///// <summary>  
        ///// 不合格数  
        ///// </summary>  
        //public string NgQty { get; set; } = string.Empty;
    }
}
