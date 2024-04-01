using FluentValidation;
using FluentValidation.Results;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.CoreServices.Bos.Common;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;

namespace Hymson.MES.CoreServices.Services.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ManuCommonService
    {
        /// <summary>
        /// 获取条码信息
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="localizationService">国际化</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">状态(在 无效 删除 报废 锁定) 产品序列码状态为【xxxx】，不允许操作  包装验证  产品序列码已经被包装，不允许操作 </exception>
        public async Task<IEnumerable<ManuSfcBo>> GetManuSfcInfos(MultiSFCBo param, ILocalizationService localizationService)
        {
            var manuSfcEntitiesTask = _manuSfcRepository.GetManuSfcEntitiesAsync(new EntityBySFCsQuery { SiteId = param.SiteId, SFCs = param.SFCs });
            var manuSfcProduceEntitiesTask = _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery { SiteId = param.SiteId, Sfcs = param.SFCs });
            var sfcPackListTask = _manuContainerPackRepository.GetByLadeBarCodesAsync(new ManuContainerPackQuery { LadeBarCodes = param.SFCs, SiteId = param.SiteId });

            var manuSfcEntities = await manuSfcEntitiesTask;
            var manuSfcProduceEntities = await manuSfcProduceEntitiesTask;
            var sfcPackList = await sfcPackListTask;

            var manuSfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsWithIsUseAsync(manuSfcEntities.Select(x => x.Id));
            var planWorkOrderEntities = await _masterDataService.GetWorkOrderEntitiesByIdsAsync(manuSfcInfoEntities.Select(x => x.WorkOrderId??0));

            List<ManuSfcBo> list = new List<ManuSfcBo>();
            var validationFailures = new List<ValidationFailure>();
            foreach (var item in param.SFCs)
            {
                var manuSfcEntity = manuSfcEntities.FirstOrDefault(x => x.SFC == item);

                if (manuSfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex",item}};
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", item);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16375);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                if (ManuSfcStatus.ForbidSfcStatuss.Contains(manuSfcEntity.Status))
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", manuSfcEntity.SFC}};
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", manuSfcEntity.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16373);
                    validationFailure.FormattedMessagePlaceholderValues.Add("Status", localizationService.GetResource($"Hymson.MES.Core.Enums.manu.SfcStatusEnum.{SfcStatusEnum.GetName(manuSfcEntity.Status)}"));
                    validationFailures.Add(validationFailure);
                    continue;
                }

                if (sfcPackList.Any(x => x.LadeBarCode == manuSfcEntity.SFC))
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", manuSfcEntity.SFC}};
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", manuSfcEntity.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16374);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                var manuSfcInfoEntity = manuSfcInfoEntities.FirstOrDefault(x => x.SfcId == manuSfcEntity.Id);
                var planWorkOrderEntitity = planWorkOrderEntities.FirstOrDefault(x => x.Id == manuSfcInfoEntity?.WorkOrderId);
                if (planWorkOrderEntitity != null && planWorkOrderEntitity.Status == PlanWorkOrderStatusEnum.Pending)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", manuSfcEntity.SFC}};
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", manuSfcEntity.SFC);
                    }
                    validationFailure.FormattedMessagePlaceholderValues.Add("ordercode", planWorkOrderEntitity.OrderCode);
                    validationFailure.ErrorCode = nameof(ErrorCode.MES16302);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                var manuSfcProduceEntity = manuSfcProduceEntities.FirstOrDefault(x => x.SFC == item);
                var manuSfcBo = new ManuSfcBo
                {
                    Id = manuSfcEntity.Id,
                    SiteId = manuSfcEntity.SiteId,
                    SFC = manuSfcEntity.SFC,
                    Qty = manuSfcEntity.Qty,
                    Status = manuSfcEntity.Status,
                    WorkOrderId = manuSfcInfoEntity?.WorkOrderId ?? 0,
                    ProductId = manuSfcProduceEntity?.ProductId,
                    ProductBOMId = manuSfcProduceEntity?.ProductBOMId,
                };

                if (manuSfcProduceEntity == null)
                {
                    manuSfcBo.ProcessRouteId = planWorkOrderEntitity?.ProcessRouteId ?? 0;
                }
                else
                {
                    manuSfcBo.ProcedureId = manuSfcProduceEntity.ProcedureId;
                    manuSfcBo.ResourceId = manuSfcProduceEntity.ResourceId;
                    manuSfcBo.ProcessRouteId = manuSfcProduceEntity.ProcessRouteId;
                }
                list.Add(manuSfcBo);
            }

            if (validationFailures != null && validationFailures.Any())
            {
                throw new ValidationException(localizationService.GetResource("SFCError"), validationFailures);
            }
            return list;
        }
    }
}
