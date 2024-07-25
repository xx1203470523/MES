/*
 *creator: Karl
 *
 *describe: 马威FQC检验    服务 | 代码由框架生成
 *builder:  pengxin
 *build datetime: 2024-07-24 03:09:40
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.QualFqcInspectionMavalAttachment;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.QualFqcInspectionMaval;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.QualFqcInspectionMavalAttachment;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.QualFqcInspectionMaval;
using Hymson.MES.Services.Dtos.QualFqcInspectionMaval;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Mysqlx.Crud;

namespace Hymson.MES.Services.Services.QualFqcInspectionMaval
{
    /// <summary>
    /// 马威FQC检验 服务
    /// </summary>
    public class QualFqcInspectionMavalService : IQualFqcInspectionMavalService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 马威FQC检验 仓储
        /// </summary>
        private readonly IQualFqcInspectionMavalRepository _qualFqcInspectionMavalRepository;
        private readonly IProcResourceRepository _procResourceRepository;
        private readonly IProcProcedureRepository _procProcedureRepository;
        private readonly IQualFqcInspectionMavalAttachmentRepository _qualFqcInspectionMavalAttachmentRepository;
        private readonly IInteAttachmentRepository _inteAttachmentRepository;
        private readonly AbstractValidator<QualFqcInspectionMavalCreateDto> _validationCreateRules;
        private readonly AbstractValidator<QualFqcInspectionMavalModifyDto> _validationModifyRules;

        public QualFqcInspectionMavalService(ICurrentUser currentUser, ICurrentSite currentSite, IQualFqcInspectionMavalRepository qualFqcInspectionMavalRepository, AbstractValidator<QualFqcInspectionMavalCreateDto> validationCreateRules, AbstractValidator<QualFqcInspectionMavalModifyDto> validationModifyRules, IProcResourceRepository procResourceRepository, IProcProcedureRepository procProcedureRepository, IQualFqcInspectionMavalAttachmentRepository qualFqcInspectionMavalAttachmentRepository, IInteAttachmentRepository inteAttachmentRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _qualFqcInspectionMavalRepository = qualFqcInspectionMavalRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procResourceRepository = procResourceRepository;
            _procProcedureRepository = procProcedureRepository;
            _qualFqcInspectionMavalAttachmentRepository = qualFqcInspectionMavalAttachmentRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="qualFqcInspectionMavalCreateDto"></param>
        /// <returns></returns>
        public async Task CreateQualFqcInspectionMavalAsync(QualFqcInspectionMavalCreateDto qualFqcInspectionMavalCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(qualFqcInspectionMavalCreateDto);

            var siteId = _currentSite.SiteId ?? 0;

            var procProcedure = await _procProcedureRepository.GetByCodeAsync(new EntityByCodeQuery { Code = qualFqcInspectionMavalCreateDto.ProcedureCode, Site = siteId })
             ?? throw new CustomerValidationException(nameof(ErrorCode.MES11750)).WithData("Code", qualFqcInspectionMavalCreateDto.ProcedureCode);
            var procResource = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery { Code = qualFqcInspectionMavalCreateDto.ResourceCode, Site = siteId })
             ?? throw new CustomerValidationException(nameof(ErrorCode.MES11751)).WithData("Code", qualFqcInspectionMavalCreateDto.ProcedureCode);

            var qualFqcInspectionMaval = await _qualFqcInspectionMavalRepository.GetBySFCAsync(new QualFqcInspectionMavalQuery { SFC = qualFqcInspectionMavalCreateDto.SFC, SiteId = siteId });
            if (qualFqcInspectionMaval != null)
            {
                qualFqcInspectionMaval.JudgmentResults = qualFqcInspectionMavalCreateDto.JudgmentResults;
                qualFqcInspectionMaval.ProcedureId = procProcedure.Id;
                qualFqcInspectionMaval.ResourceId = procResource.Id;
                qualFqcInspectionMaval.UpdatedBy = _currentUser.UserName;
                qualFqcInspectionMaval.UpdatedOn = HymsonClock.Now();
                await _qualFqcInspectionMavalRepository.UpdateAsync(qualFqcInspectionMaval);
                //var row = await _qualFqcInspectionMavalRepository.DeleteAsync(qualFqcInspectionMaval.Id);
                //if (row <= 0)
                //{
                //    throw new CustomerValidationException(nameof(ErrorCode.MES11752)).WithData("SFC", qualFqcInspectionMavalCreateDto.SFC);
                //}
            }
            else
            {
                //DTO转换实体
                var qualFqcInspectionMavalEntity = qualFqcInspectionMavalCreateDto.ToEntity<QualFqcInspectionMavalEntity>();

                qualFqcInspectionMavalEntity.SFC = qualFqcInspectionMavalCreateDto.SFC;
                qualFqcInspectionMavalEntity.ProcedureId = procProcedure.Id;
                qualFqcInspectionMavalEntity.ResourceId = procResource.Id;
                qualFqcInspectionMavalEntity.Qty = qualFqcInspectionMavalCreateDto.Qty;
                qualFqcInspectionMavalEntity.JudgmentResults = qualFqcInspectionMavalCreateDto.JudgmentResults;

                qualFqcInspectionMavalEntity.Id = qualFqcInspectionMavalCreateDto.Id != 0 ? qualFqcInspectionMavalCreateDto.Id : IdGenProvider.Instance.CreateId();
                qualFqcInspectionMavalEntity.CreatedBy = _currentUser.UserName;
                qualFqcInspectionMavalEntity.UpdatedBy = _currentUser.UserName;
                qualFqcInspectionMavalEntity.CreatedOn = HymsonClock.Now();
                qualFqcInspectionMavalEntity.UpdatedOn = HymsonClock.Now();
                qualFqcInspectionMavalEntity.SiteId = _currentSite.SiteId ?? 0;

                //入库
                await _qualFqcInspectionMavalRepository.InsertAsync(qualFqcInspectionMavalEntity);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteQualFqcInspectionMavalAsync(long id)
        {
            await _qualFqcInspectionMavalRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesQualFqcInspectionMavalAsync(long[] ids)
        {
            return await _qualFqcInspectionMavalRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="qualFqcInspectionMavalPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualFqcInspectionMavalDto>> GetPagedListAsync(QualFqcInspectionMavalPagedQueryDto qualFqcInspectionMavalPagedQueryDto)
        {
            var qualFqcInspectionMavalPagedQuery = qualFqcInspectionMavalPagedQueryDto.ToQuery<QualFqcInspectionMavalPagedQuery>();
            qualFqcInspectionMavalPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _qualFqcInspectionMavalRepository.GetPagedInfoAsync(qualFqcInspectionMavalPagedQuery);

            //实体到DTO转换 装载数据
            List<QualFqcInspectionMavalDto> qualFqcInspectionMavalDtos = PrepareQualFqcInspectionMavalDtos(pagedInfo);
            return new PagedInfo<QualFqcInspectionMavalDto>(qualFqcInspectionMavalDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<QualFqcInspectionMavalDto> PrepareQualFqcInspectionMavalDtos(PagedInfo<QualFqcInspectionMavalEntity> pagedInfo)
        {
            var qualFqcInspectionMavalDtos = new List<QualFqcInspectionMavalDto>();
            foreach (var qualFqcInspectionMavalEntity in pagedInfo.Data)
            {
                var qualFqcInspectionMavalDto = qualFqcInspectionMavalEntity.ToModel<QualFqcInspectionMavalDto>();
                qualFqcInspectionMavalDtos.Add(qualFqcInspectionMavalDto);
            }

            return qualFqcInspectionMavalDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="qualFqcInspectionMavalDto"></param>
        /// <returns></returns>
        public async Task ModifyQualFqcInspectionMavalAsync(QualFqcInspectionMavalModifyDto qualFqcInspectionMavalModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(qualFqcInspectionMavalModifyDto);

            //DTO转换实体
            var qualFqcInspectionMavalEntity = qualFqcInspectionMavalModifyDto.ToEntity<QualFqcInspectionMavalEntity>();
            qualFqcInspectionMavalEntity.UpdatedBy = _currentUser.UserName;
            qualFqcInspectionMavalEntity.UpdatedOn = HymsonClock.Now();

            await _qualFqcInspectionMavalRepository.UpdateAsync(qualFqcInspectionMavalEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualFqcInspectionMavalDto> QueryQualFqcInspectionMavalByIdAsync(long id)
        {
            var qualFqcInspectionMavalEntity = await _qualFqcInspectionMavalRepository.GetByIdAsync(id);
            if (qualFqcInspectionMavalEntity != null)
            {
                return qualFqcInspectionMavalEntity.ToModel<QualFqcInspectionMavalDto>();
            }
            return null;
        }


        #region 附件

        /// <summary>
        /// 保存检验单附件
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<int> SaveAttachmentAsync(QualFqcInspectionMavalSaveAttachmentDto requestDto)
        {
            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //单据 
            //var entity = await _QualFqcInspectionMavalRepository.GetByIdAsync(requestDto.OrderId)
            //    ?? throw new CustomerValidationException(nameof(ErrorCode.MES17961));

            if (!requestDto.Attachments.Any()) return 0;


            List<InteAttachmentEntity> inteAttachmentEntities = new();
            List<QualFqcInspectionMavalAttachmentEntity> orderAttachmentEntities = new();
            foreach (var attachment in requestDto.Attachments)
            {
                var attachmentId = IdGenProvider.Instance.CreateId();
                inteAttachmentEntities.Add(new InteAttachmentEntity
                {
                    Id = attachmentId,
                    Name = attachment.Name,
                    Path = attachment.Path,
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn,
                    SiteId = _currentSite.SiteId ?? 0,
                });

                orderAttachmentEntities.Add(new QualFqcInspectionMavalAttachmentEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    FqcMavalId = requestDto.FqcMavalId,
                    AttachmentId = attachmentId,
                    Remark = "",
                    CreatedBy = updatedBy,
                    CreatedOn = updatedOn,
                    UpdatedBy = updatedBy,
                    UpdatedOn = updatedOn
                });
            }

            var rows = 0;
            try
            {
                using var trans = TransactionHelper.GetTransactionScope();
                rows += await _inteAttachmentRepository.InsertRangeAsync(inteAttachmentEntities);
                rows += await _qualFqcInspectionMavalAttachmentRepository.InsertsAsync(orderAttachmentEntities);
                trans.Complete();
            }
            catch (Exception ex) { }
            return rows;
        }

        /// <summary>
        /// 删除检验单附件
        /// </summary>
        /// <param name="orderAttachmentId"></param>
        /// <returns></returns>
        public async Task<int> DeleteAttachmentByIdAsync(long orderAttachmentId)
        {
            var attachmentEntity = await _qualFqcInspectionMavalAttachmentRepository.GetByIdAsync(orderAttachmentId);
            if (attachmentEntity == null) return default;

            var rows = 0;
            using var trans = TransactionHelper.GetTransactionScope();
            rows += await _inteAttachmentRepository.DeleteAsync(attachmentEntity.AttachmentId);
            rows += await _qualFqcInspectionMavalAttachmentRepository.DeleteAsync(attachmentEntity.Id);
            trans.Complete();
            return rows;
        }


        /// <summary>
        /// 查询单据附件
        /// </summary>
        /// <param name="fqcMavalId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteAttachmentBaseDto>> QueryOrderAttachmentListByIdAsync(QualFqcInspectionMavalAttachmentDto dto)
        {
            var orderAttachments = await _qualFqcInspectionMavalAttachmentRepository.GetByFqcMavalIdListAsync(dto.FqcMavalId);
            if (orderAttachments == null) return Array.Empty<InteAttachmentBaseDto>();

            var attachmentEntities = await _inteAttachmentRepository.GetByIdsAsync(orderAttachments.Select(s => s.AttachmentId));
            if (attachmentEntities == null) return Array.Empty<InteAttachmentBaseDto>();

            return PrepareAttachmentBaseDtos(orderAttachments, attachmentEntities);
        }
        /// <summary>
        /// 转换成附近DTO
        /// </summary>
        /// <param name="linkAttachments"></param>
        /// <param name="attachmentEntities"></param>
        /// <returns></returns>
        private static IEnumerable<InteAttachmentBaseDto> PrepareAttachmentBaseDtos(IEnumerable<dynamic> linkAttachments, IEnumerable<InteAttachmentEntity> attachmentEntities)
        {
            List<InteAttachmentBaseDto> dtos = new();
            foreach (var item in linkAttachments)
            {
                var dto = new InteAttachmentBaseDto
                {
                    Id = item.Id,
                    AttachmentId = item.AttachmentId
                };

                var attachmentEntity = attachmentEntities.FirstOrDefault(f => f.Id == item.AttachmentId);
                if (attachmentEntity == null)
                {
                    dto.Name = "附件不存在";
                    dto.Path = "";
                    dto.Url = "";
                    dtos.Add(dto);
                    continue;
                }

                dto.Id = item.Id;
                dto.Name = attachmentEntity.Name;
                dto.Path = attachmentEntity.Path;
                dto.Url = attachmentEntity.Path;
                dtos.Add(dto);
            }

            return dtos;
        }
        #endregion

        #region 帮助

        /// <summary>
        /// 获取ID
        /// </summary>
        /// <returns></returns>
        public long GetNewId()
        {
            return IdGenProvider.Instance.CreateId();
        }

        #endregion
    }
}
