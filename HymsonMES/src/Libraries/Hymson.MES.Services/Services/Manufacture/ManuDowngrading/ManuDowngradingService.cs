using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 降级录入 服务
    /// </summary>
    public class ManuDowngradingService : IManuDowngradingService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 降级录入 仓储
        /// </summary>
        private readonly IManuDowngradingRepository _manuDowngradingRepository;
        private readonly IManuDowngradingRecordRepository _manuDowngradingRecordRepository;

        private readonly IManuDowngradingRuleRepository _manuDowngradingRuleRepository;

        /// <summary>
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

        /// <summary>
        /// 仓储接口（条码步骤）
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="manuDowngradingRepository"></param>
        /// <param name="manuDowngradingRecordRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuDowngradingRuleRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        public ManuDowngradingService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            IManuDowngradingRepository manuDowngradingRepository,
            IManuDowngradingRecordRepository manuDowngradingRecordRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuDowngradingRuleRepository manuDowngradingRuleRepository,
            IManuSfcStepRepository manuSfcStepRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuDowngradingRepository = manuDowngradingRepository;
            _manuDowngradingRecordRepository = manuDowngradingRecordRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuDowngradingRuleRepository = manuDowngradingRuleRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="manuDowngradingSaveDto"></param>
        /// <returns></returns>
        public async Task SaveManuDowngradingAsync(ManuDowngradingSaveDto manuDowngradingSaveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            if (string.IsNullOrEmpty(manuDowngradingSaveDto.Grade))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11401));
            }

            if (!manuDowngradingSaveDto.Sfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11402));
            }

            #region 验证录入对应的降级编码是否存在
            var manuDowngradingRule = await _manuDowngradingRuleRepository.GetByCodeAsync(new ManuDowngradingRuleCodeQuery
            {
                Code = manuDowngradingSaveDto.Grade,
                SiteId = _currentSite.SiteId ?? 0
            });
            if (manuDowngradingRule == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11406)).WithData("code", manuDowngradingSaveDto.Grade);
            }
            #endregion

            #region 验证对应的sfc 是否符合要求：如是否存在,是否锁定或者报废 
            var sfcList = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = manuDowngradingSaveDto.Sfcs
            });

            var noFindSfcs = new List<string>();
            foreach (var item in manuDowngradingSaveDto.Sfcs)
            {
                if (!sfcList.Any(y => y.SFC == item))
                {
                    noFindSfcs.Add(item);
                }
            }

            if (noFindSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11403)).WithData("sfc", string.Join(",", noFindSfcs));
            }

            //查询已经废弃的
            var scrappingSfcs = new List<string>();
            foreach (var item in sfcList)
            {
                if (item.Status == SfcStatusEnum.Scrapping)
                {
                    scrappingSfcs.Add(item.SFC);
                }
            }

            if (scrappingSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11404)).WithData("sfc", string.Join(",", scrappingSfcs));
            }

            //查询sfc对应的过程
            var sfcProduces = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = sfcList.Select(x => x.SFC).ToArray(),
            });
            #endregion

            ////DTO转换实体
            var downgradings = await _manuDowngradingRepository.GetBySfcsAsync(new ManuDowngradingBySfcsQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = manuDowngradingSaveDto.Sfcs
            });

            #region 校验 当前录入的等级不能高于之前等级 （按照等级编码的顺序，排序靠前等级越高）:表示 录入的等级的排序需要大于之前的等级的排序号
            //获取全部降级规则
            var allRuleList = await _manuDowngradingRuleRepository.GetManuDowngradingRuleEntitiesAsync(new ManuDowngradingRuleQuery()
            {
                SiteId = _currentSite.SiteId ?? 0
            });

            allRuleList = allRuleList.OrderBy(x => x.SerialNumber);

            //查询sfc的降级等级是否大于当前需要修改的等级 （按照等级编码的顺序，排序靠前等级越高）
            foreach (var item in downgradings)
            {
                var oldRule = allRuleList.FirstOrDefault(x => x.Code == item.Grade);
                if (oldRule != null && manuDowngradingRule.SerialNumber < oldRule.SerialNumber)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES11409)).WithData("sfc", item.SFC);
                }
            }


            #endregion


            List<ManuDowngradingEntity> addEntities = new List<ManuDowngradingEntity>();
            List<ManuDowngradingEntity> updateEntities = new List<ManuDowngradingEntity>();

            List<ManuDowngradingRecordEntity> addRecordEntitys = new List<ManuDowngradingRecordEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new();

            foreach (var item in manuDowngradingSaveDto.Sfcs)
            {
                //条码步骤记录
                var sfcInfo = sfcList.FirstOrDefault(x => x.SFC == item);
                var sfcProduce = sfcProduces.FirstOrDefault(x => x.SFC == item);
                var stepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item,
                    ProductId = sfcInfo?.ProductId ?? 0,
                    ResourceId = sfcProduce?.ResourceId,
                    WorkOrderId = sfcInfo?.WorkOrderId ?? 0,
                    ProductBOMId = sfcProduce?.ProductBOMId,
                    ProcessRouteId = sfcProduce?.ProcessRouteId,
                    WorkCenterId = sfcProduce?.WorkCenterId ?? 0,
                    Qty = sfcProduce?.Qty ?? 0,
                    ProcedureId = sfcProduce?.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.EnterDowngrading,
                    CurrentStatus = sfcProduce?.Status ?? 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                };
                manuSfcStepList.Add(stepEntity);

                //记录
                var rocordEntity = new ManuDowngradingRecordEntity()
                {
                    SFC = item,
                    Grade = manuDowngradingSaveDto.Grade,
                    IsCancellation = ManuDowngradingRecordTypeEnum.Entry,
                    Remark = manuDowngradingSaveDto.Remark,
                    SfcInfoId = sfcInfo?.SFCInfoId ?? 0,
                    // SFCStepId= stepEntity.Id,
                    ProcedureId = sfcProduce?.ProcedureId ?? 0,
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };
                addRecordEntitys.Add(rocordEntity);

                //新增/更改 sfc对应的降级等级
                var currentDowngrading = downgradings.FirstOrDefault(x => x.SFC == item);
                if (currentDowngrading != null)
                {
                    currentDowngrading.Grade = manuDowngradingSaveDto.Grade;
                    currentDowngrading.Remark = manuDowngradingSaveDto.Remark ?? "";
                    currentDowngrading.UpdatedOn = HymsonClock.Now();
                    currentDowngrading.UpdatedBy = _currentUser.UserName;
                    updateEntities.Add(currentDowngrading);
                }
                else
                {
                    addEntities.Add(new ManuDowngradingEntity
                    {
                        SFC = item,
                        Grade = manuDowngradingSaveDto.Grade,
                        Remark = manuDowngradingSaveDto.Remark ?? "",
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                    });
                }
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                if (addEntities.Any())
                    await _manuDowngradingRepository.InsertsAsync(addEntities);
                if (updateEntities.Any())
                    await _manuDowngradingRepository.UpdatesAsync(updateEntities);

                //保存记录 
                if (addRecordEntitys.Any())
                {
                    await _manuDowngradingRecordRepository.InsertsAsync(addRecordEntitys);
                }

                if (manuSfcStepList.Any())
                {
                    await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);
                }

                ts.Complete();
            }

        }

        /// <summary>
        /// 根据SFC获取数据
        /// </summary>
        /// <param name="sfcs"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuDowngradingDto>> GetManuDowngradingBySfcsAsync(string[] sfcs)
        {
            var entitys = await _manuDowngradingRepository.GetBySfcsAsync(new ManuDowngradingBySfcsQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = sfcs
            });

            var dtos = entitys.Select(s => s.ToModel<ManuDowngradingDto>());
            return dtos;
        }


        /// <summary>
        /// 保存-降级移除
        /// </summary>
        /// <param name="manuDowngradingSaveRemoveDto"></param>
        /// <returns></returns>
        public async Task SaveManuDowngradingRemoveAsync(ManuDowngradingSaveRemoveDto manuDowngradingSaveRemoveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            if (!manuDowngradingSaveRemoveDto.Sfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11402));
            }

            #region 验证对应的sfc 是否符合要求：如是否存在,是否锁定或者报废 
            var sfcList = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { SiteId = _currentSite.SiteId ?? 0, Sfcs = manuDowngradingSaveRemoveDto.Sfcs });

            var noFindSfcs = new List<string>();
            foreach (var item in manuDowngradingSaveRemoveDto.Sfcs)
            {
                if (!sfcList.Any(y => y.SFC == item))
                {
                    noFindSfcs.Add(item);
                }
            }

            if (noFindSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11403)).WithData("sfc", string.Join(",", noFindSfcs));
            }

            //查询已经废弃的
            var scrappingSfcs = new List<string>();
            foreach (var item in sfcList)
            {
                if (item.Status == SfcStatusEnum.Scrapping)
                {
                    scrappingSfcs.Add(item.SFC);
                }
            }

            if (scrappingSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11404)).WithData("sfc", string.Join(",", scrappingSfcs));
            }

            //查询sfc对应的过程
            var sfcProduces = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = sfcList.Select(x => x.SFC).ToArray(),
            });
            ////找到锁定状态的
            //var lockedSfcs = new List<string>();
            //foreach (var item in sfcProduces)
            //{
            //    if (item.Status == SfcProduceStatusEnum.Locked)
            //    {
            //        lockedSfcs.Add(item.SFC);
            //    }
            //}
            //if (lockedSfcs.Any())
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES11405)).WithData("sfc", string.Join(",", lockedSfcs));
            //}
            #endregion

            ////DTO转换实体
            //var manuDowngradingEntity = manuDowngradingSaveDto.ToEntity<ManuDowngradingEntity>();
            var downgradings = await _manuDowngradingRepository.GetBySfcsAsync(new ManuDowngradingBySfcsQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = manuDowngradingSaveRemoveDto.Sfcs
            });

            //校验当前降级是否有记录
            var noDowngradingSfcs = manuDowngradingSaveRemoveDto.Sfcs.Where(x => downgradings.FirstOrDefault(y => y.SFC == x) == null);
            if (noDowngradingSfcs.Any()) throw new CustomerValidationException(nameof(ErrorCode.MES11407)).WithData("sfc", string.Join(",", noDowngradingSfcs));

            List<long> delIds = new List<long>();

            List<ManuDowngradingRecordEntity> addRecordEntitys = new List<ManuDowngradingRecordEntity>();
            List<ManuSfcStepEntity> manuSfcStepList = new();

            foreach (var item in manuDowngradingSaveRemoveDto.Sfcs)
            {
                //条码步骤记录
                var sfcInfo = sfcList.FirstOrDefault(x => x.SFC == item);
                var sfcProduce = sfcProduces.FirstOrDefault(x => x.SFC == item);

                var stepEntity = new ManuSfcStepEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    SFC = item,
                    ProductId = sfcInfo?.ProductId ?? 0,
                    WorkOrderId = sfcInfo?.WorkOrderId ?? 0,
                    ProductBOMId = sfcProduce?.ProductBOMId,
                    ProcessRouteId = sfcProduce?.ProcessRouteId,
                    WorkCenterId = sfcProduce?.WorkCenterId ?? 0,
                    Qty = sfcProduce?.Qty ?? 0,
                    ProcedureId = sfcProduce?.ProcedureId,
                    Operatetype = ManuSfcStepTypeEnum.RemoveDowngrading,
                    CurrentStatus = sfcProduce?.Status ?? 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName
                };
                manuSfcStepList.Add(stepEntity);

                //记录
                var rocordEntity = new ManuDowngradingRecordEntity()
                {
                    SFC = item,
                    Grade = "",
                    IsCancellation = ManuDowngradingRecordTypeEnum.Remove,
                    Remark = manuDowngradingSaveRemoveDto.Remark,
                    SFCStepId= stepEntity.Id,
                    SfcInfoId = sfcInfo?.SFCInfoId ?? 0,
                    ProcedureId = sfcProduce?.ProcedureId ?? 0,
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                };
                addRecordEntitys.Add(rocordEntity);

                //删除
                var currentDowngrading = downgradings.FirstOrDefault(x => x.SFC == item);
                if (currentDowngrading != null)
                {
                    delIds.Add(currentDowngrading.Id);
                }
            }

            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                var result = 0;
                if (delIds.Any())
                {
                    result = await _manuDowngradingRepository.DeletesTrueByIdsAsync(delIds.ToArray());

                    if (result > 0 && result == delIds.Distinct().Count())
                    {
                        //保存记录 
                        if (addRecordEntitys.Any())
                        {
                            await _manuDowngradingRecordRepository.InsertsAsync(addRecordEntitys);
                        }

                        if (manuSfcStepList.Any())
                        {
                            await _manuSfcStepRepository.InsertRangeAsync(manuSfcStepList);
                        }
                    }
                    else
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES11410));
                    }
                }

                ts.Complete();
            }

        }

    }
}
