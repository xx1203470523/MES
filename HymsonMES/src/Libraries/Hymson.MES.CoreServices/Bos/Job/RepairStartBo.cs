namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class RepairStartRequestBo : JobBaseBo
    {

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }
        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RepairStartResponseBo
    {

    }

}
