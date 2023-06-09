namespace Hymson.MES.CoreServices.Dtos.Manufacture.ManuCommon.ManuCommon
{
    /// <summary>
    /// 工艺路线详情
    /// </summary>
    public class ProcessRouteDetailDto
    {
        public long key { set; get; }
        /// <summary>
        /// 工艺路线索引
        /// </summary>
        public IList<long> ProcedureIds { get; set; }
    }
}
