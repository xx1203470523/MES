using Hymson.MES.Core.Attribute.Job;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateStatusCommand
    {
        /// <summary>
        ///Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcProduceStatusEnum Status { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>

    [JobProxy(typeof(IEnumerable<ManuSfcProduceEntity>))]
    public class MultiUpdateProduceSFCCommand : UpdateCommand
    {
        /// <summary>
        ///Id
        /// </summary>
        [Condition]
        [Ignore]
        [Field("Id")]
        public IEnumerable<long> Ids { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcProduceStatusEnum Status { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 复投次数;复投次数
        /// </summary>
        public int RepeatedCount { get; set; }

    }
}
