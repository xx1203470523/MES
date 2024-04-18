using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;

namespace Hymson.MES.Data.Repositories.Manufacture
{
    public class ManuSfcProduceView
    {
        /// <summary>
        ///  唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 锁;1：未锁定；2：即时锁；3：将来锁；
        /// </summary>
        public QualityLockEnum? Lock { get; set; }

        /// <summary>
        /// 未来锁工序id
        /// </summary>
        public long? LockProductionId { get; set; }

        /// <summary>
        /// 工艺路线
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 当前工序
        /// </summary>
        public long? ProcedureId { get; set; }
        /// <summary>
        /// 产品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string OrderCode { get; set; }

        /// <summary>
        /// 工序编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>

        public string MaterialName { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 是否报废
        /// </summary>
        public TrueOrFalseEnum? IsScrap { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResCode { get; set; }
    }

    public class ManuSfcProduceSelectView
    {
        /// <summary>
        ///  唯一标识
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sfc { get; set; }

        /// <summary>
        /// 锁;1：未锁定；2：即时锁；3：将来锁；
        /// </summary>
        public QualityLockEnum? Lock { get; set; }

        /// <summary>
        /// 未来锁工序id
        /// </summary>
        public long? LockProductionId { get; set; }


        /// <summary>
        /// 工艺路线
        /// </summary>
        public long? ProcessRouteId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long? ProductBOMId { get; set; }

        /// <summary>
        /// 当前工序
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 状态;1：排队；2：活动；
        /// </summary>
        public SfcStatusEnum Status { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }


        // 2023.10.24 COPY FROM XINSHIJIE
        /// <summary>
        /// 
        /// </summary>
        public TrueOrFalseEnum? IsScrap { get; set; }

        /// <summary>
        /// 条码数量
        /// </summary>
        public decimal Qty { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ManuSfcProduceInfoView : ManuSfcProduceEntity
    {
        public long SfcInfoId { get; set; }
    }

    public class ManuSfcAboutInfoView : ManuSfcEntity 
    {
        /// <summary>
        /// 工单ID
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set;}

        /// <summary>
        /// 工单编码
        /// </summary>
        public string WorkOrderCode { get; set; }

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public long ProcessRouteId { get; set;}

        /// <summary>
        /// BomId
        /// </summary>
        public long ProductBomId {  get; set; }
    }

    #region 顷刻

    /// <summary>
    /// 在制品信息+产品型号
    /// </summary>
    public class ManuSfcProductMaterialView : ManuSfcProduceEntity
    {
        /// <summary>
        /// 产品型号
        /// </summary>
        public string MaterialCode { get; set; }
    }

    #endregion

}
