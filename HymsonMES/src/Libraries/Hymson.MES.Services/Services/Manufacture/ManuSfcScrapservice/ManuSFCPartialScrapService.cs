using FluentValidation.Results;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuSFCScrap;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Manufacture.ManuSfcScrapservice
{
    /// <summary>
    /// 部分报废实现
    /// </summary>
    public class ManuSFCPartialScrapService : IManuSFCPartialScrapService
    {
        /// <summary>
        /// 当前对象（登录用户）
        /// </summary>
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 当前对象（站点）
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 条码信息表  仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuSfcRepository"></param>
        public ManuSFCPartialScrapService(ICurrentUser currentUser, ICurrentSite currentSite, IManuSfcRepository manuSfcRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcRepository = manuSfcRepository;
        }

        /// <summary>
        /// 报废
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task PartialScrapAsync(ManuSFCPartialScrapDto param)
        {
            //条码表
            var sfcEntities = await _manuSfcRepository.GetListAsync(new ManuSfcQuery
            {
                SFCs = param.BarcodeScrapList.Select(x => x.SFC),
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            //条码信息表
            var sfcInfoEntities = await _manuSfcInfoRepository.GetBySFCIdsAsync(sfcEntities.Select(x => x.Id));
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = param.BarcodeScrapList.Select(x => x.SFC), SiteId = _currentSite.SiteId ?? 00 };
            //在制品信息
            var manuSfcProduces = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);
            var validationFailures = new List<ValidationFailure>();
            foreach (var barcodeItem in param.BarcodeScrapList)
            {
                var sfcEntity = sfcEntities.FirstOrDefault(x => x.SFC == barcodeItem.SFC);

                if (sfcEntity == null)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", barcodeItem.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", barcodeItem.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15415);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (sfcEntity.Qty < barcodeItem.ScrapQty)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", barcodeItem.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", barcodeItem.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15448);
                    validationFailure.FormattedMessagePlaceholderValues.Add("Qty", sfcEntity.Qty);
                    validationFailure.FormattedMessagePlaceholderValues.Add("ScrapQty", barcodeItem.ScrapQty);
                    validationFailures.Add(validationFailure);
                    continue;
                }
                if (sfcEntity.Status == SfcStatusEnum.Scrapping)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", barcodeItem.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", barcodeItem.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15401);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                if (sfcEntity.Status == SfcStatusEnum.Locked)
                {
                    var validationFailure = new ValidationFailure();
                    if (validationFailure.FormattedMessagePlaceholderValues == null || !validationFailure.FormattedMessagePlaceholderValues.Any())
                    {
                        validationFailure.FormattedMessagePlaceholderValues = new Dictionary<string, object> {
                            { "CollectionIndex", barcodeItem.SFC}
                        };
                    }
                    else
                    {
                        validationFailure.FormattedMessagePlaceholderValues.Add("CollectionIndex", barcodeItem.SFC);
                    }
                    validationFailure.ErrorCode = nameof(ErrorCode.MES15416);
                    validationFailures.Add(validationFailure);
                    continue;
                }

                if (sfcEntity.Status == SfcStatusEnum.Complete)
                {
                    //TODO 完成品逻辑不清楚
                }

                var sfcInfoEntity = sfcInfoEntities.FirstOrDefault(x => x.SfcId == sfcEntity.Id);

                if (sfcInfoEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15401));
                }
                var manuSfcProduceInfoEntity = manuSfcProduces.FirstOrDefault(x => x.SFC == barcodeItem.SFC);

                if (manuSfcProduceInfoEntity != null)
                {
                    //updateManuSfcProduceStatusByIdCommands.Add(new UpdateManuSfcProduceStatusByIdCommand
                    //{
                    //    Id = manuSfcProduceInfoEntity.Id,
                    //    Status = SfcStatusEnum.Scrapping,
                    //    CurrentStatus = manuSfcProduceInfoEntity.Status,
                    //    UpdatedOn = HymsonClock.Now(),
                    //    UpdatedBy = _currentUser.UserName
                    //});
                }
            }
            throw new NotImplementedException("未实现");
        }
    }
}
