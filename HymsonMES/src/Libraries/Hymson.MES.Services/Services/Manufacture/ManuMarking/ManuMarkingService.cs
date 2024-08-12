using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums.QualUnqualifiedCode;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Manufacture.ManuSfcMarking.Command;
using Hymson.MES.Data.Repositories.Manufacture.Query;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;

namespace Hymson.MES.Services.Services.Manufacture.ManuMarking
{
    /// <summary>
    /// Marking标识服务层
    /// </summary>
    public class ManuMarkingService : IManuMarkingService
    {
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;
        private readonly ICurrentUser _currentUser;

        #region 仓储

        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
        private readonly IManuProductBadRecordRepository _manuProductBadRecordRepository;
        private readonly IManuSfcStepRepository _manuSfcStepRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IManuSfcMarkingRepository _manuSfcMarkingRepository;
        private readonly IManuSfcMarkingExecuteRepository _manuSfcMarkingExecuteRepository;

        #endregion

        #region 验证器

        private readonly AbstractValidator<ManuMarkingCheckDto> _validationCheckRules;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManuMarkingService(IProcProcedureRepository procProcedureRepository,
            AbstractValidator<ManuMarkingCheckDto> validationCheckRules,
            ICurrentSite currentSite,
            IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository,
            IManuSfcRepository manuSfcRepository,
            IManuSfcInfoRepository manuSfcInfoRepository,
            IProcMaterialRepository procMaterialRepository,
            IPlanWorkOrderRepository planWorkOrderRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IManuProductBadRecordRepository manuProductBadRecordRepository,
            ICurrentUser currentUser,
            IManuSfcStepRepository manuSfcStepRepository,
            IProcResourceRepository procResourceRepository,
            IManuSfcMarkingRepository manuSfcMarkingRepository,
            IManuSfcMarkingExecuteRepository manuSfcMarkingExecuteRepository)
        {
            _procProcedureRepository = procProcedureRepository;
            _validationCheckRules = validationCheckRules;
            _currentSite = currentSite;
            _qualUnqualifiedCodeRepository = qualUnqualifiedCodeRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _procMaterialRepository = procMaterialRepository;
            _planWorkOrderRepository = planWorkOrderRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
            _manuProductBadRecordRepository = manuProductBadRecordRepository;
            _currentUser = currentUser;
            _manuSfcStepRepository = manuSfcStepRepository;
            _procResourceRepository = procResourceRepository;
            _manuSfcMarkingRepository = manuSfcMarkingRepository;
            _manuSfcMarkingExecuteRepository = manuSfcMarkingExecuteRepository;
        }

        /// <summary>
        /// Marking录入校验
        /// </summary>
        /// <param name="checkDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MarkingEnterViewDto> CheckMarkingEnterAsync(ManuMarkingCheckDto checkDto)
        {
            // 验证DTO
            await _validationCheckRules.ValidateAndThrowAsync(checkDto);

            //获取工序信息
            var procedureEntities = await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery { SiteId = _currentSite.SiteId ?? 0 });

