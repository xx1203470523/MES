/*
 *creator: Karl
 *
 *describe: 环境检验单    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-03-22 05:04:53
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.QualEnvOrder;
using Hymson.MES.CoreServices.Bos.Quality;
using Hymson.MES.CoreServices.Services.Quality;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.QualEnvOrder;
using Hymson.MES.Services.Dtos.QualEnvOrder;
using Hymson.Snowflake;
using Hymson.Utils;
using Org.BouncyCastle.Pqc.Crypto.Frodo;
using System.Transactions;

namespace Hymson.MES.Services.Services.QualEnvOrder
{
    /// <summary>
    /// 环境检验单 服务
    /// </summary>
    public class QualEnvOrderService : IQualEnvOrderService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 环境检验单 仓储
        /// </summary>
        private readonly IQualEnvOrderRepository _qualEnvOrderRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IInteWorkCenterRepository _inteWorkCenterRepository;
        private readonly AbstractValidator<QualEnvOrderCreateDto> _validationCreateRules;
        private readonly AbstractValidator<QualEnvOrderModifyDto> _validationModifyRules;

        private readonly IEnvOrderCreateService _envOrderCreateService;

        public QualEnvOrderService(ICurrentUser currentUser, ICurrentSite currentSite, IQualEnvOrderRepository qualEnvOrderRepository, AbstractValidator<QualEnvOrderCreateDto> validationCreateRules, AbstractValidator<QualEnvOrderModifyDto> validationModifyRules, IProcProcedureRepository procProcedureRepository, IInteWorkCenterRepository inteWorkCenterRepository, IEnvOrderCreateService envOrderCreateService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _qualEnvOrderRepository = qualEnvOrderRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procProcedureRepository = procProcedureRepository;
            _inteWorkCenterRepository = inteWorkCenterRepository;
            _envOrderCreateService = envOrderCreateService;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="qualEnvOrderCreateDto"></param>
        /// <returns></returns>
        public async Task CreateQualEnvOrderAsync(QualEnvOrderCreateDto qualEnvOrderCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(qualEnvOrderCreateDto);

            var bo = new EnvOrderManualCreateBo
            {
                SiteId = _currentSite.SiteId ?? 0,
                UserName = _currentUser.UserName,
                WorkCenterId = qualEnvOrderCreateDto.WorkCenterId,
                ProcedureId = qualEnvOrderCreateDto.ProcedureId
            };

            await _envOrderCreateService.ManualCreateAsync(bo);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteQualEnvOrderAsync(long id)
        {
            await _qualEnvOrderRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualEnvOrderAsync(long[] ids)
        {
            return await _qualEnvOrderRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="qualEnvOrderPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualEnvOrderDto>> GetPagedListAsync(QualEnvOrderPagedQueryDto qualEnvOrderPagedQueryDto)
        {
            var qualEnvOrderPagedQuery = qualEnvOrderPagedQueryDto.ToQuery<QualEnvOrderPagedQuery>();
            qualEnvOrderPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var qualEnvOrderDtos = new List<QualEnvOrderDto>();

            if (!string.IsNullOrWhiteSpace(qualEnvOrderPagedQueryDto.WorkCenterCode)|| !string.IsNullOrWhiteSpace(qualEnvOrderPagedQueryDto.WorkCenterName))
            {
                var inteWorkCenterQuery = new InteWorkCenterFirstQuery { SiteId = _currentSite.SiteId ?? 0, Code = qualEnvOrderPagedQueryDto.WorkCenterCode, Name = qualEnvOrderPagedQueryDto.WorkCenterName };
                var inteWorkCenter = await _inteWorkCenterRepository.GetEntitieAsync(inteWorkCenterQuery); 
                if (inteWorkCenter == null)
                {
                    return new PagedInfo<QualEnvOrderDto>(qualEnvOrderDtos, qualEnvOrderPagedQueryDto.PageIndex, qualEnvOrderPagedQueryDto.PageSize, 0);
                }
                qualEnvOrderPagedQuery.WorkCenterId = inteWorkCenter.Id;
            }
            if (!string.IsNullOrWhiteSpace(qualEnvOrderPagedQueryDto.ProcedureCode) || !string.IsNullOrWhiteSpace(qualEnvOrderPagedQueryDto.ProcedureName))
            {

                var procProcedureQuery = new ProcProcedureQuery {  SiteId = _currentSite.SiteId??0, Code = qualEnvOrderPagedQueryDto.ProcedureCode, Name = qualEnvOrderPagedQueryDto.ProcedureName };
                var procProcedure = await _procProcedureRepository.GetEntitieAsync(procProcedureQuery);
                
                if (procProcedure == null)
                {
                    return new PagedInfo<QualEnvOrderDto>(qualEnvOrderDtos, qualEnvOrderPagedQueryDto.PageIndex, qualEnvOrderPagedQueryDto.PageSize, 0);
                }
                qualEnvOrderPagedQuery.ProcedureId = procProcedure.Id;
            }
            var pagedInfo = await _qualEnvOrderRepository.GetPagedInfoAsync(qualEnvOrderPagedQuery);



            if (pagedInfo != null && pagedInfo.Data != null && pagedInfo.Data.Any())
            {
                var workCenters = await _inteWorkCenterRepository.GetByIdsAsync(pagedInfo.Data.Select(it => it.WorkCenterId).ToArray());
                var procedures = await _procProcedureRepository.GetByIdsAsync(pagedInfo.Data.Select(it => it.ProcedureId).ToArray());
                foreach (var item in pagedInfo.Data)
                {
                    var workCenter = workCenters.Where(it => it.Id == item.WorkCenterId).FirstOrDefault();
                    var procedure = procedures.Where(it => it.Id == item.ProcedureId).FirstOrDefault();
                    var dot = new QualEnvOrderDto()
                    {
                        Id = item.Id,
                        InspectionOrder = item.InspectionOrder,
                        GroupSnapshootId = item.GroupSnapshootId,
                        WorkCenterId = item.WorkCenterId,
                        WorkCenterCode = workCenter?.Code,
                        WorkCenterName = workCenter?.Name,
                        ProcedureId = item.ProcedureId,
                        ProcedureCode = procedure?.Code,
                        ProcedureName = procedure?.Name,
                        Remark = item.Remark,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,
                        UpdatedBy = item?.UpdatedBy,
                        UpdatedOn = item?.UpdatedOn,
                    };
                    qualEnvOrderDtos.Add(dot);
                }
            }
            return new PagedInfo<QualEnvOrderDto>(qualEnvOrderDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<QualEnvOrderDto> PrepareQualEnvOrderDtos(PagedInfo<QualEnvOrderEntity> pagedInfo)
        {
            var qualEnvOrderDtos = new List<QualEnvOrderDto>();
            foreach (var qualEnvOrderEntity in pagedInfo.Data)
            {
                var qualEnvOrderDto = qualEnvOrderEntity.ToModel<QualEnvOrderDto>();
                qualEnvOrderDtos.Add(qualEnvOrderDto);
            }

            return qualEnvOrderDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="qualEnvOrderModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyQualEnvOrderAsync(QualEnvOrderModifyDto qualEnvOrderModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(qualEnvOrderModifyDto);

            //DTO转换实体
            var qualEnvOrderEntity = qualEnvOrderModifyDto.ToEntity<QualEnvOrderEntity>();
            qualEnvOrderEntity.UpdatedBy = _currentUser.UserName;
            qualEnvOrderEntity.UpdatedOn = HymsonClock.Now();

            await _qualEnvOrderRepository.UpdateAsync(qualEnvOrderEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualEnvOrderDto> QueryQualEnvOrderByIdAsync(long id)
        {
            var qualEnvOrderEntity = await _qualEnvOrderRepository.GetByIdAsync(id);
            if (qualEnvOrderEntity != null)
            {
                return qualEnvOrderEntity.ToModel<QualEnvOrderDto>();
            }
            return null;
        }

        /// <summary>
        /// 根据ID查询关联信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualEnvOrderDto> QueryQualEnvOrderRelatesInfoByIdAsync(long id)
        {
            var qualEnvOrderEntity = await _qualEnvOrderRepository.GetByIdAsync(id);

            if (qualEnvOrderEntity != null)
            {
                var dto = qualEnvOrderEntity.ToModel<QualEnvOrderDto>();
                var workCenter = await _inteWorkCenterRepository.GetByIdAsync(qualEnvOrderEntity.WorkCenterId);
                var procedure = await _procProcedureRepository.GetByIdAsync(qualEnvOrderEntity.ProcedureId);
                dto.WorkCenterCode = workCenter.Code;
                dto.WorkCenterName = workCenter.Name;
                dto.ProcedureCode = procedure.Code;
                dto.ProcedureName = procedure.Name;

                return dto;
            }
            return null;
        }
    }
}
