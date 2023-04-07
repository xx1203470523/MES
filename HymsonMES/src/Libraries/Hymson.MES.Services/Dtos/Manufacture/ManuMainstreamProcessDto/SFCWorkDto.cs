namespace Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto
{
    /// <summary>
    /// 作业Dto
    /// </summary>
    public class SFCWorkDto
    {
        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; }
    }
}
