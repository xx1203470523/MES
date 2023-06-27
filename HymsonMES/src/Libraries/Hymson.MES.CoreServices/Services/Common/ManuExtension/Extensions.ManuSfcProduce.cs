using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.Utils;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Hymson.MES.CoreServices.Services.Common.ManuExtension
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ManuSfcProduceExtensions
    {
        /// <summary>
        /// 条码资源关联校验
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="resourceId"></param>
        public static ManuSfcProduceEntity VerifyResource(this ManuSfcProduceEntity sfcProduceEntity, long resourceId)
        {
            // 当前资源是否对于的上
            if (sfcProduceEntity.ResourceId.HasValue  && sfcProduceEntity.ResourceId != resourceId)
                throw new CustomerValidationException(nameof(ErrorCode.MES16316)).WithData("SFC", sfcProduceEntity.SFC);

            return sfcProduceEntity;
        }

        /// <summary>
        /// 检查条码状态是否合法
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="produceStatus"></param>
        public static ManuSfcProduceEntity VerifySFCStatus(this ManuSfcProduceEntity sfcProduceEntity, SfcProduceStatusEnum produceStatus)
        {
            // 当前条码是否是被锁定
            if (sfcProduceEntity.Status == SfcProduceStatusEnum.Locked) throw new CustomerValidationException(nameof(ErrorCode.MES16314)).WithData("SFC", sfcProduceEntity.SFC);

            // 当前条码是否是已报废
            if (sfcProduceEntity.IsScrap == TrueOrFalseEnum.Yes) throw new CustomerValidationException(nameof(ErrorCode.MES16322)).WithData("SFC", sfcProduceEntity.SFC);

            // 当前工序是否是指定状态
            if (sfcProduceEntity.Status != produceStatus) throw new CustomerValidationException(nameof(ErrorCode.MES16313)).WithData("Status", produceStatus.GetDescription());

            return sfcProduceEntity;
        }

        /// <summary>
        /// 工序活动状态校验
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public static ManuSfcProduceEntity VerifyProcedure(this ManuSfcProduceEntity sfcProduceEntity, long procedureId)
        {
            // 产品编码是否和工序对应
            if (sfcProduceEntity.ProcedureId != procedureId) throw new CustomerValidationException(nameof(ErrorCode.MES16308));

            return sfcProduceEntity;
        }

        /// <summary>
        /// 工序锁校验
        /// </summary>
        /// <param name="sfcProduceBusinessEntity"></param>
        /// <param name="sfc"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public static ManuSfcProduceBusinessEntity? VerifyProcedureLock(this ManuSfcProduceBusinessEntity sfcProduceBusinessEntity, string sfc, long procedureId)
        {
            // 是否被锁定
            if (sfcProduceBusinessEntity == null) return sfcProduceBusinessEntity;
            if (sfcProduceBusinessEntity.BusinessType != ManuSfcProduceBusinessType.Lock) return sfcProduceBusinessEntity;

            var sfcProduceLockBo = JsonSerializer.Deserialize<SfcProduceLockBo>(sfcProduceBusinessEntity.BusinessContent);//sfcProduceBusinessEntity.BusinessContent
            if (sfcProduceLockBo == null) return sfcProduceBusinessEntity;

            if (sfcProduceLockBo.Lock == QualityLockEnum.InstantLock)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16314)).WithData("SFC", sfc);
            }

            if (sfcProduceLockBo.Lock == QualityLockEnum.FutureLock && sfcProduceLockBo.LockProductionId == procedureId)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16314)).WithData("SFC", sfc);
            }

            return sfcProduceBusinessEntity;
        }

        /// <summary>
        /// 验证条码
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="maskCodeRules"></param>
        /// <returns></returns>
        public static bool VerifyBarCode(this string barCode, IEnumerable<ProcMaskCodeRuleEntity> maskCodeRules)
        {
            // 对掩码规则进行校验
            foreach (var ruleEntity in maskCodeRules)
            {
                var rule = Regex.Replace(ruleEntity.Rule, "[?？]", ".");
                var pattern = $"{rule}";

                switch (ruleEntity.MatchWay)
                {
                    case MatchModeEnum.Start:
                        pattern = $"{rule}.+";
                        break;
                    case MatchModeEnum.Middle:
                        pattern = $".+{rule}.+";
                        break;
                    case MatchModeEnum.End:
                        pattern = $".+{rule}";
                        break;
                    case MatchModeEnum.Whole:
                        pattern = $"^{pattern}$";
                        break;
                    default:
                        break;
                }

                if (!Regex.IsMatch(barCode, pattern) ) return false;
            }

            return true;
        }

    }
}
