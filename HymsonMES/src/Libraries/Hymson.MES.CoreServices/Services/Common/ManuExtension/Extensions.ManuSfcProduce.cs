using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Bos.Job;
using Hymson.MES.CoreServices.Bos.Manufacture;
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
            if (sfcProduceEntity.ResourceId.HasValue && sfcProduceEntity.ResourceId != resourceId)
                throw new CustomerValidationException(nameof(ErrorCode.MES16316)).WithData("SFC", sfcProduceEntity.SFC);

            return sfcProduceEntity;
        }

        /// <summary>
        /// 条码资源关联校验
        /// </summary>
        /// <param name="sfcProduceEntities"></param>
        /// <param name="resourceId"></param>
        public static IEnumerable<ManuSfcProduceEntity> VerifyResource(this IEnumerable<ManuSfcProduceEntity> sfcProduceEntities, long resourceId)
        {
            // 当前资源是否对于的上
            if (sfcProduceEntities.Any(a => a.ResourceId.HasValue && a.ResourceId != resourceId))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16333));
            }

            return sfcProduceEntities;
        }


        /// <summary>
        /// 检查条码状态是否合法
        /// </summary>
        /// <param name="sfcProduceEntity"></param>
        /// <param name="sfcStatus"></param>
        /// <param name="localizationService"></param>
        public static ManuSfcProduceEntity VerifySFCStatus(this ManuSfcProduceEntity sfcProduceEntity, SfcStatusEnum sfcStatus, ILocalizationService localizationService)
        {
            // 当前条码是否是被锁定
            if (sfcProduceEntity.Status == SfcStatusEnum.Locked) throw new CustomerValidationException(nameof(ErrorCode.MES16314)).WithData("SFC", sfcProduceEntity.SFC);

            // 当前条码是否是已报废
            if (sfcProduceEntity.IsScrap == TrueOrFalseEnum.Yes) throw new CustomerValidationException(nameof(ErrorCode.MES16322)).WithData("SFC", sfcProduceEntity.SFC);

            // 当前工序是否是指定状态
            if (sfcProduceEntity.Status != sfcStatus)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16361))
                            .WithData("SFC", sfcProduceEntity.SFC)
                            .WithData("Current", localizationService.GetSFCStatusEnumDescription(sfcProduceEntity.Status))
                            .WithData("Status", localizationService.GetSFCStatusEnumDescription(sfcStatus));
            }

            return sfcProduceEntity;
        }

        /// <summary>
        /// 检查条码状态是否合法
        /// </summary>
        /// <param name="sfcProduceEntities"></param>
        /// <param name="sfcStatus"></param>
        /// <param name="localizationService"></param>
        public static IEnumerable<ManuSfcProduceEntity> VerifySFCStatus(this IEnumerable<ManuSfcProduceEntity> sfcProduceEntities, SfcStatusEnum sfcStatus, ILocalizationService localizationService)
        {
            // 当前条码是否是已报废
            if (sfcProduceEntities.Any(a => a.IsScrap == TrueOrFalseEnum.Yes)) throw new CustomerValidationException(nameof(ErrorCode.MES16324));

            // 当前条码是否是被锁定
            if (sfcProduceEntities.Any(a => a.Status == SfcStatusEnum.Locked)) throw new CustomerValidationException(nameof(ErrorCode.MES16325));

            // 当前条码是否是指定状态
            var sfcProduceEntitiesOfStatus = sfcProduceEntities.Where(a => a.Status != sfcStatus);
            if (sfcProduceEntitiesOfStatus.Any())
            {
                foreach (var item in sfcProduceEntitiesOfStatus)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16361))
                        .WithData("SFC", item.SFC)
                        .WithData("Current", localizationService.GetSFCStatusEnumDescription(item.Status))
                        .WithData("Status", localizationService.GetSFCStatusEnumDescription(sfcStatus));
                }
            }

            return sfcProduceEntities;
        }

        /// <summary>
        /// 检查条码的复投次数
        /// </summary>
        /// <param name="sfcProduceEntities"></param>
        /// <param name="cycle"></param>
        public static IEnumerable<ManuSfcProduceEntity> VerifySFCRepeatedCount(this IEnumerable<ManuSfcProduceEntity> sfcProduceEntities, int cycle)
        {
            // 复投次数验证
            var moreThanEntities = sfcProduceEntities.Where(a => a.RepeatedCount >= cycle);
            if (moreThanEntities.Any() == false) return sfcProduceEntities;

            foreach (var entity in moreThanEntities)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16360))
                    .WithData("Current", entity.RepeatedCount)
                    .WithData("Cycle", cycle)
                    .WithData("SFC", entity.SFC);
            }

            return moreThanEntities;
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
        /// 工序活动状态校验
        /// </summary>
        /// <param name="sfcProduceEntities"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public static IEnumerable<ManuSfcProduceEntity> VerifyProcedure(this IEnumerable<ManuSfcProduceEntity> sfcProduceEntities, long procedureId)
        {
            // 产品编码是否和工序对应
            if (sfcProduceEntities.Any(a => a.ProcedureId != procedureId))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES16308));
            }

            return sfcProduceEntities;
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
        /// 工序锁校验
        /// </summary>
        /// <param name="sfcProduceBusinessEntities"></param>
        /// <param name="SFCs"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        public static IEnumerable<ManuSfcProduceBusinessEntity>? VerifyProcedureLock(this IEnumerable<ManuSfcProduceBusinessEntity> sfcProduceBusinessEntities, IEnumerable<string> SFCs, long procedureId)
        {
            /*
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
            */
            // TODO
            return sfcProduceBusinessEntities;
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
                        pattern = $"^{rule}.+";
                        break;
                    case MatchModeEnum.Middle:
                        pattern = $".+{rule}.+";
                        break;
                    case MatchModeEnum.End:
                        pattern = $".+{rule}$";
                        break;
                    case MatchModeEnum.Whole:
                        pattern = $"^{pattern}$";
                        break;
                    default:
                        break;
                }

                if (!Regex.IsMatch(barCode, pattern)) return false;
            }

            return true;
        }

        /// <summary>
        /// 验证条码
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="maskCodeRules"></param>
        /// <returns></returns>
        public static string GetSFCStatusEnumDescription(this ILocalizationService localizationService, SfcStatusEnum statusEnum)
        {
            return statusEnum switch
            {
                SfcStatusEnum.lineUp => localizationService.GetResource($"{typeof(SfcStatusEnum).FullName}.{nameof(SfcStatusEnum.lineUp)}"),
                SfcStatusEnum.Activity => localizationService.GetResource($"{typeof(SfcStatusEnum).FullName}.{nameof(SfcStatusEnum.Activity)}"),
                SfcStatusEnum.InProductionComplete => localizationService.GetResource($"{typeof(SfcStatusEnum).FullName}.{nameof(SfcStatusEnum.InProductionComplete)}"),
                SfcStatusEnum.Complete => localizationService.GetResource($"{typeof(SfcStatusEnum).FullName}.{nameof(SfcStatusEnum.Complete)}"),
                SfcStatusEnum.Locked => localizationService.GetResource($"{typeof(SfcStatusEnum).FullName}.{nameof(SfcStatusEnum.Locked)}"),
                SfcStatusEnum.Scrapping => localizationService.GetResource($"{typeof(SfcStatusEnum).FullName}.{nameof(SfcStatusEnum.Scrapping)}"),
                _ => ""
            };
        }

        /// <summary>
        /// 转换BO对象
        /// </summary>
        /// <typeparam name="TBo"></typeparam>
        /// <param name="bo"></param>
        /// <returns></returns>
        public static TBo ToBo<TBo>(this JobBaseBo bo) where TBo : JobBaseBo
        {
            if (bo == null) throw new ArgumentNullException(nameof(bo));
            return AutoMapperConfiguration.Mapper.Map<TBo>(bo);
        }

    }
}
