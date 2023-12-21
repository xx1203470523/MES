using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务（配方操作组） 
    /// </summary>
    public class ProcFormulaOperationGroupService : IProcFormulaOperationGroupService
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
        private readonly AbstractValidator<ProcFormulaOperationGroupSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（配方操作组）
        /// </summary>
        private readonly IProcFormulaOperationGroupRepository _procFormulaOperationGroupRepository;

        /// <summary>
        /// 仓储接口（配方操作组）
        /// </summary>
        private readonly IProcFormulaOperationRepository _procFormulaOperationRepository;

        /// <summary>
        /// 仓储接口（配方操作组关系）
        /// </summary>
        private readonly IProcFormulaOperationGroupRelatiionRepository _procFormulaOperationGroupRelatiionRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="procFormulaOperationGroupRepository"></param>
        /// <param name="procFormulaOperationRepository"></param>
        /// <param name="procFormulaOperationGroupRelatiionRepository"></param>
        public ProcFormulaOperationGroupService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ProcFormulaOperationGroupSaveDto> validationSaveRules, 
            IProcFormulaOperationGroupRepository procFormulaOperationGroupRepository,
            IProcFormulaOperationRepository procFormulaOperationRepository,
            IProcFormulaOperationGroupRelatiionRepository procFormulaOperationGroupRelatiionRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _procFormulaOperationGroupRepository = procFormulaOperationGroupRepository;
            _procFormulaOperationRepository = procFormulaOperationRepository;
            _procFormulaOperationGroupRelatiionRepository = procFormulaOperationGroupRelatiionRepository;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task CreateProcFormulaOperationGroupAsync(AddFormulaOperationGroupDto addDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(addDto.FormulaOperationGroup);

            addDto.FormulaOperationGroup.Code = addDto.FormulaOperationGroup.Code.Trim();
            addDto.FormulaOperationGroup.Name = addDto.FormulaOperationGroup.Name.Trim();

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            //验证是否编码唯一
            var entity = await _procFormulaOperationGroupRepository.GetByCodeAsync(addDto.FormulaOperationGroup.Code);
            if (entity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11307));
            }

            //DTO转换实体
            var operationGroupEntity = addDto.FormulaOperationGroup.ToEntity<ProcFormulaOperationGroupEntity>();
            operationGroupEntity.Id = IdGenProvider.Instance.CreateId();
            operationGroupEntity.CreatedBy = updatedBy;
            operationGroupEntity.UpdatedBy = updatedBy;
            operationGroupEntity.UpdatedOn = updatedOn;
            operationGroupEntity.CreatedOn = updatedOn;
            operationGroupEntity.SiteId = _currentSite.SiteId ?? 0;

            //配方操作
            List<ProcFormulaOperationGroupRelatiionEntity> operationList = new();
            if (addDto.FormulaOperationDtos != null && addDto.FormulaOperationDtos.Any())
            {
                foreach (var item in addDto.FormulaOperationDtos)
                {
                    operationList.Add(new ProcFormulaOperationGroupRelatiionEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = updatedBy,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        CreatedOn = updatedOn,

                        FormulaOperationId = item.Id,
                        FormulaOperationGroupId = operationGroupEntity.Id,
                        IsDeleted = 0
                    });
                }
            }

            //入库
            using TransactionScope ts = TransactionHelper.GetTransactionScope();
            await _procFormulaOperationGroupRepository.InsertAsync(operationGroupEntity);
            if (addDto.FormulaOperationDtos != null && addDto.FormulaOperationDtos.Any())
            {
                await _procFormulaOperationGroupRelatiionRepository.InsertRangeAsync(operationList);
            }
            ts.Complete();

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task ModifyProcFormulaOperationGroupAsync(AddFormulaOperationGroupDto addDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            //验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(addDto.FormulaOperationGroup);

            addDto.FormulaOperationGroup.Code = addDto.FormulaOperationGroup.Code.Trim();
            addDto.FormulaOperationGroup.Name = addDto.FormulaOperationGroup.Name.Trim();

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();

            //配方操作
            List<ProcFormulaOperationGroupRelatiionEntity> operationList = new();
            if (addDto.FormulaOperationDtos != null && addDto.FormulaOperationDtos.Any())
            {
                foreach (var item in addDto.FormulaOperationDtos)
                {
                    operationList.Add(new ProcFormulaOperationGroupRelatiionEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = updatedBy,
                        CreatedOn = updatedOn,

                        FormulaOperationId = item.Id,
                        FormulaOperationGroupId = addDto.FormulaOperationGroup.Id,
                        IsDeleted = 0
                    });
                }
            }

            //DTO转换实体
            var operationGroupEntity = addDto.FormulaOperationGroup.ToEntity<ProcFormulaOperationGroupEntity>();
            operationGroupEntity.UpdatedBy = updatedBy;
            operationGroupEntity.UpdatedOn = updatedOn;
            //operationGroupEntity.CreatedBy = addDto.FormulaOperationGroup.CreatedBy;
            //operationGroupEntity.CreatedOn = addDto.FormulaOperationGroup.CreatedOn;
            operationGroupEntity.SiteId = _currentSite.SiteId ?? 0;


            //入库
            using TransactionScope ts = TransactionHelper.GetTransactionScope();
            var command = new DeleteCommand()
            {
                DeleteOn = HymsonClock.Now(),
                UserId = _currentUser.UserName
            };
            long[] ids = {};
            Array.Resize(ref ids, ids.Length + 1);
            ids[^1] = operationGroupEntity.Id;
            command.Ids = ids;
            await _procFormulaOperationGroupRelatiionRepository.DeletesByGroupIdsAsync(command);
            await _procFormulaOperationGroupRepository.UpdateAsync(operationGroupEntity);
            if (addDto.FormulaOperationDtos != null && addDto.FormulaOperationDtos.Any())
            {
                await _procFormulaOperationGroupRelatiionRepository.InsertRangeAsync(operationList);
            }
            ts.Complete();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcFormulaOperationGroupAsync(long id)
        {
            return await _procFormulaOperationGroupRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcFormulaOperationGroupAsync(long[] ids)
        {
            if (ids.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            int rows = 0;
            using (TransactionScope ts = TransactionHelper.GetTransactionScope())
            {
                var command = new DeleteCommand()
                {
                    Ids = ids,
                    DeleteOn = HymsonClock.Now(),
                    UserId = _currentUser.UserName
                };
                rows += await _procFormulaOperationGroupRepository.DeletesAsync(command);
                rows += await _procFormulaOperationGroupRelatiionRepository.DeletesByGroupIdsAsync(command);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcFormulaOperationGroupDto?> QueryProcFormulaOperationGroupByIdAsync(long id) 
        {
           var procFormulaOperationGroupEntity = await _procFormulaOperationGroupRepository.GetByIdAsync(id);
           if (procFormulaOperationGroupEntity == null) return null;
           
           return procFormulaOperationGroupEntity.ToModel<ProcFormulaOperationGroupDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcFormulaOperationGroupDto>> GetPagedListAsync(ProcFormulaOperationGroupPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ProcFormulaOperationGroupPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procFormulaOperationGroupRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ProcFormulaOperationGroupDto>());
            return new PagedInfo<ProcFormulaOperationGroupDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取配方操作
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcFormulaOperationDto>> GetFormulaOperationListAsync(OperationGroupGetOperationPagedQueryDto pagedQueryDto)
        {
            var operationPageIndex = new GetByIdsPagedQuery()
            {
                PageSize = pagedQueryDto.PageSize,
                PageIndex = pagedQueryDto.PageIndex
            };
            if(pagedQueryDto.Id != null)
            {
                var entities = await _procFormulaOperationGroupRelatiionRepository.
                    GetOperationIdsByGroupIdAsync(pagedQueryDto.Id ?? 0);
                var aaa = entities.Select(x => x.FormulaOperationId).ToArray();
                operationPageIndex.Ids =aaa;
            }

            var dtos = new List<ProcFormulaOperationDto>();
            int totalCount = 0;
            if (operationPageIndex.Ids != null && operationPageIndex.Ids.Any())
            {
                var info = await _procFormulaOperationRepository.GetPagedInfoByIdsAsync(operationPageIndex);
                foreach(var item in info.Data)
                {
                    dtos.Add(item.ToModel<ProcFormulaOperationDto>());
                }
                totalCount=info.TotalCount;
            }
            return new PagedInfo<ProcFormulaOperationDto>(dtos, pagedQueryDto.PageIndex, pagedQueryDto.PageSize, totalCount);
        }
    }
}