            //校验发现工序
            var foundBadprocedureEntity = new ProcProcedureEntity();
            if (!string.IsNullOrWhiteSpace(checkDto.FoundBadOperationCode))
            {
                foundBadprocedureEntity = procedureEntities.FirstOrDefault(a => a.Code == checkDto.FoundBadOperationCode);
                if (foundBadprocedureEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19704)).WithData("code", checkDto.FoundBadOperationCode);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(checkDto.FoundBadOperationId))
                {
                    //foundBadprocedureEntity = await _procProcedureRepository.GetByCodeAsync(new EntityByCodeQuery { Code = checkDto.FoundBadOperationId, Site = _currentSite.SiteId });
                    foundBadprocedureEntity = procedureEntities.FirstOrDefault(a => a.Code == checkDto.FoundBadOperationId);
                    if (foundBadprocedureEntity == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19704)).WithData("code", checkDto.FoundBadOperationId);
                    }
                }
            }

            //校验拦截工序
            var interceptProcedureEntity = new ProcProcedureEntity();
            if (!string.IsNullOrWhiteSpace(checkDto.InterceptProcedureCode))
            {
                //interceptProcedureIdEntity = await _procProcedureRepository.GetByCodeAsync(new EntityByCodeQuery { Code = checkDto.InterceptProcedureCode, Site = _currentSite.SiteId });
                interceptProcedureEntity = procedureEntities.FirstOrDefault(a => a.Code == checkDto.InterceptProcedureCode);
                if (interceptProcedureEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19705)).WithData("code", checkDto.InterceptProcedureCode);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(checkDto.InterceptProcedureId))
                {
                    //interceptProcedureIdEntity = await _procProcedureRepository.GetByCodeAsync(new EntityByCodeQuery { Code = checkDto.InterceptProcedureId, Site = _currentSite.SiteId });
                    interceptProcedureEntity = procedureEntities.FirstOrDefault(a => a.Code == checkDto.InterceptProcedureId);
                    if (interceptProcedureEntity == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19705)).WithData("code", checkDto.InterceptProcedureId);
                    }
                }
            }

            //校验不合格代码
            var unqualifiedCodeEntity = new QualUnqualifiedCodeEntity();
            if (!string.IsNullOrWhiteSpace(checkDto.UnqualifiedCode))
            {
                unqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByCodeAsync(new QualUnqualifiedCodeByCodeQuery { Code = checkDto.UnqualifiedCode, Site = _currentSite.SiteId });
                if (unqualifiedCodeEntity == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19708)).WithData("code", checkDto.UnqualifiedCode);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(checkDto.UnqualifiedId))
                {
                    unqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByCodeAsync(new QualUnqualifiedCodeByCodeQuery { Code = checkDto.UnqualifiedId, Site = _currentSite.SiteId });
                    if (unqualifiedCodeEntity == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19708)).WithData("code", checkDto.UnqualifiedId);
                    }
                }
            }
            //校验不良代码是否为标识
            if (unqualifiedCodeEntity.Type != QualUnqualifiedCodeTypeEnum.Mark)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15437));
            }

            //校验产品序列码
            if (string.IsNullOrWhiteSpace(checkDto.Sfc))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19703));
            }
            var sfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = checkDto.Sfc,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            if (sfcEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19706)).WithData("code", checkDto.Sfc);
            }
            else
            {
                switch (sfcEntity.Status)
                {
                    case SfcStatusEnum.Scrapping:
                        throw new CustomerValidationException(nameof(ErrorCode.MES19707)).WithData("code", checkDto.Sfc).WithData("status", SfcStatusEnum.Invalid.GetDescription());
                    //break;
                    case SfcStatusEnum.Delete:
                        throw new CustomerValidationException(nameof(ErrorCode.MES19707)).WithData("code", checkDto.Sfc).WithData("status", SfcStatusEnum.Delete.GetDescription());
                    //break;
                    case SfcStatusEnum.Invalid:
                        throw new CustomerValidationException(nameof(ErrorCode.MES19707)).WithData("code", checkDto.Sfc).WithData("status", SfcStatusEnum.Invalid.GetDescription());
                    //break;
                    default:
                        break;
                }
            }

            //获取产品序列码信息
            var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdWithIsUseAsync(sfcEntity.Id);
            if (manuSfcInfoEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19709)).WithData("code", checkDto.Sfc);
            }

            //校验是否已打过Marking
            var markedEntities = await _manuSfcMarkingExecuteRepository.GetEntitiesAsync(new ManuSfcMarkingExecuteQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = checkDto.Sfc,
                UnqualifiedCodeId = unqualifiedCodeEntity.Id
            });
            if (markedEntities != null && markedEntities.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19715)).WithData("sfc", checkDto.Sfc).WithData("unqualifiedCode", unqualifiedCodeEntity.UnqualifiedCode);
            }

            //获取物料信息
            var materialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcInfoEntity.ProductId);

            //获取工单信息
            var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcInfoEntity.WorkOrderId ?? 0);

            //查询条码在制表
            var sfcProduceEntities = await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs = new List<string> { sfcEntity.SFC }
            });

            //条码在制表工序
            var sfcProduceEntity = new ProcProcedureEntity();
            if (sfcProduceEntities != null && sfcProduceEntities.Any())
            {
                var produceId = sfcProduceEntities.FirstOrDefault()?.ProcedureId;
                sfcProduceEntity = procedureEntities.FirstOrDefault(a => a.Id == produceId);
            }

            var result = new MarkingEnterViewDto
            {
                Sfc = sfcEntity.SFC,
                Status = sfcEntity.Status,
                ProcedureCode = sfcProduceEntity?.Code,
                WorkOrderCode = workOrderEntity?.OrderCode,
                MaterialCodeVersion = $"{materialEntity?.MaterialCode}/{materialEntity?.Version}",
                UnqualifiedCode = unqualifiedCodeEntity.UnqualifiedCode,
                UnqualifiedName = unqualifiedCodeEntity.UnqualifiedCodeName,
                FoundBadOperationCode = foundBadprocedureEntity.Code,
                InterceptProcedureCode = interceptProcedureEntity.Code,
                Remark = checkDto.Remark
            };

            return result;
        }

        /// <summary>
        /// Marking关闭检索
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public async Task<IEnumerable<MarkingCloseViewDto>> GetBarcodePagedListBySFCAsync(string sfc)
        {
            //校验产品序列码
            if (string.IsNullOrWhiteSpace(sfc))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19703));
            }
            var sfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = sfc,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            if (sfcEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19706)).WithData("code", sfc);
            }

            var markingEntities = await _manuSfcMarkingExecuteRepository.GetEntitiesAsync(new ManuSfcMarkingExecuteQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                SFC = sfc
            });

            var result = new List<MarkingCloseViewDto>();

            if (markingEntities != null && markingEntities.Any())
            {
                //获取不合格代码信息
                var qualUnqualifiedCodeIds = markingEntities.Select(a => a.UnqualifiedCodeId).Distinct();
                var qualUnqualifiedEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(qualUnqualifiedCodeIds);

                //获取拦截工序信息
                var interceptOperationIds = markingEntities.Select(a => a.ShouldInterceptProcedureId).Distinct();
                var procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(interceptOperationIds);

                var sfcProcedureInfoEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery { Sfc = sfc, SiteId = _currentSite.SiteId ?? 0 });

                var resourceEntity = new ProcResourceView();
                if (sfcProcedureInfoEntity != null)
                {
                    resourceEntity = await _procResourceRepository.GetByIdAsync(sfcProcedureInfoEntity.ResourceId.GetValueOrDefault());
                }

                foreach (var item in markingEntities)
                {
                    var model = new MarkingCloseViewDto();
                    model.Id = item.Id;
                    model.Remark = item.Remark;
                    model.Status = false;

                    var qualUnqualifiedEntity = qualUnqualifiedEntities.FirstOrDefault(a => a.Id == item.UnqualifiedCodeId);
                    model.UnqualifiedCode = qualUnqualifiedEntity?.UnqualifiedCode;
                    model.UnqualifiedName = qualUnqualifiedEntity?.UnqualifiedCodeName;

                    var procProcedureEntity = procProcedureEntities.FirstOrDefault(a => a.Id == item.ShouldInterceptProcedureId);
                    model.InterceptProcedureCode = procProcedureEntity?.Code;
                    model.ResourceCode = item.CreatedBy;

                    result.Add(model);
                }
            }

            return result;
        }

        /// <summary>
        /// Marking关闭SFC校验
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CheckSFCMarkingCloseAsync(string sfc)
        {
            //校验产品序列码
            if (string.IsNullOrWhiteSpace(sfc))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19703));
            }
            var sfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = sfc,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            if (sfcEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19706)).WithData("code", sfc);
            }
            else
            {
                switch (sfcEntity.Status)
                {
                    case SfcStatusEnum.Scrapping:
                        throw new CustomerValidationException(nameof(ErrorCode.MES19707)).WithData("code", sfc).WithData("status", SfcStatusEnum.Invalid.GetDescription());
                    //break;
                    case SfcStatusEnum.Delete:
                        throw new CustomerValidationException(nameof(ErrorCode.MES19707)).WithData("code", sfc).WithData("status", SfcStatusEnum.Delete.GetDescription());
                    //break;
                    case SfcStatusEnum.Invalid:
                        throw new CustomerValidationException(nameof(ErrorCode.MES19707)).WithData("code", sfc).WithData("status", SfcStatusEnum.Invalid.GetDescription());
                    //break;
                    default:
                        break;
                }
            }

        }

        /// <summary>
        /// Marking关闭确认提交
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task SaveMarkingCloseAsync(MarkingCloseConfirmDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Sfc))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15432));
            }

            if (dto.MarkingCloseConfirmBadRecords == null || !dto.MarkingCloseConfirmBadRecords.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19711));
            }

            //查询Marking记录
            var markingExecuteEntities = await _manuSfcMarkingExecuteRepository.GetByIdsAsync(dto.MarkingCloseConfirmBadRecords.Select(x => x.Id.GetValueOrDefault()).ToArray());
            if (!markingExecuteEntities.Any())
            {
                return;
            }

            //
            var updateMarkingList = new List<UpdateMarkingStatusCommand>();
            foreach (var item in dto.MarkingCloseConfirmBadRecords)
            {
                var entity = markingExecuteEntities.FirstOrDefault(x => x.Id == item.Id);
                if (entity == null)
                {
                    continue;
                }
                updateMarkingList.Add(new UpdateMarkingStatusCommand
                {
                    Id = entity.SfcMarkingId,
                    Status = MarkingStatusEnum.Close,
                    Remark = string.IsNullOrWhiteSpace(item.Remark) ? dto.SfcStepRemark : item.Remark,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                });
            }

            using var trans = TransactionHelper.GetTransactionScope();
            //物理删除manu_sfc_marking_execute表数据
            await _manuSfcMarkingExecuteRepository.DeletesPhysicsAsync(markingExecuteEntities.Select(x => x.Id).Distinct());
            //修改manu_sfc_marking状态
            await _manuSfcMarkingRepository.UpdateMarkingStatusAsync(updateMarkingList);
            trans.Complete();
        }
    }
}
