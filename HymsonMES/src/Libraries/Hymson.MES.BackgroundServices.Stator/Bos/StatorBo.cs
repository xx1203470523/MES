using Hymson.MES.Core.Domain.Manufacture;

namespace Hymson.MES.BackgroundServices.Stator
{
    /// <summary>
    /// 基类（响应）
    /// </summary>
    public class StatorSummaryBo
    {
        /// <summary>
        /// 集合（条码）
        /// </summary>
        public List<ManuSfcEntity> ManuSFCEntities { get; set; } = new();

        /// <summary>
        /// 集合（条码信息）
        /// </summary>
        public List<ManuSfcInfoEntity> ManuSFCInfoEntities { get; set; } = new();

        /// <summary>
        /// 集合（步骤信息）
        /// </summary>
        public List<ManuSfcStepEntity> ManuSfcStepEntities { get; set; } = new();

        /// <summary>
        /// 集合（流转信息）
        /// </summary>
        public List<ManuSfcCirculationEntity> ManuSfcCirculationEntities { get; set; } = new();

        /// <summary>
        /// 集合（产品不良记录）
        /// </summary>
        public List<ManuProductBadRecordEntity> ManuProductBadRecordEntities { get; set; } = new();

        /// <summary>
        /// 集合（产品NG记录）
        /// </summary>
        public List<ManuProductNgRecordEntity> ManuProductNgRecordEntities { get; set; } = new();

    }

}
