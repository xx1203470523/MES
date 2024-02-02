using FluentValidation;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Plan;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.Utils;
using System.Net.NetworkInformation;

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

        #region 仓储

        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IQualUnqualifiedCodeRepository _qualUnqualifiedCodeRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IPlanWorkOrderRepository _planWorkOrderRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;
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
        public ManuMarkingService(IProcProcedureRepository procProcedureRepository, AbstractValidator<ManuMarkingCheckDto> validationCheckRules, ICurrentSite currentSite, IQualUnqualifiedCodeRepository qualUnqualifiedCodeRepository, IManuSfcRepository manuSfcRepository, IManuSfcInfoRepository manuSfcInfoRepository, IProcMaterialRepository procMaterialRepository, IPlanWorkOrderRepository planWorkOrderRepository, IManuSfcProduceRepository manuSfcProduceRepository)
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
            var procedureEntities = await _procProcedureRepository.GetProcProcedureEntitiesAsync(new ProcProcedureQuery {SiteId= _currentSite.SiteId??0 });

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
            else {
                if (!string.IsNullOrWhiteSpace(checkDto.FoundBadOperationId))
                {
                    //foundBadprocedureEntity = await _procProcedureRepository.GetByCodeAsync(new EntityByCodeQuery { Code = checkDto.FoundBadOperationId, Site = _currentSite.SiteId });
                    foundBadprocedureEntity= procedureEntities.FirstOrDefault(a => a.Code == checkDto.FoundBadOperationId);
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
                interceptProcedureEntity= procedureEntities.FirstOrDefault(a => a.Code == checkDto.InterceptProcedureCode);
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
            else {
                if (!string.IsNullOrWhiteSpace(checkDto.UnqualifiedId)) {
                    unqualifiedCodeEntity = await _qualUnqualifiedCodeRepository.GetByCodeAsync(new QualUnqualifiedCodeByCodeQuery { Code = checkDto.UnqualifiedId, Site = _currentSite.SiteId });
                    if (unqualifiedCodeEntity == null)
                    {
                        throw new CustomerValidationException(nameof(ErrorCode.MES19708)).WithData("code", checkDto.UnqualifiedId);
                    }
                }
            }

            //校验产品序列码
            var sss = SfcStatusEnum.Invalid.GetDescription();
            if (string.IsNullOrWhiteSpace(checkDto.Sfc)) {
                throw new CustomerValidationException(nameof(ErrorCode.MES19703));
            }
            var sfcEntity = await _manuSfcRepository.GetBySFCAsync(new EntityBySFCQuery { SFC = checkDto.Sfc, SiteId = _currentSite.SiteId ?? 0 });
            if (sfcEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19706)).WithData("code", checkDto.Sfc);
            }
            else {
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
            var manuSfcInfoEntity = await _manuSfcInfoRepository.GetBySFCAsync(sfcEntity.Id);
            if (manuSfcInfoEntity == null) {
                throw new CustomerValidationException(nameof(ErrorCode.MES19709)).WithData("code", checkDto.Sfc);
            }

            //获取物料信息
            var materialEntity = await _procMaterialRepository.GetByIdAsync(manuSfcInfoEntity.ProductId);

            //获取工单信息
            var workOrderEntity = await _planWorkOrderRepository.GetByIdAsync(manuSfcInfoEntity.WorkOrderId);

            //查询条码在制表
            var sfcProduceEntities= await _manuSfcProduceRepository.GetListBySfcsAsync(new ManuSfcProduceBySfcsQuery()
            {
                SiteId = _currentSite.SiteId ?? 0,
                Sfcs=new List<string> {sfcEntity.SFC}
            });

            //条码在制表工序
            var sfcProduceEntity = new ProcProcedureEntity();
            if (sfcProduceEntities!=null&&sfcProduceEntities.Any()) {
                var produceId= sfcProduceEntities.FirstOrDefault()?.Id;
                sfcProduceEntity = procedureEntities.FirstOrDefault(a => a.Id == produceId);
            }

            var result = new MarkingEnterViewDto { 
                Sfc= sfcEntity.SFC,
                Status= sfcEntity.Status,
                ProcedureCode= sfcProduceEntity?.Code,
                WorkOrderCode= workOrderEntity?.OrderCode,
                MaterialCodeVersion=$"{materialEntity?.MaterialCode}/{materialEntity?.Version}",
                UnqualifiedCode= unqualifiedCodeEntity.UnqualifiedCode,
                UnqualifiedName= unqualifiedCodeEntity.UnqualifiedCodeName,
                FoundBadOperationCode= foundBadprocedureEntity.Code,
                InterceptProcedureCode=interceptProcedureEntity.Code,
                Remark=checkDto.Remark
            };

            return result;
        }
    }
}
