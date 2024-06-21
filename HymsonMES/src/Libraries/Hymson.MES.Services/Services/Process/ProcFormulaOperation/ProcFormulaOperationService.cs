using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Text;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 服务（配方操作） 
    /// </summary>
    public class ProcFormulaOperationService : IProcFormulaOperationService
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
        private readonly AbstractValidator<ProcFormulaOperationSaveDto> _validationSaveRules;

        /// <summary>
        /// 仓储接口（配方操作）
        /// </summary>
        private readonly IProcFormulaOperationRepository _procFormulaOperationRepository;

        /// <summary>
        /// 仓储接口（配方操作设置值）
        /// </summary>
        private readonly IProcFormulaOperationSetRepository _procFormulaOperationSetRepository;

        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationSaveRules"></param>
        /// <param name="procFormulaOperationRepository"></param>
        /// <param name="procFormulaOperationSetRepository"></param>
        /// <param name="localizationService"></param>
        public ProcFormulaOperationService(ICurrentUser currentUser, ICurrentSite currentSite, AbstractValidator<ProcFormulaOperationSaveDto> validationSaveRules, 
            IProcFormulaOperationRepository procFormulaOperationRepository,
            IProcFormulaOperationSetRepository procFormulaOperationSetRepository,
             ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationSaveRules = validationSaveRules;
            _procFormulaOperationRepository = procFormulaOperationRepository;
            _procFormulaOperationSetRepository = procFormulaOperationSetRepository;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task CreateProcFormulaOperationAsync(AddFormulaOperationDto addDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(addDto.FormulaOperation);

            addDto.FormulaOperation.Code = addDto.FormulaOperation.Code.Trim();
            addDto.FormulaOperation.Name = addDto.FormulaOperation.Name.Trim();

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            //验证是否编码和版本唯一
            var entity = await _procFormulaOperationRepository.GetByCodeAndVersionAsync(new EntityByCodeQuery
            {
                Code = addDto.FormulaOperation.Code,
                Version= addDto.FormulaOperation.Version,
                Site = _currentSite.SiteId ?? 0
            });
            if (entity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11307));
            }

            //DTO转换实体
            var operationEntity = addDto.FormulaOperation.ToEntity<ProcFormulaOperationEntity>();
            operationEntity.Id = IdGenProvider.Instance.CreateId();
            operationEntity.Status = SysDataStatusEnum.Build;
            operationEntity.CreatedBy = updatedBy;
            operationEntity.UpdatedBy = updatedBy;
            operationEntity.UpdatedOn = updatedOn;
            operationEntity.CreatedOn = updatedOn;
            operationEntity.SiteId = _currentSite.SiteId ?? 0;

            //设置值类型
            List<ProcFormulaOperationSetEntity> setList = new();
            if (addDto.FormulaOperation.Type == FormulaOperationTypeEnum.SetValue && addDto.FormulaOperationSetDtos != null && addDto.FormulaOperationSetDtos.Any())
            {
                if (addDto.FormulaOperationSetDtos.Any(x => string.IsNullOrWhiteSpace(x.Code))) throw new CustomerValidationException("设置编码不能为空");
                if (addDto.FormulaOperationSetDtos.Any(x => string.IsNullOrWhiteSpace(x.Name))) throw new CustomerValidationException("设置名称不能为空");
                if (addDto.FormulaOperationSetDtos.Any(x => x.Value<0 || x.Value % 1 != 0)) throw new CustomerValidationException("设置值不能为空");

                int i = 0;
                foreach (var item in addDto.FormulaOperationSetDtos)
                {
                    i++;
                    setList.Add(new ProcFormulaOperationSetEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = updatedBy,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        CreatedOn = updatedOn,

                        Serial=i,
                        FormulaOperationId= operationEntity.Id,
                        Code=item.Code,
                        Name=item.Name,
                        Value=item.Value,
                        IsDeleted= 0
                    });
                }
            }

            #region 验证设置编码是否重复

            if (addDto.FormulaOperationSetDtos != null)
            {
                var duplicateCode = addDto.FormulaOperationSetDtos.GroupBy(m => m.Code).Where(m => m.Count() > 1);
                if (duplicateCode.Any())
                {
                    var exCode = new StringBuilder();
                    foreach (var group in duplicateCode)
                    {
                        exCode.Append(group.Key);
                        exCode.Append(',');
                    }

                    throw new CustomerValidationException(nameof(ErrorCode.MES15728)).WithData("code", exCode.ToString());
                }
            }

            #endregion

            //入库
            using TransactionScope ts = TransactionHelper.GetTransactionScope();
            await _procFormulaOperationRepository.InsertAsync(operationEntity);
            if (addDto.FormulaOperation.Type == FormulaOperationTypeEnum.SetValue 
                    && addDto.FormulaOperationSetDtos != null && addDto.FormulaOperationSetDtos.Any())
            {
                await _procFormulaOperationSetRepository.InsertRangeAsync(setList);
            }
            ts.Complete();

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="addDto"></param>
        /// <returns></returns>
        public async Task ModifyProcFormulaOperationAsync(AddFormulaOperationDto addDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0) throw new ValidationException(nameof(ErrorCode.MES10101));

            //验证DTO
            await _validationSaveRules.ValidateAndThrowAsync(addDto.FormulaOperation);

            addDto.FormulaOperation.Code = addDto.FormulaOperation.Code.Trim();
            addDto.FormulaOperation.Name = addDto.FormulaOperation.Name.Trim();

            // 更新时间
            var updatedBy = _currentUser.UserName;
            var updatedOn = HymsonClock.Now();
            //验证是否编码和版本唯一
            var entity = await _procFormulaOperationRepository.GetByCodeAndVersionAsync(new EntityByCodeQuery
            {
                Code = addDto.FormulaOperation.Code,
                Version = addDto.FormulaOperation.Version,
                Site = _currentSite.SiteId ?? 0
            });
            if (entity != null && entity.Id != addDto.FormulaOperation.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES11307));
            }

            var oldEntity = await _procFormulaOperationRepository.GetByIdAsync(addDto.FormulaOperation.Id) ?? throw new CustomerValidationException("配方操作不存在");
            //验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == oldEntity.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            //设置值类型
            List<ProcFormulaOperationSetEntity> setList = new();
            if (addDto.FormulaOperation.Type == FormulaOperationTypeEnum.SetValue && addDto.FormulaOperationSetDtos != null && addDto.FormulaOperationSetDtos.Any())
            {
                if (addDto.FormulaOperationSetDtos.Any(x => string.IsNullOrWhiteSpace(x.Code))) throw new CustomerValidationException("设置编码不能为空");
                if (addDto.FormulaOperationSetDtos.Any(x => string.IsNullOrWhiteSpace(x.Name))) throw new CustomerValidationException("设置名称不能为空");
                if (addDto.FormulaOperationSetDtos.Any(x => x.Value < 0 || x.Value % 1 != 0)) throw new CustomerValidationException("设置值不能为空");

                int i = 0;
                foreach (var item in addDto.FormulaOperationSetDtos)
                {
                    i++;
                    setList.Add(new ProcFormulaOperationSetEntity
                    {
                        Id = IdGenProvider.Instance.CreateId(),
                        SiteId = _currentSite.SiteId ?? 0,
                        CreatedBy = updatedBy,
                        UpdatedBy = updatedBy,
                        UpdatedOn = updatedOn,
                        CreatedOn = updatedOn,

                        Serial = i,
                        FormulaOperationId = oldEntity.Id,
                        Code = item.Code,
                        Name = item.Name,
                        Value = item.Value,
                        IsDeleted = 0
                    });
                }
            }

            #region 验证设置编码是否重复

            if (addDto.FormulaOperationSetDtos != null)
            {
                var duplicateCode = addDto.FormulaOperationSetDtos.GroupBy(m => m.Code).Where(m => m.Count() > 1);
                if (duplicateCode.Any())
                {
                    var exCode = new StringBuilder();
                    foreach (var group in duplicateCode)
                    {
                        exCode.Append(group.Key);
                        exCode.Append(',');
                    }

                    throw new CustomerValidationException(nameof(ErrorCode.MES15728)).WithData("code", exCode.ToString());
                }
            }

            #endregion

            //DTO转换实体
            var operationEntity = addDto.FormulaOperation.ToEntity<ProcFormulaOperationEntity>();
            operationEntity.Status = oldEntity.Status;
            operationEntity.CreatedBy = oldEntity.CreatedBy;
            operationEntity.UpdatedBy = updatedBy;
            operationEntity.UpdatedOn = updatedOn;
            operationEntity.CreatedOn = oldEntity.CreatedOn;
            operationEntity.SiteId = _currentSite.SiteId ?? 0;


            //入库
            using TransactionScope ts = TransactionHelper.GetTransactionScope();
            await _procFormulaOperationSetRepository.DeleteByFormulaOperationIdAsync(operationEntity.Id);
            await _procFormulaOperationRepository.UpdateAsync(operationEntity);
            if (addDto.FormulaOperation.Type == FormulaOperationTypeEnum.SetValue
                    && addDto.FormulaOperationSetDtos != null && addDto.FormulaOperationSetDtos.Any())
            {
                await _procFormulaOperationSetRepository.InsertRangeAsync(setList);
            }
            ts.Complete();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcFormulaOperationAsync(long id)
        {
            return await _procFormulaOperationRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcFormulaOperationAsync(long[] ids)
        {
            if (ids.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }
            var entitys = await _procFormulaOperationRepository.GetByIdsAsync(ids);
            if (entitys != null && entitys.Any(a => a.Status != SysDataStatusEnum.Build))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10106));
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
                rows += await _procFormulaOperationRepository.DeletesAsync(command);
                rows += await _procFormulaOperationSetRepository.DeleteByFormulaOperationIdsAsync(command);
                ts.Complete();
            }
            return rows;
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcFormulaOperationDto?> QueryProcFormulaOperationByIdAsync(long id)
        {
            var procFormulaOperationEntity = await _procFormulaOperationRepository.GetByIdAsync(id);
            if (procFormulaOperationEntity == null) return null;

            return procFormulaOperationEntity.ToModel<ProcFormulaOperationDto>();
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcFormulaOperationDto>> GetPagedListAsync(ProcFormulaOperationPagedQueryDto pagedQueryDto)
        {
            var pagedQuery = pagedQueryDto.ToQuery<ProcFormulaOperationPagedQuery>();
            pagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procFormulaOperationRepository.GetPagedInfoAsync(pagedQuery);

            // 实体到DTO转换 装载数据
            var dtos = pagedInfo.Data.Select(s => s.ToModel<ProcFormulaOperationDto>());
            return new PagedInfo<ProcFormulaOperationDto>(dtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取配方操作设置值
        /// </summary>
        /// <param name="pagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcFormulaOperationSetDto>> GetFormulaOperationConfigSetListAsync(ProcFormulaOperationSetPagedQueryDto pagedQueryDto)
        {
            var setPageIndex = new ProcFormulaOperationSetPagedQuery()
            {
                FormulaOperationId = pagedQueryDto.FormulaOperationId,
                PageSize = pagedQueryDto.PageSize,
                PageIndex = pagedQueryDto.PageIndex,
                Code = pagedQueryDto.Code,
                Name = pagedQueryDto.Name
            };

            var setDtos = await _procFormulaOperationSetRepository.GetPagedInfoAsync(setPageIndex);

            // 实体到DTO转换 装载数据
            var dtos = setDtos.Data.Select(s => s.ToModel<ProcFormulaOperationSetDto>());
            return new PagedInfo<ProcFormulaOperationSetDto>(dtos, setDtos.PageIndex, setDtos.PageSize, setDtos.TotalCount);
        }

        #region 状态变更
        /// <summary>
        /// 状态变更
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(ChangeStatusDto param)
        {
            #region 参数校验
            if (param.Id == 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10125));
            }
            if (!Enum.IsDefined(typeof(SysDataStatusEnum), param.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10126));
            }
            if (param.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10128));
            }

            #endregion

            var changeStatusCommand = new ChangeStatusCommand()
            {
                Id = param.Id,
                Status = param.Status,
                UpdatedBy = _currentUser.UserName,
                UpdatedOn = HymsonClock.Now()
            };

            #region 校验数据
            var entity = await _procFormulaOperationRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18701));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).
                    WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }

            #endregion

            #region 操作数据库
            await _procFormulaOperationRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion
    }
}
