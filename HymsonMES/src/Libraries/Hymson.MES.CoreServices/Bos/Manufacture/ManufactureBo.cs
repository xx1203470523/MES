using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 
    /// </summary>
    public record ManufactureBo
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime Time { get; set; } = HymsonClock.Now();

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
    /// 当前生成对象
    /// </summary>
    public class ManufactureRequestBo
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResourceCode { get; set; } = "";

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode { get; set; } = "";
    }

    /// <summary>
    /// 生产对象（ID）
    /// </summary>
    public class ManufactureIdsBo
    {
        /// <summary>
        /// 站点ID
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime Time { get; set; } = HymsonClock.Now();

    }

    /// <summary>
    /// 当前生成对象
    /// </summary>
    public class ManufactureResponseBo : ManufactureIdsBo
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public new long EquipmentId { get; set; }

    }

    /// <summary>
    /// 在制维修出站
    /// </summary>
    public class ManufactureRepairBo
    {
        /// <summary>
        /// 当前工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 返回工序ID
        /// </summary>
        public long ReturnProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string SFC { get; set; } = "";
    }

    /// <summary>
    /// 核心入口请求
    /// </summary>
    public class ConsumptionCoreEntryRequestBo
    {
        /// <summary>
        /// 产品序列码
        /// </summary>
        public ManuSfcProduceEntity SFCProduceEntity { get; set; }

        /// <summary>
        /// 已分组的物料库存集合
        /// </summary>
        public Dictionary<long, IGrouping<long, ManuFeedingEntity>> ManuFeedingsDict { get; set; }

        /// <summary>
        /// 进行消耗的物料对象（主物料对象）
        /// </summary>
        public MaterialDeductResponseBo MainMaterialBo { get; set; }

        /// <summary>
        /// 进行消耗的物料对象（当前物料对象）
        /// </summary>
        public MaterialDeductResponseBo CurrentMaterialBo { get; set; }

        /// <summary>
        /// 是否主物料
        /// </summary>
        public bool IsMainMaterial { get; set; } = true;

        /// <summary>
        /// 作业基础参数
        /// </summary>
        public ManufactureIdsBo IdsBo { get; set; }

        /// <summary>
        /// 步骤ID
        /// </summary>
        public long SFCStepId { get; set; }

    }

    /// <summary>
    /// 扣料
    /// </summary>
    public class MaterialDeductRequestBo
    {
        /// <summary>
        /// 站点Id
        /// </summary>
        public long SiteId { get; set; }

        /// <summary>
        /// 工序id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// BOMId
        /// </summary>
        public long ProductBOMId { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }
    }

    /// <summary>
    /// 扣料
    /// </summary>
    public class MaterialDeductResponseBo : MaterialDeductItemBo
    {
        /// <summary>
        /// 替代料集合
        /// </summary>
        public IEnumerable<MaterialDeductItemBo> ReplaceMaterials { get; set; } = new List<MaterialDeductItemBo>();

    }

    /// <summary>
    /// 扣料
    /// </summary>
    public class MaterialDeductResponseSummaryBo
    {
        /// <summary>
        /// 属于半成品的物料
        /// </summary>
        public IEnumerable<ProcBomDetailEntity> SmiFinisheds { get; set; }

        /// <summary>
        /// 即将扣料的物料数据
        /// </summary>
        public List<MaterialDeductResponseBo> InitialMaterials { get; set; }

    }

    /// <summary>
    /// 扣料项
    /// </summary>
    public class MaterialDeductItemBo
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// 物料Code
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 用量
        /// </summary>
        public decimal Usages { get; set; }

        /// <summary>
        /// 损耗
        /// </summary>
        public decimal? Loss { get; set; }

        /// <summary>
        /// 消耗系数
        /// </summary>
        public decimal ConsumeRatio { get; set; } = 100;

        /// <summary>
        /// 数据收集方式（用之前要确认是否有赋值）
        /// </summary>
        public MaterialSerialNumberEnum? DataCollectionWay { get; set; }

        /// <summary>
        /// 数据收集方式（用之前要确认是否有赋值）
        /// </summary>
        public MaterialSerialNumberEnum? SerialNumber { get; set; }
    }

    /// <summary>
    /// 容器包装
    /// </summary>
    public class ManufactureContainerBo
    {
        /// <summary>
        /// 容器ID
        /// </summary>
        public long ContainerId { get; set; }
    }

    /// <summary>
    /// 判断上一个工序是否是随机工序
    /// </summary>
    public class IsRandomPreProcedureBo
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public long ProcessRouteId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }
    }

    /// <summary>
    /// 获取生产工单Bo
    /// </summary>
    public class GetProduceWorkOrderByIdBo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// 是否验证激活
        /// </summary>
        public bool IsVerifyActivation { get; set; } = true;
    }


}
