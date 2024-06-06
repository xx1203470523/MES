using Hymson.MES.Core.Enums;

namespace Hymson.MES.Core.Constants.Manufacture
{
    /// <summary>
    /// 条码常量
    /// </summary>
    public static class ManuSfcStatus
    {
        /// <summary>
        /// 进站允许的状态（排队，活动中）
        /// </summary>
        public static readonly IEnumerable<SfcStatusEnum> InStationAllowStatus = new List<SfcStatusEnum>() {
            SfcStatusEnum.lineUp,
            SfcStatusEnum.Activity
        };

        /// <summary>
        /// 条码在制品状态为 排队 活动 在制完成
        /// </summary>
        public static readonly IEnumerable<SfcStatusEnum> SfcStatusInProcess = new List<SfcStatusEnum>() {
            SfcStatusEnum.lineUp,
            SfcStatusEnum.Activity,
            SfcStatusEnum.InProductionComplete
        };

        /// <summary>
        /// 禁止操作状态 报废 删除 无效 锁定 离脱
        /// </summary>
        public static readonly IEnumerable<SfcStatusEnum> ForbidSfcStatuss = new List<SfcStatusEnum>() {
            SfcStatusEnum.Scrapping,
            SfcStatusEnum.Delete,
            SfcStatusEnum.Invalid,
            SfcStatusEnum.Locked,
            SfcStatusEnum.Detachment
        };

        /// <summary>
        /// 禁止报废状态 报废 删除 无效 锁定
        /// </summary>
        public static readonly IEnumerable<SfcStatusEnum> ForbidScrapSfcStatuss = new List<SfcStatusEnum>() {
            SfcStatusEnum.Scrapping,
            SfcStatusEnum.Delete,
            SfcStatusEnum.Invalid,
            SfcStatusEnum.Locked
        };
    }
}
