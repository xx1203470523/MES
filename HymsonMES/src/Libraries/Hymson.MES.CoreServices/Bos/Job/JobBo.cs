using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.Utils;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 
    /// </summary>
    public class JobBo
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        public string Name { get; set; } = "";
    }

    /// <summary>
    /// 
    /// </summary>
    public class PanelRequestBo
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string SFC { get; set; }

        /// <summary>
        /// 载具条码
        /// </summary>
        public string? VehicleCode { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CommonResponseBo
    {
        /// <summary>
        /// 请求源
        /// </summary>
        public RequestSourceEnum Source { get; set; } = RequestSourceEnum.EquipmentApi;

        /// <summary>
        /// 条码类型
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum Type { get; set; } = ManuFacePlateBarcodeTypeEnum.Product;

        /// <summary>
        /// 条码/载具数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 产品序列码/载具编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否尾工序
        /// </summary>
        public bool IsLastProcedure { get; set; } = true;

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcedureCode { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SfcStatusEnum Status { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class EmptyRequestBo { }

    /// <summary>
    /// 请求Bo
    /// </summary>
    public class InStationBo
    {
        /// <summary>
        /// 工厂Id
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
        /// 条码
        /// </summary>
        public IEnumerable<string>? SFCs { get; set; }

        /// <summary>
        /// 请求源
        /// </summary>
        public RequestSourceEnum Source { get; set; } = RequestSourceEnum.EquipmentApi;

        /// <summary>
        /// 条码类型
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum Type { get; set; } = ManuFacePlateBarcodeTypeEnum.Product;

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

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
    /// 请求Bo
    /// </summary>
    public class JobRequestBo : JobBaseBo
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 工序ID
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源ID
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        ///  容器ID
        /// </summary>
        public long ContainerId { get; set; }

        /// <summary>
        /// 不合格代码Id
        /// </summary>
        public long[]? UnqualifiedIds { get; set; }

        /// <summary>
        /// 不合格工艺路线id
        /// </summary>
        public long? BadProcessRouteId { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string? Remark { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 请求源
        /// </summary>
        public RequestSourceEnum Source { get; set; } = RequestSourceEnum.EquipmentApi;

        /// <summary>
        /// 条码类型
        /// </summary>
        public ManuFacePlateBarcodeTypeEnum Type { get; set; } = ManuFacePlateBarcodeTypeEnum.Product;

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime Time { get; set; } = HymsonClock.Now();

        /// <summary>
        /// 条码（面板用）
        /// </summary>
        public IEnumerable<PanelRequestBo>? PanelRequestBos { get; set; }

        /// <summary>
        /// 进站对象
        /// </summary>
        public IEnumerable<InStationRequestBo>? InStationRequestBos { get; set; }

        /// <summary>
        /// 出站对象 / 半成品
        /// </summary>
        public IEnumerable<OutStationRequestBo>? OutStationRequestBos { get; set; }
        /// <summary>
        /// 载具操作
        /// </summary>
        public VehicleBo? VehicleBo { get; set; }
        /// <summary>
        /// 条码转换BO
        /// </summary>
        public BarcodeChangeBo? BarcodeChangeBos { get; set; }
      
    }

    /// <summary>
    /// 响应Bo
    /// </summary>
    public class JobResponseBo
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// 影响行数
        /// </summary>
        public int Rows { get; set; } = 0;

        /// <summary>
        /// 内容
        /// </summary>
        public Dictionary<string, string> Content { get; set; } = new();

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; } = "";

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; } = HymsonClock.Now();

    }

}
