/*
 *creator: Karl
 *
 *describe: 产品不良录入    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-03-27 03:49:17
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Policy;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 产品不良录入 服务
    /// </summary>
    public class ManuProductBadRecordService : IManuProductBadRecordService
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
        /// 条码生产信息（物理删除） 仓储
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        /// <summary>
        /// 条码步骤表仓储 仓储
        /// </summary>
        private readonly IManuSfcStepRepository _manuSfcStepRepository;

        /// <summary>
        /// 产品不良录入 仓储
        /// </summary>
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;

        /// <summary>
        /// 不合格代码仓储
        /// </summary>
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        private readonly AbstractValidator<ManuProductBadRecordCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuProductBadRecordModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuProductBadRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
        IManuSfcProduceRepository manuSfcProduceRepository,
        IManuSfcInfoRepository manuSfcInfoRepository,
        IManuSfcStepRepository manuSfcStepRepository,
        IManuProductBadRecordRepository manuProductBadRecordRepository,
        IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
        AbstractValidator<ManuProductBadRecordCreateDto> validationCreateRules,
        AbstractValidator<ManuProductBadRecordModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 产品不良录入
        /// </summary>
        /// <param name="createDto"></param>
        /// <returns></returns>
        public async Task CreateManuProductBadRecordAsync(ManuProductBadRecordCreateDto createDto)
        {
            if (createDto == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }

            //验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(manuProductBadRecordCreateDto);

            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = createDto.Sfcs };
            //获取条码列表
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);

            //获取不合格代码列表
            var qualUnqualifiedCodes = await _qualUnqualifiedCodeRepository.GetByIdsAsync(createDto.UnqualifiedIds);

            var manuProductBadRecords = new List<ManuProductBadRecordEntity>();
            long badResourceId = 0;
            if (!string.IsNullOrWhiteSpace(createDto.FoundBadResourceId))
            {
                badResourceId = createDto.FoundBadResourceId.ParseToLong();
            }
            foreach (var item in manuSfcs)
            {
                foreach (var unqualified in qualUnqualifiedCodes)
                {
                    manuProductBadRecords.Add(new ManuProductBadRecordEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        FoundBadOperationId = createDto.FoundBadOperationId,
                        FoundBadResourceId = badResourceId,
                        OutflowOperationId = createDto.OutflowOperationId,
                        UnqualifiedId = unqualified.Id,
                        SFC = item.SFC,
                        Qty = item.Qty,
                        Status = ProductBadRecordStatusEnum.Open,
                        Source = ProductBadRecordSourceEnum.BadManualEntry,
                        Remark = createDto.Remark ?? "",
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    });
                }
            }

            //TODO
            // 1）如添加不合格代码包含缺陷类型，则将条码置于不合格代码对应不合格工艺路线首工序排队，原工序的状态清除；同时如有多条不合格工艺路线需手动选择；
            //2）如添加不合格代码均为标记类型，则不改变当前条码的状态；
            //3）如添加不合格代码为“SCRAP”，需将条码状态更新为“报废

            var codes = qualUnqualifiedCodes.Where(x => x.Type == QualUnqualifiedCodeTypeEnum.Defect);
            if (codes.Any())
            {

            }

            //报废不合格代码
            var scrapCode = qualUnqualifiedCodes.FirstOrDefault(a => a.UnqualifiedCode == "SCRAP");
            if (scrapCode != null)
            {
                var rows = 0;
                var sfcs = manuSfcs.Select(a => a.SFC).ToArray();
                var updateCommand = new ManuSfcInfoUpdateCommand
                {
                    Sfcs = sfcs,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    Status = SfcStatusEnum.Scrapping
                };
                var isScrapCommand = new UpdateIsScrapCommand
                {
                    Sfcs = sfcs,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    IsScrap = TrueOrFalseEnum.Yes
                };
                using (var trans = TransactionHelper.GetTransactionScope())
                {
                    //走报废流程
                    //修改在制品状态
                    rows += await _manuSfcProduceRepository.UpdateIsScrapAsync(isScrapCommand);
                    //修改条码状态
                    rows += await _manuSfcInfoRepository.UpdateStatusAsync(updateCommand);

                    if (manuProductBadRecords.Any())
                    {
                        //入库
                        rows += await _manuProductBadRecordRepository.InsertRangeAsync(manuProductBadRecords);
                    }
                    trans.Complete();
                }
            }
            else
            {
                if (manuProductBadRecords.Any())
                {
                    //入库
                    await _manuProductBadRecordRepository.InsertRangeAsync(manuProductBadRecords);
                }
            }
        }

        /// <summary>
        /// 查询条码的不合格代码信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ManuProductBadRecordViewDto>> GetBadRecordsBySfcAsync(ManuProductBadRecordQueryDto queryDto)
        {
            var query = new ManuProductBadRecordQuery
            {
                SFC = queryDto.SFC,
                Status = queryDto.Status,
                Type = queryDto.Type,
                SiteId = _currentSite.SiteId ?? 0
            };
            var manuProductBads = await _manuProductBadRecordRepository.GetBadRecordsBySfcAsync(query);

            //实体到DTO转换 装载数据
            var manuProductBadRecordDtos = new List<ManuProductBadRecordViewDto>();
            foreach (var manuProductBad in manuProductBads)
            {
                manuProductBadRecordDtos.Add(new ManuProductBadRecordViewDto
                {
                    UnqualifiedId = manuProductBad.UnqualifiedId,
                    UnqualifiedCode = manuProductBad.UnqualifiedCode,
                    UnqualifiedCodeName = manuProductBad.UnqualifiedCodeName,
                    ResCode = manuProductBad.ResCode,
                    ResName = manuProductBad.ResName,
                    ProcessRouteId = manuProductBad.ProcessRouteId,
                    Remark = ""
                });
            }

            //根据条码和不合格代码和资源去重显示
            manuProductBadRecordDtos= manuProductBadRecordDtos.DistinctBy(x=>x.UnqualifiedId).ToList();
            return manuProductBadRecordDtos;
        }

        /// <summary>
        /// 不良复判
        /// </summary>
        /// <param name="badReJudgmentDto"></param>
        /// <returns></returns>
        public async Task BadReJudgmentAsync(BadReJudgmentDto badReJudgmentDto)
        {
            //判断是否关闭所有不合格代码
            //判断当前工序是否末工序
        }

        /// <summary>
        /// 取消标识
        /// </summary>
        /// <param name="cancelDto"></param>
        /// <returns></returns>
        public async Task CancelSfcIdentificationAsync(CancelSfcIdentificationDto cancelDto)
        {
            if (cancelDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            if (!cancelDto.UnqualifiedLists.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }
            #region  组装数据

            var sfcs = cancelDto.UnqualifiedLists.Select(x => x.Sfc).Distinct().ToArray();
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = sfcs };
            //获取条码列表
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);

            var sfcStepList = new List<ManuSfcStepEntity>();
            if (manuSfcs.Any())
            {
                sfcStepList.Add(GetSfcStepList(manuSfcs.ToList()[0], cancelDto.Remark ?? ""));
            }

            var updateCommandList = new List<ManuProductBadRecordCommand>();
            foreach (var unqualified in cancelDto.UnqualifiedLists)
            {
                updateCommandList.Add(new ManuProductBadRecordCommand
                {
                    SiteId=_currentSite.SiteId??0,
                    Sfc = unqualified.Sfc,
                    UnqualifiedId = unqualified.UnqualifiedId,
                    Remark = unqualified.Remark ?? "",
                    Status = ProductBadRecordStatusEnum.Close,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                });
            }
            #endregion

            //入库
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //1.记录数据
                rows += await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

                //2.修改状态为关闭
                rows += await _manuProductBadRecordRepository.UpdateStatusAsync(updateCommandList);

                trans.Complete();
            }
        }

        /// <summary>
        /// 获取条码步骤数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private ManuSfcStepEntity GetSfcStepList(ManuSfcProduceEntity sfc, string remark)
        {
            var sfcStepList = new List<ManuSfcStepEntity>();
            return new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SFC = sfc.SFC,
                ProductId = sfc.ProductId,
                WorkOrdeId = sfc.WorkOrderId,
                WorkCenterId = sfc.WorkCenterId,
                ProductBOMId = sfc.BOMId,
                Qty = sfc.Qty,
                EquipmentId = sfc.EquipmentId,
                ResourceId = sfc.ResourceId,
                ProcedureId = sfc.ProcedureId,
                Type = ManuSfcStepTypeEnum.Discard,
                Status = sfc.Status,
                Lock = sfc.Lock,
                Remark = remark,
                SiteId = _currentSite.SiteId ?? 0,
                CreatedBy = sfc.CreatedBy,
                UpdatedBy = sfc.UpdatedBy
            };
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuProductBadRecordAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }
            var command = new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsArr
            };
            return await _manuProductBadRecordRepository.DeleteRangeAsync(command);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuProductBadRecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuProductBadRecordDto>> GetPageListAsync(ManuProductBadRecordPagedQueryDto manuProductBadRecordPagedQueryDto)
        {
            var manuProductBadRecordPagedQuery = manuProductBadRecordPagedQueryDto.ToQuery<ManuProductBadRecordPagedQuery>();
            manuProductBadRecordPagedQuery.SiteId = _currentSite.SiteId;
            var pagedInfo = await _manuProductBadRecordRepository.GetPagedInfoAsync(manuProductBadRecordPagedQuery);

            //实体到DTO转换 装载数据
            List<ManuProductBadRecordDto> manuProductBadRecordDtos = PrepareManuProductBadRecordDtos(pagedInfo);
            return new PagedInfo<ManuProductBadRecordDto>(manuProductBadRecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuProductBadRecordDto> PrepareManuProductBadRecordDtos(PagedInfo<ManuProductBadRecordEntity> pagedInfo)
        {
            var manuProductBadRecordDtos = new List<ManuProductBadRecordDto>();
            foreach (var manuProductBadRecordEntity in pagedInfo.Data)
            {
                var manuProductBadRecordDto = manuProductBadRecordEntity.ToModel<ManuProductBadRecordDto>();
                manuProductBadRecordDtos.Add(manuProductBadRecordDto);
            }

            return manuProductBadRecordDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuProductBadRecordModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyManuProductBadRecordAsync(ManuProductBadRecordModifyDto manuProductBadRecordModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuProductBadRecordModifyDto);

            //DTO转换实体
            var manuProductBadRecordEntity = manuProductBadRecordModifyDto.ToEntity<ManuProductBadRecordEntity>();
            manuProductBadRecordEntity.UpdatedBy = _currentUser.UserName;

            await _manuProductBadRecordRepository.UpdateAsync(manuProductBadRecordEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuProductBadRecordDto> QueryManuProductBadRecordByIdAsync(long id)
        {
            var manuProductBadRecordEntity = await _manuProductBadRecordRepository.GetByIdAsync(id);
            if (manuProductBadRecordEntity != null)
            {
                return manuProductBadRecordEntity.ToModel<ManuProductBadRecordDto>();
            }
            return null;
        }
    }
}
