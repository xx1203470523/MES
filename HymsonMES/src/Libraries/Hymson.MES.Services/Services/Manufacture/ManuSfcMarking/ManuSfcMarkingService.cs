using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Excel.Abstractions;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.CoreServices.Services;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 服务（Marking信息表） 
    /// </summary>
    public class ManuSfcMarkingService : IManuSfcMarkingService
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 参数验证器
        /// </summary>
        private readonly AbstractValidator<IEnumerable<ManuSfcMarkingSaveDto>> _validationSaveRules;

        /// <summary>
        /// 仓储接口（Marking信息表）
        /// </summary>
        private readonly IManuSfcMarkingRepository _manuSfcMarkingRepository;
        private readonly IManuSfcMarkingExecuteRepository _manuSfcMarkingExecuteRepository;
        private readonly IManuSfcMarkingInterceptRepository _manuSfcMarkingInterceptRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;

        private readonly ITracingSourceCoreService _tracingSourceCoreService;
        private readonly IExcelService _excelService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuSfcMarkingService(ICurrentUser currentUser,
            ICurrentSite currentSite,
            AbstractValidator<IEnumerable<ManuSfcMarkingSaveDto>> validationSaveRules,
            IManuSfcMarkingRepository manuSfcMarkingRepository,
            IManuSfcMarkingExecuteRepository manuSfcMarkingExecuteRepository,
            IManuSfcMarkingInterceptRepository manuSfcMarkingInterceptRepository,
            IProcProcedureRepository procProcedureRepository,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            ITracingSourceCoreService tracingSourceCoreService,
            IExcelService excelService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _manuSfcMarkingRepository = manuSfcMarkingRepository;
            _manuSfcMarkingExecuteRepository = manuSfcMarkingExecuteRepository;
            _manuSfcMarkingInterceptRepository = manuSfcMarkingInterceptRepository;
            _procProcedureRepository = procProcedureRepository;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _tracingSourceCoreService = tracingSourceCoreService;
            _excelService = excelService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="saveDto"></param>
        /// <returns></returns>
        public async Task CreateAsync(IEnumerable<ManuSfcMarkingSaveDto> saveDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10101));

            // 验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(saveDto);

            //工序
            //var procedureEntities = await _procProcedureRepository.GetEntitiesAsync(new ProcProcedureQuery { SiteId = _currentSite.SiteId ?? 0 });
            //不合格代码
            var unqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(saveDto.Select(x => x.UnqualifiedId).Distinct());

            //组装数据
            var markingEntities = new List<ManuSfcMarkingEntity>();
            var markingExecuteEntities = new List<ManuSfcMarkingExecuteEntity>();

            //按不合格代码、发现不良工序分组处理
            var unqualifiedGroupSfcs = saveDto.GroupBy(x => new { UnqualifiedCodeId = x.UnqualifiedId });
            foreach (var group in unqualifiedGroupSfcs)
            {
                //校验是否已标记过Marking
                var markedEntities = await _manuSfcMarkingExecuteRepository.GetEntitiesAsync(new ManuSfcMarkingExecuteQuery
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    UnqualifiedCodeId = group.Key.UnqualifiedCodeId,
                    SFCs = group.Select(x => x.SFC).Distinct()
                });
                if (markedEntities != null && markedEntities.Any())
                {
                    var unqualifiedCode = unqualifiedCodeEntities.FirstOrDefault(x => x.Id == group.Key.UnqualifiedCodeId)?.UnqualifiedCode ?? "";
                    throw new CustomerValidationException(nameof(ErrorCode.MES19715)).WithData("unqualifiedCode", unqualifiedCode).WithData("sfc", string.Join(',', markedEntities.Select(x => x.SFC)));
                }

                //循环处理组中每个条码
                foreach (var item in group)
                {
                    //去除重复(针对本次录入条码列表中有父子关系的情况)
                    if (markingExecuteEntities.Any(x => x.SFC == item.SFC && x.UnqualifiedCodeId == item.UnqualifiedId))
                    {
                        continue;
                    }
                    //追溯条码信息
                    var tracingSfcList = await _tracingSourceCoreService.DestinationListAsync(new EntityBySFCQuery
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        SFC = item.SFC
                    });
                    //过滤原材料码
                    tracingSfcList = tracingSfcList?.Where(x => !x.SFC.Contains(','));
                    if (tracingSfcList == null || !tracingSfcList.Any())
                    {
                        continue;
                    }
                    //过滤已标记过相同不合格代码的追溯条码
                    tracingSfcList = tracingSfcList.Where(x => !markingExecuteEntities.Any(z => z.SFC == x.SFC && z.UnqualifiedCodeId == item.UnqualifiedId));
                    if (!tracingSfcList.Any())
                    {
                        continue;
                    }
                    var tracingSfcMarkedEntities = await _manuSfcMarkingExecuteRepository.GetEntitiesAsync(new ManuSfcMarkingExecuteQuery
                    {
                        SiteId = _currentSite.SiteId ?? 0,
                        UnqualifiedCodeId = item.UnqualifiedId,
                        SFCs = tracingSfcList.Select(x => x.SFC)
                    });
                    if (tracingSfcMarkedEntities != null && tracingSfcMarkedEntities.Any())
                    {
                        tracingSfcList = tracingSfcList.Where(x => !tracingSfcMarkedEntities.Any(z => z.SFC == x.SFC && z.UnqualifiedCodeId == item.UnqualifiedId));
                    }
                    if (!tracingSfcList.Any())
                    {
                        continue;
                    }

                    //添加追溯条码信息
                    foreach (var tracingSfc in tracingSfcList)
                    {
                        if (markingExecuteEntities.Any(x => x.SFC == tracingSfc.SFC && x.UnqualifiedCodeId == item.UnqualifiedId))
                        {
                            continue;
                        }
                        var id = IdGenProvider.Instance.CreateId();

                        markingEntities.Add(new ManuSfcMarkingEntity
                        {
                            Id = id,
                            SiteId = _currentSite.SiteId ?? 0,
                            SFC = tracingSfc.SFC,
                            FoundBadProcedureId = item.FoundBadOperationId,
                            UnqualifiedCodeId = item.UnqualifiedId,
                            ShouldInterceptProcedureId = item.InterceptProcedureId,
                            Status = MarkingStatusEnum.Open,
                            MarkingType = item.InterceptProcedureId == 0 ? MarkingTypeEnum.Mark : MarkingTypeEnum.Intercept,
                            SourceType = tracingSfc.SFC == item.SFC ? MarkingSourceTypeEnum.Directly : MarkingSourceTypeEnum.Inherited,
                            ParentSFC = tracingSfc.ParentNodeSFC,
                            OriginalSFC = item.SFC,
                            Remark = item.Remark,
                            CreatedBy = _currentUser.UserName,
                            UpdatedBy = _currentUser.UserName
                        });

                        markingExecuteEntities.Add(new ManuSfcMarkingExecuteEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteId = _currentSite.SiteId ?? 0,
                            SfcMarkingId = id,
                            SFC = tracingSfc.SFC,
                            FoundBadProcedureId = item.FoundBadOperationId,
                            UnqualifiedCodeId = item.UnqualifiedId,
                            ShouldInterceptProcedureId = item.InterceptProcedureId,
                            MarkingType = item.InterceptProcedureId == 0 ? MarkingTypeEnum.Mark : MarkingTypeEnum.Intercept,
                            Remark = item.Remark,
                            CreatedBy = _currentUser.UserName,
                            UpdatedBy = _currentUser.UserName
                        });
                    }

                }
            }

            using var trans = TransactionHelper.GetTransactionScope();
            await _manuSfcMarkingRepository.InsertRangeAsync(markingEntities);
            await _manuSfcMarkingExecuteRepository.InsertRangeAsync(markingExecuteEntities);
            trans.Complete();
        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        public async Task ImportAsync([FromForm(Name = "file")] IFormFile formFile)
        {
            using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);
            var excelImportDtos = _excelService.Import<ManuProductBadRecordMarkEntryImportDto>(memoryStream);

            if (excelImportDtos == null || !excelImportDtos.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES14908));
            }

            #region 验证数据

            foreach (var item in excelImportDtos)
            {
                if (string.IsNullOrEmpty(item.SFC)) throw new CustomerValidationException(nameof(ErrorCode.MES15432));
                if (string.IsNullOrEmpty(item.FoundBadOperationCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15433));
                if (string.IsNullOrEmpty(item.UnqualifiedCode)) throw new CustomerValidationException(nameof(ErrorCode.MES15434));
            }

            //发现不良工序
            var procedureCodes = excelImportDtos.Select(x => x.FoundBadOperationCode).Distinct();
            var procedureEntities = await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = procedureCodes
            });
            var notExistProcedureCodes = procedureCodes.Except(procedureEntities.Select(x => x.Code).Distinct());
            if (notExistProcedureCodes != null && notExistProcedureCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15443)).WithData("foundBadOperationCode", string.Join(",", notExistProcedureCodes));
            }

            //拦截工序
            IEnumerable<ProcProcedureEntity> interceptProcedureEntities = new List<ProcProcedureEntity>();
            var interceptProcedureCodes = excelImportDtos.Where(x => !string.IsNullOrWhiteSpace(x.InterceptProcedureCode)).Select(x => x.InterceptProcedureCode ?? "")?.Distinct();
            if (interceptProcedureCodes != null && interceptProcedureCodes.Any())
            {
                interceptProcedureEntities = await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery()
                {
                    SiteId = _currentSite.SiteId ?? 0,
                    Codes = interceptProcedureCodes
                });
                var notExistInterceptProcedureCodes = interceptProcedureCodes.Except(interceptProcedureEntities.Select(x => x.Code).Distinct());
                if (notExistInterceptProcedureCodes != null && notExistInterceptProcedureCodes.Any())
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES15444)).WithData("interceptProcedureCode", string.Join(",", notExistInterceptProcedureCodes));
                }
            }

            //不合格代码
            var unqualifiedCodes = excelImportDtos.Select(x => x.UnqualifiedCode).Distinct();
            var unqualifiedCodeEntities = await _qualUnqualifiedCodeRepository.GetByCodesAsync(new QualUnqualifiedCodeByCodesQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Codes = unqualifiedCodes
            });
            var notExistUnquaCodes = unqualifiedCodes.Except(unqualifiedCodeEntities.Select(x => x.UnqualifiedCode).Distinct());
            if (notExistUnquaCodes != null && notExistUnquaCodes.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15441)).WithData("unqualifiedCode", string.Join(",", notExistUnquaCodes));
            }

            #endregion

            var param = excelImportDtos.Select(item => new ManuSfcMarkingSaveDto
            {
                SFC = item.SFC,
                FoundBadOperationId = procedureEntities.FirstOrDefault(x => x.Code == item.FoundBadOperationCode)?.Id ?? 0,
                UnqualifiedId = unqualifiedCodeEntities.FirstOrDefault(x => x.UnqualifiedCode == item.UnqualifiedCode)?.Id ?? 0,
                InterceptProcedureId = interceptProcedureEntities.FirstOrDefault(x => x.Code == item.InterceptProcedureCode)?.Id ?? 0,
                Remark = item.Remark
            });

            await CreateAsync(param);
        }

        /// <summary>
        /// 获取打开状态Marking信息
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MarkingInfoDto>> GetOpenMarkingListBySFCAsync(string sfc)
        {
            //校验产品序列码
            if (string.IsNullOrWhiteSpace(sfc))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19703));
            }

            var markingEntities = await _manuSfcMarkingExecuteRepository.GetEntitiesAsync(new ManuSfcMarkingExecuteQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = sfc
            });

            var result = new List<MarkingInfoDto>();

            if (markingEntities != null && markingEntities.Any())
            {
                //获取不合格代码信息
                var qualUnqualifiedCodeIds = markingEntities.Select(a => a.UnqualifiedCodeId).Distinct();
                var qualUnqualifiedEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(qualUnqualifiedCodeIds);

                //获取拦截工序信息
                var interceptOperationIds = markingEntities.Select(a => a.ShouldInterceptProcedureId).Distinct();
                var procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(interceptOperationIds);

                foreach (var item in markingEntities)
                {
                    var qualUnqualifiedEntity = qualUnqualifiedEntities.FirstOrDefault(a => a.Id == item.UnqualifiedCodeId);
                    var procProcedureEntity = procProcedureEntities.FirstOrDefault(a => a.Id == item.ShouldInterceptProcedureId);

                    var model = new MarkingInfoDto
                    {
                        Id = item.Id,
                        Remark = item.Remark ?? "",
                        Status = true,
                        UnqualifiedCode = qualUnqualifiedEntity?.UnqualifiedCode ?? "",
                        UnqualifiedName = qualUnqualifiedEntity?.UnqualifiedCodeName ?? "",
                        InterceptProcedureCode = procProcedureEntity?.Code ?? "",
                        ResourceCode = item.CreatedBy
                    };

                    result.Add(model);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuSfcMarkingDto?> QueryByIdAsync(long id) 
        {
           var manuSfcMarkingEntity = await _manuSfcMarkingRepository.GetByIdAsync(id);
           if (manuSfcMarkingEntity == null) return null;
           
           return manuSfcMarkingEntity.ToModel<ManuSfcMarkingDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuSfcMarkingDto>> GetPagedListAsync(ManuSfcMarkingPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ManuSfcMarkingPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _manuSfcMarkingRepository.GetPagedListAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ManuSfcMarkingDto>());
            return new PagedInfo<ManuSfcMarkingDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

    }
}
