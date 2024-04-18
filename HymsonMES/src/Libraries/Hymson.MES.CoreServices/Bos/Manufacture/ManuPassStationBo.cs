using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.CoreServices.Bos.Job;

namespace Hymson.MES.CoreServices.Bos.Manufacture
{
    /// <summary>
    /// 进站实体类（条码进站）
    /// </summary>
    public sealed class SFCInStationBo : CoreBaseBo
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }
    }

    /// <summary>
    /// 进站实体类（托盘进站）
    /// </summary>
    public sealed class VehicleInStationBo : CoreBaseBo
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 载具条码
        /// </summary>
        public IEnumerable<string> VehicleCodes { get; set; }
    }

    /// <summary>
    /// 出站实体类（条码出站）
    /// </summary>
    public sealed class SFCOutStationBo : CoreBaseBo
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        public IEnumerable<OutStationRequestBo> OutStationRequestBos { get; set; }  = new List<OutStationRequestBo>();
    }

    /// <summary>
    /// 出站实体类（托盘出站）
    /// </summary>
    public sealed class VehicleOutStationBo : CoreBaseBo
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 载具条码
        /// </summary>
        public IEnumerable<OutStationRequestBo> OutStationRequestBos { get; set; }
    }

    /// <summary>
    /// 中止实体类
    /// </summary>
    public sealed class SFCStopStationBo : CoreBaseBo
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public long ProcedureId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public long ResourceId { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public long? EquipmentId { get; set; }

        /// <summary>
        /// 产品序列码
        /// </summary>
        public IEnumerable<string> SFCs { get; set; }
    }

}
