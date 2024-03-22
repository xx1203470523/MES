using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;

namespace Hymson.MES.Data.Repositories.Manufacture
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
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 操作人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdatedOn { get; set; }
    }

    public class UpdateManuSfcProduceStatusByIdCommand : UpdateCommand
    {
        /// <summary>
        /// 条码在制品id列表
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 条码当前状态
        /// </summary>
        public SfcStatusEnum CurrentStatus { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>

    //[JobProxy(typeof(IEnumerable<ManuSfcProduceEntity>))]
    public class MultiUpdateProduceSFCCommand : UpdateCommand
    {
        /// <summary>
        ///Id
        /// </summary>
        //[Condition]
        //[Ignore]
        //[Field("Id")]
        public IEnumerable<long> Ids { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcStatusEnum Status { get; set; }

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

    /// <summary>
    /// 更新信息
    /// </summary>
    public class UpdateProduceInStationSFCCommand : UpdateCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public SfcStatusEnum CurrentStatus { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long? ResourceId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long? ProcedureId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 循环次数
        /// </summary>
        public int RepeatedCount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UpdateManuSfcProduceQtyByIdCommand : UpdateCommand
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }
    }
}
