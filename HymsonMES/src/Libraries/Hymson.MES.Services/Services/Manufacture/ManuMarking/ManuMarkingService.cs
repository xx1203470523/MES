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

        #endregion

        #region 验证器

        private readonly AbstractValidator<ManuMarkingCheckDto> _validationCheckRules;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="procProcedureRepository"></param>
        /// <param name="validationCheckRules"></param>
        /// <param name="currentSite"></param>
        /// <param name="qualUnqualifiedCodeRepository"></param>
        /// <param name="manuSfcRepository"></param>
        /// <param name="manuSfcInfoRepository"></param>
        /// <param name="procMaterialRepository"></param>
        /// <param name="planWorkOrderRepository"></param>
        /// <param name="manuSfcProduceRepository"></param>
        /// <param name="manuProductBadRecordRepository"></param>
        /// <param name="manuSfcStepRepository"></param>
        /// <param name="currentUser"></param>
        public ManuMarkingService(IProcProcedureRepository procProcedureRepository, AbstractValidator<ManuMarkingCheckDto> validationCheckRules, ICurrentSite currentSite, IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IManuSfcRepository manuSfcRepository, IManuSfcInfoRepository manuSfcInfoRepository, IProcMaterialRepository procMaterialRepository, IPlanWorkOrderRepository planWorkOrderRepository, IManuSfcProduceRepository manuSfcProduceRepository, IManuProductBadRecordRepository manuProductBadRecordRepository, ICurrentUser currentUser, IManuSfcStepRepository manuSfcStepRepository, IProcResourceRepository procResourceRepository)
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

            var manuProductBadRecordEntities = await _manuProductBadRecordRepository.GetManuProductBadRecordEntitiesBySFCAsync(new ManuProductBadRecordBySfcQuery { SFC = sfc, SiteId = _currentSite.SiteId ?? 0 });

            var result = new List<MarkingCloseViewDto>();

            if (manuProductBadRecordEntities != null && manuProductBadRecordEntities.Any())
            {

                //获取不合格代码信息
                var qualUnqualifiedCodeIds = manuProductBadRecordEntities.Select(a => a.UnqualifiedId).Distinct();
                var qualUnqualifiedEntities = await _qualUnqualifiedCodeRepository.GetByIdsAsync(qualUnqualifiedCodeIds);

                //获取拦截工序信息
                var interceptOperationIds = manuProductBadRecordEntities.Select(a => a.InterceptOperationId.GetValueOrDefault()).Distinct();
                var procProcedureEntities = await _procProcedureRepository.GetByIdsAsync(interceptOperationIds);

                var sfcProcedureInfoEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery { Sfc = sfc, SiteId = _currentSite.SiteId ?? 0 });

                var resourceEntity = new ProcResourceView();
                if (sfcProcedureInfoEntity != null)
                {
                    resourceEntity = await _procResourceRepository.GetByIdAsync(sfcProcedureInfoEntity.ResourceId.GetValueOrDefault());
                }

                foreach (var item in manuProductBadRecordEntities)
                {
                    var model = new MarkingCloseViewDto();
                    model.Id = item.Id;
                    model.Remark = item.Remark;
                    if (item.Status == ProductBadRecordStatusEnum.Open)
                    {
                        model.Status = true;
                    }
                    if (item.Status == ProductBadRecordStatusEnum.Close)
                    {
                        model.Status = false;
                    }
                    //model.Status = item.Status;

                    var qualUnqualifiedEntity = qualUnqualifiedEntities.FirstOrDefault(a => a.Id == item.UnqualifiedId);
                    model.UnqualifiedCode = qualUnqualifiedEntity?.UnqualifiedCode;
                    model.UnqualifiedName = qualUnqualifiedEntity?.UnqualifiedCodeName;

                    var procProcedureEntity = procProcedureEntities.FirstOrDefault(a => a.Id == item.InterceptOperationId);
                    model.InterceptProcedureCode = procProcedureEntity?.Code;
                    model.ResourceCode = resourceEntity?.ResCode;

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
        /// <param name="markingCloseConfirmDto"></param>
        /// <returns></returns>
        public async Task SaveMarkingCloseAsync(MarkingCloseConfirmDto markingCloseConfirmDto)
        {
            if (string.IsNullOrWhiteSpace(markingCloseConfirmDto.Sfc))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15432));
            }

            if (markingCloseConfirmDto.MarkingCloseConfirmBadRecords == null || !markingCloseConfirmDto.MarkingCloseConfirmBadRecords.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19711));
            }

            //获取条码信息
            var sfcEntity = await _manuSfcRepository.GetSingleAsync(new ManuSfcQuery
            {
                SFC = markingCloseConfirmDto.Sfc,
                SiteId = _currentSite.SiteId ?? 0,
                Type = SfcTypeEnum.Produce
            });
            if (sfcEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES15439)).WithData("sfc", markingCloseConfirmDto.Sfc);
            }
            //校验SFC状态
            switch (sfcEntity.Status)
            {
                case SfcStatusEnum.Scrapping:
                    throw new CustomerValidationException(nameof(ErrorCode.MES19707)).WithData("code", markingCloseConfirmDto.Sfc).WithData("status", SfcStatusEnum.Invalid.GetDescription());
                //break;
                case SfcStatusEnum.Delete:
                    throw new CustomerValidationException(nameof(ErrorCode.MES19707)).WithData("code", markingCloseConfirmDto.Sfc).WithData("status", SfcStatusEnum.Delete.GetDescription());
                //break;
                case SfcStatusEnum.Invalid:
                    throw new CustomerValidationException(nameof(ErrorCode.MES19707)).WithData("code", markingCloseConfirmDto.Sfc).WithData("status", SfcStatusEnum.Invalid.GetDescription());
                //break;
                default:
                    break;
            }

            //在制品信息
            var manuSfcProduceInfoEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery { Sfc = markingCloseConfirmDto.Sfc, SiteId = _currentSite.SiteId ?? 0 });

            //条码信息表
            var sfcInfoEntity = await _manuSfcInfoRepository.GetBySFCIdWithIsUseAsync(sfcEntity.Id);

            //组装步骤表数据
            var manuSfcStepEntity = new ManuSfcStepEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SFC = markingCloseConfirmDto.Sfc,
                ProductId = sfcInfoEntity?.ProductId ?? 0,
                WorkOrderId = sfcInfoEntity?.WorkOrderId ?? 0,
                WorkCenterId = manuSfcProduceInfoEntity?.WorkCenterId,
                ProductBOMId = manuSfcProduceInfoEntity?.ProductBOMId,
                ProcessRouteId = manuSfcProduceInfoEntity?.ProcessRouteId,
                Qty = sfcEntity?.Qty ?? 0,
                EquipmentId = manuSfcProduceInfoEntity?.EquipmentId,
                ResourceId = manuSfcProduceInfoEntity?.ResourceId,
                ProcedureId = manuSfcProduceInfoEntity?.ProcedureId,
                Operatetype = ManuSfcStepTypeEnum.CloseMarking,
                CurrentStatus = sfcEntity!.Status,
                SiteId = _currentSite.SiteId ?? 0,
                CreatedOn = HymsonClock.Now(),
                CreatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now(),
                UpdatedBy = _currentUser.UserName,
                Remark = markingCloseConfirmDto.SfcStepRemark
            };

            //组装不良录入数据
            var updateManuProductBadRecords = new List<ManuProductBadRecordEntity>();
            foreach (var item in markingCloseConfirmDto.MarkingCloseConfirmBadRecords)
            {
                if (item.Id == null || item.Status == null)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19711));
                }

                var manuProductBadRecordEntity = new ManuProductBadRecordEntity
                {
                    Id = item.Id.GetValueOrDefault(),
                    Remark = item.Remark ?? "",
                    Status = item.Status.GetValueOrDefault(),
                    CloseSfcStepId = manuSfcStepEntity.Id,
                    UpdatedBy = _currentUser.UserName,
                    UpdatedOn = HymsonClock.Now()
                };

                updateManuProductBadRecords.Add(manuProductBadRecordEntity);
            }

            using (var trans = TransactionHelper.GetTransactionScope())
            {
                //修改不合格记录
                if (updateManuProductBadRecords != null && updateManuProductBadRecords.Any())
                {
                    var insertResult = await _manuProductBadRecordRepository.UpdateByMarkingCloseRangeAsync(updateManuProductBadRecords);
                    if (insertResult != updateManuProductBadRecords.Count)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19712));
                    }
                }
                //插入步骤表
                var insertSfcStepResult = await _manuSfcStepRepository.InsertAsync(manuSfcStepEntity);
                if (insertSfcStepResult == 0)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES19712));
                }

                trans.Complete();
            }
        }
    }
}
