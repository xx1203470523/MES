namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseOPEntity
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int index { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime? RDate { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string YDate { get; set; }

        /// <summary>
        /// 工序名
        /// </summary>
        public string OP { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 工序完成时间
        /// </summary>
        public string CycleTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Linegroup { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Pallet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Result { get; set; }

    }
}
