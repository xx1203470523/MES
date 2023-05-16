﻿namespace Hymson.MES.EquipmentServices.Request.Feeding
{
    /// <summary>
    /// 请求参数（卸料）
    /// </summary>
    public class FeedingUnloadingRequest : BaseRequest
    {
        /// <summary>
        /// 卸料条码
        /// </summary>
        public string SFC { get; set; } = "";

        /// <summary>
        /// 卸料类型（2：代表剩余物料卸料；3：代表剩余物料卸料并报废）
        /// </summary>
        public int Type { get; set; }
    }
}
