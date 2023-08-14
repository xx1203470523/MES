/*
 *creator: Karl
 *
 *describe: 降级录入    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-10 10:15:26
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Query;
using Hymson.MES.Services.Dtos.Integrated;
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

        public ManuDowngradingService(ICurrentUser currentUser, ICurrentSite currentSite, IManuDowngradingRepository manuDowngradingRepository, IManuDowngradingRecordRepository manuDowngradingRecordRepository, IManuSfcRepository manuSfcRepository, IManuSfcProduceRepository manuSfcProduceRepository, IManuDowngradingRuleRepository manuDowngradingRuleRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuDowngradingRepository = manuDowngradingRepository;
            _manuDowngradingRecordRepository = manuDowngradingRecordRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuDowngradingRuleRepository = manuDowngradingRuleRepository;
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
                throw new CustomerValidationException(nameof(ErrorCode.MES21201));
            }

            if (!manuDowngradingSaveDto.Sfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES21202));
            }

            #region 验证对应的降级编码是否存在
            var manuDowngradingRule= await _manuDowngradingRuleRepository.GetByCodeAsync(new ManuDowngradingRuleCodeQuery 
            { 
                Code=manuDowngradingSaveDto.Grade,
                SiteId=_currentSite.SiteId??0 
            });
            if (manuDowngradingRule == null) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES21206)).WithData("code", manuDowngradingSaveDto.Grade);
            }
            #endregion

            #region 验证对应的sfc 是否符合要求：如是否存在,是否锁定或者报废 
            var sfcList = await _manuSfcRepository.GetManuSfcInfoEntitiesAsync(new ManuSfcStatusQuery { Sfcs = manuDowngradingSaveDto.Sfcs });

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
                throw new CustomerValidationException(nameof(ErrorCode.MES21203)).WithData("sfcs", string.Join(",", noFindSfcs));
            }

            //查询已经废弃的
            var scrappingSfcs=new List<string>();
            foreach (var item in sfcList)
            {
                if (item.Status == SfcStatusEnum.Scrapping) 
                {
                    scrappingSfcs.Add(item.SFC);
                }
            }

            if (scrappingSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES21204)).WithData("sfcs", string.Join(",", scrappingSfcs));
            }

            //查询sfc对应的过程
            var sfcProduces = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = sfcList.Select(x => x.SFC).ToArray(),
            });
            //找到锁定状态的
            var lockedSfcs = new List<string>();
            foreach (var item in sfcProduces)
            {
                if (item.Status == SfcProduceStatusEnum.Locked) 
                {
                    lockedSfcs.Add(item.SFC);
                }
            }
            if (lockedSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES21205)).WithData("sfcs", string.Join(",", lockedSfcs));
            }
            #endregion

            ////DTO转换实体
            //var manuDowngradingEntity = manuDowngradingSaveDto.ToEntity<ManuDowngradingEntity>();
            var downgradings = await _manuDowngradingRepository.GetBySfcsAsync(new ManuDowngradingBySfcsQuery 
            {
                SiteId=_currentSite.SiteId??0,
                Sfcs= manuDowngradingSaveDto.Sfcs
            });

            List< ManuDowngradingEntity > addEntities = new List< ManuDowngradingEntity >();
            List< ManuDowngradingEntity > updateEntities = new List< ManuDowngradingEntity >();

            List<ManuDowngradingRecordEntity> addRecordEntitys=new List<ManuDowngradingRecordEntity> ();

            foreach (var item in manuDowngradingSaveDto.Sfcs)
            {
                //记录
                var rocordEntity = new ManuDowngradingRecordEntity() 
                {
                    SFC=item,
                    Grade= manuDowngradingSaveDto.Grade,
                    IsCancellation= ManuDowngradingRecordTypeEnum.Entry,

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
                    currentDowngrading.Remark = manuDowngradingSaveDto.Remark;

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
                        Remark = manuDowngradingSaveDto.Remark,

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
                if(addEntities.Any())
                    await _manuDowngradingRepository.InsertsAsync(addEntities);
                if(updateEntities.Any())
                    await _manuDowngradingRepository.UpdatesAsync(updateEntities);

                //保存记录 
                if(addRecordEntitys.Any())
                    await _manuDowngradingRecordRepository.InsertsAsync(addRecordEntitys);

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
            var entitys= await _manuDowngradingRepository.GetBySfcsAsync(new ManuDowngradingBySfcsQuery()
            {
                SiteId=_currentSite.SiteId??0,
                Sfcs=sfcs
            });

            var dtos = entitys.Select(s => s.ToModel<ManuDowngradingDto>());
            return dtos;
        }

    }
}
