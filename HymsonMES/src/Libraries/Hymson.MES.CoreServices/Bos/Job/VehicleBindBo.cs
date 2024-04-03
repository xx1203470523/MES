using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.CoreServices.Bos.Job
{
    /// <summary>
    /// 载具BO
    /// </summary>
    public class VehicleBo
    {
        /// <summary>
        /// 托盘码
        /// </summary>
        public string PalletNo { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public long EquipmentId { get; set; }
        public IEnumerable<string> SFCs { get; set; }

    }
    public class VehicleBindResponseBo
    {
        /// <summary>
        /// 载具条码明细集合
        /// </summary>
        public List<InteVehicleFreightStackEntity> Items { get; set; } = new List<InteVehicleFreightStackEntity>();
        public List<ManuSfcStepEntity> StepList { get; set; } = new List<ManuSfcStepEntity>();
        /// <summary>
        /// 载具位置明细集合
        /// </summary>
        public List<InteVehicleFreightEntity> Locations { get; set; } = new List<InteVehicleFreightEntity>();
        public List<InteVehicleFreightRecordEntity> Records { get; set; } = new List<InteVehicleFreightRecordEntity>();

    }
}
