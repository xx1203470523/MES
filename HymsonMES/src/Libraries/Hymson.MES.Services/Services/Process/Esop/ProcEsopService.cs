/*
 *creator: Karl
 *
 *describe: ESOP    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-11-02 02:39:53
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Quality.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Dtos.Quality;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using IdGen;
using System.Collections.Generic;
using System.Transactions;
using System.Xml.Linq;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// ESOP 服务
    /// </summary>
    public class ProcEsopService : IProcEsopService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// ESOP 仓储
        /// </summary>
        private readonly IProcEsopRepository _procEsopRepository;
        private readonly IProcEsopFileRepository _esopFileRepository;
        //文件信息
        private readonly IInteAttachmentRepository _inteAttachmentRepository;

        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcProcedureRepository _procedureRepository;

        private readonly AbstractValidator<ProcEsopCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcEsopModifyDto> _validationModifyRules;

        public ProcEsopService(ICurrentUser currentUser, ICurrentSite currentSite,
            IProcEsopRepository procEsopRepository,
            IProcEsopFileRepository esopFileRepository,
            IInteAttachmentRepository inteAttachmentRepository,
            IProcMaterialRepository procMaterialRepository,
            IProcProcedureRepository procedureRepository,
            AbstractValidator<ProcEsopCreateDto> validationCreateRules,
            AbstractValidator<ProcEsopModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procEsopRepository = procEsopRepository;
            _esopFileRepository = esopFileRepository;
            _inteAttachmentRepository = inteAttachmentRepository;
            _procMaterialRepository = procMaterialRepository;
            _procedureRepository = procedureRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procEsopCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcEsopAsync(ProcEsopCreateDto procEsopCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procEsopCreateDto);

            //判断同一物料同一工序只能有一个
            var procEsops = await _procEsopRepository.GetProcEsopEntitiesAsync(new ProcEsopQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = procEsopCreateDto.ProcedureId,
                MaterialId = procEsopCreateDto.MaterialId
            });
            if (procEsops != null && procEsops.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11500));
            }

            //DTO转换实体
            var procEsopEntity = new ProcEsopEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                MaterialId = procEsopCreateDto.MaterialId,
                ProcedureId = procEsopCreateDto.ProcedureId,
                Status = procEsopCreateDto.Status,
                CreatedBy = _currentUser.UserName,
                UpdatedBy = _currentUser.UserName,
                CreatedOn = HymsonClock.Now(),
                UpdatedOn = HymsonClock.Now(),
                SiteId = _currentSite.SiteId ?? 0
            };

            //入库
            await _procEsopRepository.InsertAsync(procEsopEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcEsopAsync(long id)
        {
            await _procEsopRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcEsopAsync(long[] ids)
        {
            return await _procEsopRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procEsopPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcEsopDto>> GetPagedListAsync(ProcEsopPagedQueryDto procEsopPagedQueryDto)
        {
            var procEsopPagedQuery = procEsopPagedQueryDto.ToQuery<ProcEsopPagedQuery>();
            procEsopPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procEsopRepository.GetPagedInfoAsync(procEsopPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcEsopDto> procEsopDtos = PrepareProcEsopDtos(pagedInfo);
            return new PagedInfo<ProcEsopDto>(procEsopDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcEsopDto> PrepareProcEsopDtos(PagedInfo<ProcEsopView> pagedInfo)
        {
            var procEsopDtos = new List<ProcEsopDto>();
            foreach (var procEsopEntity in pagedInfo.Data)
            {
                var procEsopDto = procEsopEntity.ToModel<ProcEsopDto>();
                procEsopDtos.Add(procEsopDto);
            }

            return procEsopDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procEsopModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcEsopAsync(ProcEsopModifyDto procEsopModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procEsopModifyDto);

            //判断同一物料同一工序只能有一个
            var procEsops = await _procEsopRepository.GetProcEsopEntitiesAsync(new ProcEsopQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ProcedureId = procEsopModifyDto.ProcedureId,
                MaterialId = procEsopModifyDto.MaterialId
            });
            procEsops = procEsops.Where(x => x.Id != procEsopModifyDto.Id);
            if (procEsops != null && procEsops.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11500));
            }

            //DTO转换实体
            var procEsopEntity = new ProcEsopEntity()
            {
                Id = procEsopModifyDto.Id,
                MaterialId = procEsopModifyDto.MaterialId,
                ProcedureId = procEsopModifyDto.ProcedureId,
                Status = procEsopModifyDto.Status,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };
            await _procEsopRepository.UpdateAsync(procEsopEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcEsopDto> QueryProcEsopByIdAsync(long id)
        {
            var procEsopEntity = await _procEsopRepository.GetByIdAsync(id);

            var procEsopDto = new ProcEsopDto
            {
                Id = procEsopEntity.Id,
                MaterialId = procEsopEntity.MaterialId,
                ProcedureId = procEsopEntity.ProcedureId,
                Status = procEsopEntity.Status,
            };
            if (procEsopEntity != null)
            {
                //关联物料
                var material = await _procMaterialRepository.GetByIdAsync(procEsopEntity.MaterialId.GetValueOrDefault());
                if (material != null)
                {
                    procEsopDto.MaterialCode = material.MaterialCode;
                    procEsopDto.Version = material.Version ?? "";
                    procEsopDto.MaterialName = material.MaterialName;
                }

                //关联工序
                var procedure = await _procedureRepository.GetByIdAsync(procEsopEntity.ProcedureId.GetValueOrDefault());
                if (material != null)
                {
                    procEsopDto.ProcedureCode = procedure.Code;
                    procEsopDto.ProcedureName = procedure.Name;
                }
                return procEsopDto;
            }

            return new ProcEsopDto();
        }

        /// <summary>
        /// 根据Esop ID获取esop附件列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteAttachmentDto>?> GetAttachmentListAsync(long id)
        {
            var entities = await _esopFileRepository.GetProcEsopFileEntitiesAsync(new ProcEsopFileQuery { EsopId = id });
            if (entities == null)
            {
                return null;
            }

            var attachments = await _inteAttachmentRepository.GetByIdsAsync(entities.Select(x => x.AttachmentId));
            var inteAttachments = entities.Select(item =>
            {
                var dto = new InteAttachmentDto();
                var attachment = attachments.FirstOrDefault(x => x.Id == item.AttachmentId);
                if (attachment != null)
                {
                    dto.Id = item.Id;
                    dto.Name = attachment.Name;
                    dto.Path = attachment.Path;
                    dto.CreatedOn = attachment.CreatedOn;
                }
                return dto;
            });
            return inteAttachments;
        }

        /// <summary>
        /// 附件上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<int> AttachmentAddAsync(AttachmentAddDto dto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new CustomerDataException(nameof(ErrorCode.MES10101));
            }

            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            List<InteAttachmentEntity> attachments = new();
            List<ProcEsopFileEntity> annexs = new();
            foreach (var item in dto.Attachments)
            {
                var attachment = new InteAttachmentEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    Name = item.Name,
                    Path = item.Path,
                    CreatedBy = updatedBy,
                    UpdatedBy = updatedBy
                };
                attachments.Add(attachment);

                var annex = new ProcEsopFileEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 0,
                    EsopId = dto.Id,
                    AttachmentId = attachment.Id,
                    CreatedBy = updatedBy,
                    UpdatedBy = updatedBy
                };
                annexs.Add(annex);
            }

            int rows = 0;
            using (var trans = TransactionHelper.GetTransactionScope())
            {
                rows += await _inteAttachmentRepository.InsertRangeAsync(attachments);
                rows += await _esopFileRepository.InsertsAsync(annexs);
                trans.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 附件删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> AttachmentDeleteAsync(long[] ids)
        {
            return await _inteAttachmentRepository.DeletesAsync(new DeleteCommand
            {
                Ids = ids,
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            });
        }
    }
}
