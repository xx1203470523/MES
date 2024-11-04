using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Manufacture;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.CoreServices.Bos.Manufacture;
using Hymson.MES.CoreServices.Services.Common.ManuExtension;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuProductBadRecord.Query;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfc.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Command;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcProduce.Query;
using Hymson.MES.Data.Repositories.Quality.IQualityRepository;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCommonDto;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.ManuCommon;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Minio.DataModel;
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
        /// 条码表 仓储
        /// </summary>
        private readonly IManuSfcRepository _manuSfcRepository;
        /// <summary>
        /// 条码信息表 仓储
        /// </summary>
        private readonly IManuSfcInfoRepository _sfcInfoRepository;

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
        /// 生产公共服务
        /// </summary>
        private readonly IManuCommonOldService _manuCommonOldService;

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
         IManuSfcInfoRepository sfcInfoRepository,
        IManuSfcStepRepository manuSfcStepRepository,
        IManuProductBadRecordRepository manuProductBadRecordRepository,
        IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
        IManuCommonOldService manuCommonOldService,
        AbstractValidator<ManuProductBadRecordCreateDto> validationCreateRules,
        AbstractValidator<ManuProductBadRecordModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuSfcRepository = manuSfcRepository;
            _sfcInfoRepository = sfcInfoRepository;
            _manuSfcStepRepository = manuSfcStepRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _manuCommonOldService = manuCommonOldService;
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
            #region
            if (createDto == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10100));
            }

            // 验证DTO
            //await _validationCreateRules.ValidateAndThrowAsync(manuProductBadRecordCreateDto);
            if (createDto.Sfcs == null || createDto.Sfcs.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15400));
            }

            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = createDto.Sfcs, SiteId = _currentSite.SiteId ?? 123456 };
            // 获取条码列表
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceInfoEntitiesAsync(manuSfcProducePagedQuery);
            var sfcs = manuSfcs.Select(x => x.SFC).ToArray();

            //报废的不能操作
            //即时锁不能操作
            //将来锁定当前工序不能操作
            var scrapSfcs = manuSfcs.Where(x => x.IsScrap == TrueOrFalseEnum.Yes).Select(x => x.SFC).ToArray();
            //类型为报废时判断条码是否已经报废,若已经报废提示:存在已报废的条码，不可再次报废
            if (scrapSfcs.Any())
            {
                var strs = string.Join("','", scrapSfcs);
                throw new CustomerValidationException(nameof(ErrorCode.MES15411)).WithData("sfcs", strs);
            }
            await VerifySfcsLockAsync(manuSfcs.ToArray());

            //已经存在的不合格信息不允许重复录入
            var productBadRecordList = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery
            {
                Sfcs = sfcs,
                Status = ProductBadRecordStatusEnum.Open,
                SiteId = _currentSite.SiteId ?? 123456
            });
            var existUnqualifiedIds = productBadRecordList.Select(x => x.UnqualifiedId).Distinct().ToList();
            // 获取不合格代码列表
            var qualUnqualifiedCodes = await _qualUnqualifiedCodeRepository.GetByIdsAsync(createDto.UnqualifiedIds);
            var sameUnqualifiedIds = existUnqualifiedIds.Intersect(createDto.UnqualifiedIds).ToList();
            if (sameUnqualifiedIds.Any())
            {
                var codes = qualUnqualifiedCodes.Where(x => sameUnqualifiedIds.Contains(x.Id)).Select(x => x.UnqualifiedCode).ToArray();
                var existsCodes = string.Join(',', codes);
                throw new CustomerValidationException(nameof(ErrorCode.MES15409)).WithData("codes", existsCodes);
            }
            #endregion

            var manuProductBadRecords = new List<ManuProductBadRecordEntity>();
            long badResourceId = 0;
            if (!string.IsNullOrWhiteSpace(createDto.FoundBadResourceId))
            {
                badResourceId = createDto.FoundBadResourceId.ParseToLong();
            }

            // 1）如添加不合格代码包含缺陷类型，则将条码置于不合格代码对应不合格工艺路线首工序排队，原工序的状态清除；同时如有多条不合格工艺路线需手动选择；
            //2）如添加不合格代码均为标记类型，则不改变当前条码的状态；
            //3）如添加不合格代码为“SCRAP”，需将条码状态更新为“报废
            var isDefect = qualUnqualifiedCodes.Any(x => x.Type == QualUnqualifiedCodeTypeEnum.Defect && x.UnqualifiedCode.ToUpperInvariant() != ManuProductBadRecord.ScrapCode);
            var processRouteProcedure = new ProcessRouteProcedureDto();
            if (isDefect)
            {
                if (!createDto.BadProcessRouteId.HasValue || createDto.BadProcessRouteId == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15408));
                }

                //判断是否有未关闭的维修业务，有不允许添加
                var sfcRepairs = await _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { SiteId = _currentSite.SiteId ?? 123456, Sfcs = sfcs, BusinessType = ManuSfcProduceBusinessType.Repair });
                if (sfcRepairs != null && sfcRepairs.Any())
                {
                    var strs = string.Join(",", sfcRepairs.Select(x => x.Sfc));
                    throw new CustomerValidationException(nameof(ErrorCode.MES15410)).WithData("sfcs", strs);
                }
                processRouteProcedure = await _manuCommonOldService.GetFirstProcedureAsync(createDto.BadProcessRouteId ?? 0);
            }
            var sfcStepList = new List<ManuSfcStepEntity>();
            var manuSfcProduceList = new List<ManuSfcProduceBusinessEntity>();
            var scrapCode = qualUnqualifiedCodes.FirstOrDefault(a => a.UnqualifiedCode.ToUpperInvariant() == ManuProductBadRecord.ScrapCode);

            var isOnlyScrap = (scrapCode != null && qualUnqualifiedCodes.Count() == 1);
            foreach (var item in manuSfcs)
            {
                foreach (var unqualified in qualUnqualifiedCodes)
                {
                    //报废的不需要记录不良，不需要关闭和展示
                    if (unqualified.UnqualifiedCode.ToUpperInvariant() == ManuProductBadRecord.ScrapCode)
                    {
                        continue;
                    }

                    manuProductBadRecords.Add(new ManuProductBadRecordEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 123456,
                        FoundBadOperationId = createDto.FoundBadOperationId,
                        FoundBadResourceId = badResourceId,
                        OutflowOperationId = createDto.OutflowOperationId,
                        UnqualifiedId = unqualified.Id,
                        SFC = item.SFC,
                        SfcInfoId = item.SfcInfoId,
                        Qty = item.Qty,
                        Status = ProductBadRecordStatusEnum.Open,
                        Source = ProductBadRecordSourceEnum.BadManualEntry,
                        Remark = createDto.Remark ?? "",
                        DisposalResult = isDefect ? ProductBadDisposalResultEnum.AutoHandle : null,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName
                    });
                }

                var manuSfc = manuSfcs.FirstOrDefault(x => x.SFC == item.SFC);

                if (!isOnlyScrap)
                {
                    // 不良录入条码步骤
                    var sfcStepEntity = CreateSFCStepEntity(manuSfc, ManuSfcStepTypeEnum.BadEntry, createDto.Remark ?? "");
                    sfcStepList.Add(sfcStepEntity);
                }

                if (scrapCode != null)
                {
                    var scrapStep = CreateSFCStepEntity(manuSfc, ManuSfcStepTypeEnum.Discard, createDto.Remark ?? "");
                    sfcStepList.Add(scrapStep);
                }

                if (isDefect)
                {
                    // 在制品业务
                    var manuSfcProduceBusinessEntity = new ManuSfcProduceBusinessEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SfcProduceId = manuSfc.Id,
                        BusinessType = ManuSfcProduceBusinessType.Repair,
                        BusinessContent = JsonConvert.SerializeObject(new SfcProduceRepairBo
                        {
                            ProcessRouteId = manuSfc.ProcessRouteId, //createDto.BadProcessRouteId ?? 0,
                            ProcedureId = manuSfc.ProcedureId //processRouteProcedure.ProcedureId
                        }),
                        SiteId = _currentSite.SiteId ?? 123456,
                        CreatedBy = manuSfc.CreatedBy,
                        UpdatedBy = manuSfc.UpdatedBy
                    };
                    manuSfcProduceList.Add(manuSfcProduceBusinessEntity);
                }
            }

            var updateRouteCommand = new ManuSfcUpdateRouteCommand();
            if (isDefect)
            {
                updateRouteCommand = new ManuSfcUpdateRouteCommand
                {
                    ProcessRouteId = createDto.BadProcessRouteId ?? 0,
                    ProcedureId = processRouteProcedure.ProcedureId,
                    UpdatedBy = _currentUser.UserName,
                    Status = SfcProduceStatusEnum.lineUp,
                    Ids = manuSfcs.Select(x => x.Id).ToArray()
                };
            }

            //报废不合格代码
            if (scrapCode != null)
            {
                var rows = 0;
                var updateCommand = new ManuSfcUpdateCommand
                {
                    Sfcs = sfcs,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    Status = SfcStatusEnum.Scrapping
                };
                var isScrapCommand = new UpdateIsScrapCommand
                {
                    SiteId = _currentSite.SiteId ?? 123456,
                    Sfcs = sfcs,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                    IsScrap = TrueOrFalseEnum.Yes,
                    CurrentIsScrap = TrueOrFalseEnum.No
                };
                using (var trans = TransactionHelper.GetTransactionScope())
                {
                    //修改在制品状态
                    rows += await _manuSfcProduceRepository.UpdateIsScrapAsync(isScrapCommand);
                    //修改条码状态
                    rows += await _manuSfcRepository.UpdateStatusAsync(updateCommand);

                    if (manuProductBadRecords.Any())
                    {
                        //入库
                        rows += await _manuProductBadRecordRepository.InsertRangeAsync(manuProductBadRecords);
                    }

                    if (sfcStepList.Any())
                    {
                        await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
                    }
                    if (manuSfcProduceList.Any())
                    {
                        //添加维修业务
                        await _manuSfcProduceRepository.InsertSfcProduceBusinessRangeAsync(manuSfcProduceList);

                        //修改在制品工艺路线和工序信息
                        await _manuSfcProduceRepository.UpdateRouteAsync(updateRouteCommand);
                    }
                    trans.Complete();
                }
            }
            else
            {
                var rows = 0;
                using (var trans = TransactionHelper.GetTransactionScope())
                {
                    if (manuProductBadRecords.Any())
                    {
                        //入库
                        rows += await _manuProductBadRecordRepository.InsertRangeAsync(manuProductBadRecords);
                    }
                    if (sfcStepList.Any())
                    {
                        await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);
                    }
                    if (manuSfcProduceList.Any())
                    {
                        //添加维修业务
                        await _manuSfcProduceRepository.InsertSfcProduceBusinessRangeAsync(manuSfcProduceList);

                        //修改在制品工艺路线和工序信息
                        await _manuSfcProduceRepository.UpdateRouteAsync(updateRouteCommand);
                    }
                    trans.Complete();
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
                SiteId = _currentSite.SiteId ?? 123456
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

            var sfc = badReJudgmentDto.Sfc;
            var sfcs = new string[] { sfc };
            var query = new ManuProductBadRecordQuery
            {
                SFC = sfc,
                Status = ProductBadRecordStatusEnum.Open,
                Type = QualUnqualifiedCodeTypeEnum.Defect,
                SiteId = _currentSite.SiteId ?? 123456
            };
            var badRecordList = (await _manuProductBadRecordRepository.GetBadRecordsBySfcAsync(query)).ToList();
            if (!badRecordList.Any())
            {
                return;
            }
            #endregion

            #region 组装数据
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = sfcs, SiteId = _currentSite.SiteId ?? 123456 };
            // 获取条码信息
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceEntitiesAsync(manuSfcProducePagedQuery);
            if (manuSfcs == null || !manuSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15402));
            }
            var manuSfc = manuSfcs.ToList()[0];
            //var sfcInfoId = manuSfc.SfcInfoId;

            //验证是否报废
            if (manuSfc.IsScrap == TrueOrFalseEnum.Yes)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15411)).WithData("sfcs", sfc);
            }

            // IEnumerable<long> sfcInfoIds = new[] { manuSfc.Id };
            // 判断是否已存在返修信息,是否锁定
            //  var sfcProduceBusinessEntities = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCIdsAsync(sfcInfoIds);
            await VerifyLockOrRepairAsync(sfc, manuSfc.ProcedureId, manuSfc.Id);

            //判断是否关闭所有不合格信息
            var allunqualifiedIds = badRecordList.Select(x => x.UnqualifiedId.ParseToLong()).Distinct().ToArray();
            var closeunqualifiedIds = badReJudgmentDto.UnqualifiedLists.Select(x => x.UnqualifiedId).ToList();
            var diffArr = allunqualifiedIds.Where(x => !closeunqualifiedIds.Contains(x)).ToArray();

            var updateCommandList = new List<ManuProductBadRecordCommand>();
            foreach (var unqualified in badReJudgmentDto.UnqualifiedLists)
            {
                updateCommandList.Add(new ManuProductBadRecordCommand
                {
                    SiteId = _currentSite.SiteId ?? 123456,
                    Sfc = sfc,
                    UnqualifiedId = unqualified.UnqualifiedId,
                    DisposalResult = ProductBadDisposalResultEnum.ReJudgmentRepair,
                    Remark = unqualified.Remark ?? "",
                    Status = ProductBadRecordStatusEnum.Close,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now(),
                });
            }
            var sfcStepEntity = new ManuSfcStepEntity();

            //关闭了所有的不合格代码
            if (!diffArr.Any())
            {
                // 条码步骤
                sfcStepEntity = CreateSFCStepEntity(manuSfc, ManuSfcStepTypeEnum.CloseDefect, badReJudgmentDto.Remark ?? "");
                //获取工艺路线节点
                var isLast = await IsLastProcedureIdAsync(manuSfc.ProcessRouteId, manuSfc.ProcedureId);
                if (isLast)
                {
                    var manuSfcInfoUpdate = new ManuSfcUpdateCommand
                    {
                        Sfcs = sfcs,
                        UserId = _currentUser.UserName,
                        UpdatedOn = HymsonClock.Now(),
                        Status = SfcStatusEnum.Complete
                    };

                    using (var trans = TransactionHelper.GetTransactionScope())
                    {
                        //条码修改为已完成状态
                        await _manuSfcRepository.UpdateStatusAsync(manuSfcInfoUpdate);
                        //删除在制品信息
                        await _manuSfcProduceRepository.DeleteAsync(manuSfc.Id);
                        //关闭不合格信息
                        await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);
                        //记录step信息
                        await _manuSfcStepRepository.InsertAsync(sfcStepEntity);
                        trans.Complete();
                    }
                }
                else
                {
                    using (var trans = TransactionHelper.GetTransactionScope())
                    {
                        //关闭不合格信息
                        await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);
                        //记录step信息
                        await _manuSfcStepRepository.InsertAsync(sfcStepEntity);
                        trans.Complete();
                    }
                }
            }
            else
            {

                //var productBadRecordList = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySFCQuery
                //{
                //    Status = ProductBadRecordStatusEnum.Open,
                //    SFC = badReJudgmentDto.Sfc,
                //    SiteId = _currentSite.SiteId ?? 123456
                //});
                //foreach (var item in productBadRecordList)
                //{
                //    var unqualified = badReJudgmentDto.UnqualifiedLists.FirstOrDefault(x => x.UnqualifiedId == item.UnqualifiedId);
                //    if (unqualified == null)
                //    {
                //        item.DisposalResult = ProductBadDisposalResultEnum.ReJudgmentRepair;
                //    }
                //    else
                //    {
                //        item.Status = ProductBadRecordStatusEnum.Close;
                //        item.Remark = unqualified.Remark ?? "";
                //    }
                //    item.UpdatedBy = _currentUser.UserName;
                //    item.UpdatedOn = HymsonClock.Now();
                //}

                //放到不合格工艺路线指定工序排队
                //判断是否关闭所有不合格代码
                //判断当前工序是否末工序
                var manuSfcProduceBusinessEntity = new ManuSfcProduceBusinessEntity();

                if (!badReJudgmentDto.BadProcessRouteId.HasValue || badReJudgmentDto.BadProcessRouteId == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15408));
                }
                var processRouteProcedure = await _manuCommonOldService.GetFirstProcedureAsync(badReJudgmentDto.BadProcessRouteId ?? 0);
                // 条码步骤
                sfcStepEntity = CreateSFCStepEntity(manuSfc, ManuSfcStepTypeEnum.BadRejudgment, badReJudgmentDto.Remark ?? "");

                // 在制品业务
                manuSfcProduceBusinessEntity = new ManuSfcProduceBusinessEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SfcProduceId = manuSfc.Id,
                    BusinessType = ManuSfcProduceBusinessType.Repair,
                    BusinessContent = JsonConvert.SerializeObject(new SfcProduceRepairBo
                    {
                        ProcessRouteId = manuSfc.ProcessRouteId, //badReJudgmentDto.BadProcessRouteId ?? 0,
                        ProcedureId = manuSfc.ProductId,//processRouteProcedure.ProcedureId
                    }),
                    SiteId = sfcStepEntity.SiteId,
                    CreatedBy = sfcStepEntity.CreatedBy,
                    UpdatedBy = sfcStepEntity.UpdatedBy
                };

                var updateRouteCommand = new ManuSfcUpdateRouteCommand();

                updateRouteCommand = new ManuSfcUpdateRouteCommand
                {
                    ProcessRouteId = badReJudgmentDto.BadProcessRouteId ?? 0,
                    ProcedureId = processRouteProcedure.ProcedureId,
                    UpdatedBy = _currentUser.UserName,
                    Status = SfcProduceStatusEnum.lineUp,
                    Ids = new long[] { manuSfc.Id }
                };
                #endregion

                // 入库
                var rows = 0;
                using (var trans = TransactionHelper.GetTransactionScope())
                {
                    // 1.插入 manu_sfc_step，步骤为"维修"
                    rows += await _manuSfcStepRepository.InsertAsync(sfcStepEntity);

                    // 2.插入 manu_sfc_produce_business
                    //添加维修业务
                    rows += await _manuSfcProduceRepository.InsertSfcProduceBusinessAsync(manuSfcProduceBusinessEntity);

                    // 3.插入 manu_product_bad_record
                    rows += await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);

                    //4.修改在制品工艺路线和工序信息
                    rows += await _manuSfcProduceRepository.UpdateRouteAsync(updateRouteCommand);
                    trans.Complete();
                }
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

            var sfcs = cancelDto.UnqualifiedLists.Select(x => x.Sfc).Distinct().ToArray();
            var manuSfcProducePagedQuery = new ManuSfcProduceQuery { Sfcs = sfcs, SiteId = _currentSite.SiteId ?? 123456 };
            // 获取条码列表
            var manuSfcs = await _manuSfcProduceRepository.GetManuSfcProduceInfoEntitiesAsync(manuSfcProducePagedQuery);
            if (!manuSfcs.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15402));
            }
            sfcs = manuSfcs.Select(x => x.SFC).ToArray();
            var scrapSfcs = manuSfcs.Where(x => x.IsScrap == TrueOrFalseEnum.Yes).Select(x => x.SFC).ToArray();
            //类型为报废时判断条码是否已经报废,若已经报废提示:存在已报废的条码，不可再次报废
            if (scrapSfcs.Any())
            {
                var strs = string.Join("','", scrapSfcs);
                throw new CustomerValidationException(nameof(ErrorCode.MES15411)).WithData("sfcs", strs);
            }
            await VerifySfcsLockAsync(manuSfcs.ToArray());

            #region  组装数据
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
                    SiteId = _currentSite.SiteId ?? 123456,
                    Sfc = unqualified.Sfc,
                    UnqualifiedId = unqualified.UnqualifiedId,
                    Remark = unqualified.Remark ?? "",
                    Status = ProductBadRecordStatusEnum.Close,
                    UserId = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                });
            }
            #endregion

            //入库
            var rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //1.修改状态为关闭
                rows += await _manuProductBadRecordRepository.UpdateStatusRangeAsync(updateCommandList);
                if (rows < updateCommandList.Count())
                {
                    //报错
                    throw new CustomerValidationException(nameof(ErrorCode.MES15414)).WithData("sfcs", string.Join("','", sfcs));
                }

                //2.记录数据
                rows += await _manuSfcStepRepository.InsertRangeAsync(sfcStepList);

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
                SiteId = _currentSite.SiteId ?? 123456,
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

        /// <summary>
        /// 验证sfc是否锁定
        /// </summary>
        /// <param name="manuSfcs"></param>
        /// <returns></returns>
        private async Task VerifySfcsLockAsync(ManuSfcProduceInfoView[] manuSfcs)
        {
            var sfcs = manuSfcs.Select(x => x.SFC).ToArray();
            var sfcProduceBusinesss = await _manuSfcProduceRepository.GetSfcProduceBusinessListBySFCAsync(new SfcListProduceBusinessQuery { SiteId = _currentSite.SiteId ?? 123456, Sfcs = sfcs, BusinessType = ManuSfcProduceBusinessType.Lock });
            if (sfcProduceBusinesss != null && sfcProduceBusinesss.Any())
            {
                //var sfcInfoIds = sfcProduceBusinesss.Select(it => it.SfcProduceId).ToArray();
                var sfcProduceBusinesssList = sfcProduceBusinesss.ToList();
                var instantLockSfcs = new List<string>();
                foreach (var business in sfcProduceBusinesssList)
                {
                    var manuSfc = manuSfcs.FirstOrDefault(x => x.Id == business.SfcProduceId);
                    if (manuSfc == null)
                    {
                        continue;
                    }
                    var sfcProduceLockBo = System.Text.Json.JsonSerializer.Deserialize<SfcProduceLockBo>(business.BusinessContent);
                    if (sfcProduceLockBo == null)
                    {
                        continue;
                    }
                    if (sfcProduceLockBo.Lock == QualityLockEnum.InstantLock)
                    {
                        instantLockSfcs.Add(manuSfc.SFC);
                    }
                    if (sfcProduceLockBo.Lock == QualityLockEnum.FutureLock && sfcProduceLockBo.LockProductionId == manuSfc.ProcedureId)
                    {
                        instantLockSfcs.Add(manuSfc.SFC);
                    }
                }

                if (instantLockSfcs.Any())
                {
                    var strs = string.Join(",", instantLockSfcs.Distinct().ToArray());
                    throw new CustomerValidationException(nameof(ErrorCode.MES15407)).WithData("sfcs", strs);
                }
            }
        }

        /// <summary>
        /// 验证条码是否锁定和是否有返修任务
        /// </summary>
        /// <param name="sfc"></param>
        /// <param name="procedureId"></param>
        /// <param name="sfcInfoId"></param>
        /// <returns></returns>
        private async Task VerifyLockOrRepairAsync(string sfc, long procedureId, long sfcInfoId)
        {
            IEnumerable<long> sfcInfoIds = new[] { sfcInfoId };
            var sfcProduceBusinessEntities = await _manuSfcProduceRepository.GetSfcProduceBusinessBySFCIdsAsync(sfcInfoIds);
            if (sfcProduceBusinessEntities != null && sfcProduceBusinessEntities.Any())
            {
                //锁定的
                var lockEntity = sfcProduceBusinessEntities.FirstOrDefault(x => x.BusinessType == ManuSfcProduceBusinessType.Lock);
                if (lockEntity != null)
                {
                    lockEntity.VerifyProcedureLock(sfc, procedureId);
                }

                //有缺陷的返修业务
                var repairEntity = sfcProduceBusinessEntities.FirstOrDefault(x => x.BusinessType == ManuSfcProduceBusinessType.Repair);
                if (repairEntity != null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES16319)).WithData("SFC", sfc);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processRouteId"></param>
        /// <param name="procedureId"></param>
        /// <returns></returns>
        private async Task<bool> IsLastProcedureIdAsync(long processRouteId, long procedureId)
        {
            var processRouteNodes = await _manuCommonOldService.GetProcessRouteAsync(processRouteId);
            var isLast = false;
            if (processRouteNodes.Any())
            {
                var id = processRouteNodes?.FirstOrDefault()?.ProcedureIds.Last() ?? 0;
                //判断是否末尾工序，末尾工序把条码改成已完成状态
                if (id == procedureId)
                {
                    isLast = true;
                }
            }
            return isLast;
        }
    }
}
