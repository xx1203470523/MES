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
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Newtonsoft.Json;

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
        /// 仓储（在制品业务）
        /// </summary>
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;

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

        /// <summary>
        /// 
        /// </summary>
        private readonly AbstractValidator<ManuProductBadRecordCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuProductBadRecordModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuProductBadRecordService(ICurrentUser currentUser, ICurrentSite currentSite,
        IManuSfcProduceRepository manuSfcProduceRepository,
        IManuSfcRepository manuSfcRepository,
        IManuSfcStepRepository manuSfcStepRepository,
        IManuProductBadRecordRepository manuProductBadRecordRepository,
        IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
        AbstractValidator<ManuProductBadRecordCreateDto> validationCreateRules,
        AbstractValidator<ManuProductBadRecordModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcRepository = manuSfcRepository;
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
            if (createDto == null) throw new ValidationException(nameof(ErrorCode.MES10100));

            // 验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(manuProductBadRecordCreateDto);

            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = createDto.Sfcs };
            // 获取条码列表
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);

            // 获取不合格代码列表
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
                var updateCommand = new ManuSfcUpdateCommand
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
                    rows += await _manuSfcRepository.UpdateStatusAsync(updateCommand);

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

            // 实体到DTO转换 装载数据
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

            // 根据条码和不合格代码和资源去重显示
            manuProductBadRecordDtos = manuProductBadRecordDtos.DistinctBy(x => x.UnqualifiedId).ToList();
            return manuProductBadRecordDtos;
        }

        /// <summary>
        /// 不良复判
        /// </summary>
        /// <param name="badReJudgmentDto"></param>
        /// <returns></returns>
        public async Task BadReJudgmentAsync(BadReJudgmentDto badReJudgmentDto)
        {
            #region
            if (badReJudgmentDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            if (string.IsNullOrWhiteSpace(badReJudgmentDto.Sfc)) throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            if (badReJudgmentDto.UnqualifiedLists.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES15405));
            #endregion

            //TODO 放到不合格工艺路线指定工序排队
            //判断是否关闭所有不合格代码
            //判断当前工序是否末工序

            // 判断是否已存在返修信息
            var produceBusiness = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCAsync(new SfcProduceBusinessQuery
            {
                Sfc = badReJudgmentDto.Sfc,
                BusinessType = ManuSfcProduceBusinessType.Repair
            });
            if (produceBusiness != null) throw new CustomerValidationException(nameof(ErrorCode.MES15406));

            #region 组装数据
            // 获取条码
            var manuSfc = await _manuSfcProduceRepository.GetBySFCAsync(badReJudgmentDto.Sfc)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES15402));

            // 条码步骤
            var sfcStepEntity = CreateSFCStepEntity(manuSfc, ManuSfcStepTypeEnum.Repair, badReJudgmentDto.Remark ?? "");

            // 在制品业务
            var manuSfcProduceBusinessEntity = new ManuSfcProduceBusinessEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SfcInfoId = manuSfc.Id,
                BusinessType = ManuSfcProduceBusinessType.Repair,
                BusinessContent = JsonConvert.SerializeObject(new
                {
                    manuSfc.ProcessRouteId,
                    manuSfc.ProcedureId,
                    manuSfc.ResourceId,
                }),
                SiteId = sfcStepEntity.SiteId,
                CreatedBy = sfcStepEntity.CreatedBy,
                UpdatedBy = sfcStepEntity.UpdatedBy
            };

            // 产品不良录入
            var badRecordEntities = new List<ManuProductBadRecordEntity>();
            if (badReJudgmentDto.UnqualifiedLists.Any() == true)
            {
                badRecordEntities = badReJudgmentDto.UnqualifiedLists.Select(s => new ManuProductBadRecordEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SFC = badReJudgmentDto.Sfc,
                    FoundBadOperationId = manuSfc.ProductId,
                    FoundBadResourceId = manuSfc.ResourceId,
                    OutflowOperationId = manuSfc.ProductId, // TODO
                    UnqualifiedId = s.UnqualifiedId,
                    Remark = s.Remark ?? "",
                    Status = ProductBadRecordStatusEnum.Open,
                    Source = ProductBadRecordSourceEnum.EquipmentReBad,
                    SiteId = sfcStepEntity.SiteId,
                    CreatedBy = sfcStepEntity.CreatedBy,
                    UpdatedBy = sfcStepEntity.UpdatedBy
                }).ToList();
            }
            #endregion

            // 入库
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                // 1.插入 manu_sfc_step，步骤为"维修"
                rows += await _manuSfcStepRepository.InsertAsync(sfcStepEntity);

                // 2.插入 manu_sfc_produce_business
                rows += await _manuSfcProduceRepository.InsertSfcProduceBusinessAsync(manuSfcProduceBusinessEntity);

                // 3.插入 manu_product_bad_record
                rows += await _manuProductBadRecordRepository.InsertRangeAsync(badRecordEntities);

                trans.Complete();
            }
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

            //获取条码列表
            var sfcs = cancelDto.UnqualifiedLists.Select(x => x.Sfc).Distinct().ToArray();
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = sfcs };
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);
            if (!manuSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15402));
            }

            var sfcStepList = new List<ManuSfcStepEntity>();
            if (manuSfcs.Any())
            {
                sfcStepList.Add(CreateSFCStepEntity(manuSfcs.ToList()[0], ManuSfcStepTypeEnum.CloseIdentification, cancelDto.Remark ?? ""));
            }

            var updateCommandList = new List<ManuProductBadRecordCommand>();
            foreach (var unqualified in cancelDto.UnqualifiedLists)
            {
                updateCommandList.Add(new ManuProductBadRecordCommand
                {
                    SiteId = _currentSite.SiteId ?? 0,
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
                rows += await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);

                trans.Complete();
            }
        }

        /// <summary>
        /// 创建条码步骤数据
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        private ManuSfcStepEntity CreateSFCStepEntity(ManuSfcProduceEntity sfc, ManuSfcStepTypeEnum type, string remark = "")
        {
            return new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SFC = sfc.SFC,
                ProductId = sfc.ProductId,
                WorkOrderId = sfc.WorkOrderId,
                WorkCenterId = sfc.WorkCenterId,
                ProductBOMId = sfc.ProductBOMId,
                Qty = sfc.Qty,
                EquipmentId = sfc.EquipmentId,
                ResourceId = sfc.ResourceId,
                ProcedureId = sfc.ProcedureId,
                Operatetype = type,
                CurrentStatus = sfc.Status,
                //Lock = sfc.Lock,
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
