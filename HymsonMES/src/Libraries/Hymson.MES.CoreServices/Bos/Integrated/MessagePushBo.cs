namespace Hymson.MES.CoreServices.Bos.Integrated
{
    /// <summary>
    /// BO对象（消息推送）
    /// </summary>
    public class MessagePushBo
    {
        /// <summary>
        /// 推送场景
        /// </summary>
        public int PushScene { get; set; }

        /// <summary>
        /// 消息编码
        /// </summary>
        public string Code { get; set; } = "";

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; } = "";

        /// <summary>
        /// 紧急程度;1、高2、中3、低
        /// </summary>
        public string Level { get; set; } = "";

        /// <summary>
        /// 事件类型名称
        /// </summary>
        public string EventTypeName { get; set; } = "";

        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; } = "";

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName { get; set; } = "";

        /// <summary>
        /// 产线名称
        /// </summary>
        public string WorkLineName { get; set; } = "";

        /// <summary>
        /// 资源名称
        /// </summary>
        public string? ResourceName { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? EquipmentName { get; set; }

        /// <summary>
        /// 触发人
        /// </summary>
        public string TriggerUser { get; set; } = "";

        /// <summary>
        /// 触发时间
        /// </summary>
        public string TriggerTime { get; set; } = "";

        /// <summary>
        /// 接收人
        /// </summary>
        public string ReceiveUser { get; set; } = "";

        /// <summary>
        /// 接收时间
        /// </summary>
        public string ReceiveTime { get; set; } = "";

        /// <summary>
        /// 评价时间
        /// </summary>
        public string EvaluateOn { get; set; } = "";

        /// <summary>
        /// 评价人
        /// </summary>
        public string EvaluateBy { get; set; } = "";
    }
}
