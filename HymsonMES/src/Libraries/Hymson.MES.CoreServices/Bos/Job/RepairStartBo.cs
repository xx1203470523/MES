using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;

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
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

    }

    /// <summary>
    /// 
    /// </summary>
    public class RepairStartResponseBo : JobResultBo
    {
        /// <summary>
        /// 在制
        /// </summary>
        public UpdateProcedureAndResourceCommand UpdateResourceCommand { get; set; } = new UpdateProcedureAndResourceCommand();

    }

}
