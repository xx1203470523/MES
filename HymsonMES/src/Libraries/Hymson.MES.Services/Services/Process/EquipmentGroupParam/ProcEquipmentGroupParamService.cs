/*
 *creator: Karl
 *
 *describe: 设备参数组    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-08-02 01:48:35
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 设备参数组 服务
    /// </summary>
    public class ProcEquipmentGroupParamService : IProcEquipmentGroupParamService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 设备参数组 仓储
        /// </summary>
        private readonly IProcEquipmentGroupParamRepository _procEquipmentGroupParamRepository;
        private readonly AbstractValidator<ProcEquipmentGroupParamCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcEquipmentGroupParamModifyDto> _validationModifyRules;
        
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly IProcProcedureRepository _procProcedureRepository; 
        private readonly IProcProcessEquipmentGroupRepository _procProcessEquipmentGroupRepository;
        private readonly IProcEquipmentGroupParamDetailRepository _procEquipmentGroupParamDetailRepository;
        private readonly AbstractValidator<ProcEquipmentGroupParamDetailCreateDto> _paramDetailValidationCreateRules;
        private readonly IProcParameterRepository _procParameterRepository;

        public ProcEquipmentGroupParamService(ICurrentUser currentUser, ICurrentSite currentSite, IProcEquipmentGroupParamRepository procEquipmentGroupParamRepository, AbstractValidator<ProcEquipmentGroupParamCreateDto> validationCreateRules, AbstractValidator<ProcEquipmentGroupParamModifyDto> validationModifyRules, IProcMaterialRepository procMaterialRepository, IProcProcedureRepository procProcedureRepository, IProcProcessEquipmentGroupRepository procProcessEquipmentGroupRepository, IProcEquipmentGroupParamDetailRepository procEquipmentGroupParamDetailRepository, AbstractValidator<ProcEquipmentGroupParamDetailCreateDto> paramDetailValidationCreateRules, IProcParameterRepository procParameterRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procEquipmentGroupParamRepository = procEquipmentGroupParamRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procMaterialRepository = procMaterialRepository;
            _procProcedureRepository = procProcedureRepository;
            _procProcessEquipmentGroupRepository = procProcessEquipmentGroupRepository;
            _procEquipmentGroupParamDetailRepository = procEquipmentGroupParamDetailRepository;
            _paramDetailValidationCreateRules = paramDetailValidationCreateRules;
            _procParameterRepository = procParameterRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procEquipmentGroupParamCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcEquipmentGroupParamAsync(ProcEquipmentGroupParamCreateDto procEquipmentGroupParamCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procEquipmentGroupParamCreateDto);

            //DTO转换实体
            var procEquipmentGroupParamEntity = procEquipmentGroupParamCreateDto.ToEntity<ProcEquipmentGroupParamEntity>();
            procEquipmentGroupParamEntity.Id= IdGenProvider.Instance.CreateId();
            procEquipmentGroupParamEntity.CreatedBy = _currentUser.UserName;
            procEquipmentGroupParamEntity.UpdatedBy = _currentUser.UserName;
            procEquipmentGroupParamEntity.CreatedOn = HymsonClock.Now();
            procEquipmentGroupParamEntity.UpdatedOn = HymsonClock.Now();
            procEquipmentGroupParamEntity.SiteId = _currentSite.SiteId ?? 0;

            //验证是否编码唯一
            var entity = await _procEquipmentGroupParamRepository.GetByCodeAsync(new ProcEquipmentGroupParamCodeQuery { 
                Code = procEquipmentGroupParamEntity.Code,
                SiteId=_currentSite.SiteId??0
            });
            if (entity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18702));
            }
            //验证是否 逻辑 产品、工序、工艺组唯一性
            var entityOne = await _procEquipmentGroupParamRepository.GetByRelatesInformationAsync(new ProcEquipmentGroupParamRelatesInformationQuery
            {
                SiteId=_currentSite.SiteId ?? 0,
                ProductId= procEquipmentGroupParamEntity.ProductId,
                ProcedureId= procEquipmentGroupParamEntity.ProcedureId,
                EquipmentGroupId= procEquipmentGroupParamEntity.EquipmentGroupId
            });
            if (entityOne != null) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18724));
            }

            #region 处理 参数数据
                        var procEquipmentGroupParamDetailEntitys = new List<ProcEquipmentGroupParamDetailEntity>();
            if (procEquipmentGroupParamCreateDto.ParamList.Any())
            {
                foreach (var item in procEquipmentGroupParamCreateDto.ParamList)
                {
                    //验证数据
                    await _paramDetailValidationCreateRules.ValidateAndThrowAsync(item);

                    procEquipmentGroupParamDetailEntitys.Add(new ProcEquipmentGroupParamDetailEntity()
                    {
                        RecipeId= procEquipmentGroupParamEntity.Id,
                        ParamId= item.ParamId,
                        //ParamValue= item.ParamValue,
                        CenterValue= item.CenterValue,
                        MaxValue=item.MaxValue,
                        MinValue=item.MinValue,
                        DecimalPlaces=item.DecimalPlaces,


                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                }
            }
            #endregion

            using (TransactionScope ts = new TransactionScope())
            {
                //入库
                await _procEquipmentGroupParamRepository.InsertAsync(procEquipmentGroupParamEntity);

                if (procEquipmentGroupParamDetailEntitys.Any())
                    await _procEquipmentGroupParamDetailRepository.InsertsAsync(procEquipmentGroupParamDetailEntitys);

                ts.Complete();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcEquipmentGroupParamAsync(long id)
        {
            await _procEquipmentGroupParamRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcEquipmentGroupParamAsync(long[] ids)
        {
            //查询是否不是新建
            var entitys= await _procEquipmentGroupParamRepository.GetByIdsAsync(ids);
            if (entitys.Any() && entitys.Where(x => x.Status != SysDataStatusEnum.Build).Any()) 
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18714));
            }

            return await _procEquipmentGroupParamRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procEquipmentGroupParamPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcEquipmentGroupParamViewDto>> GetPagedListAsync(ProcEquipmentGroupParamPagedQueryDto procEquipmentGroupParamPagedQueryDto)
        {
            var procEquipmentGroupParamPagedQuery = procEquipmentGroupParamPagedQueryDto.ToQuery<ProcEquipmentGroupParamPagedQuery>();
            procEquipmentGroupParamPagedQuery.SiteId = _currentSite.SiteId??0;
            var pagedInfo = await _procEquipmentGroupParamRepository.GetPagedInfoAsync(procEquipmentGroupParamPagedQuery);

            List<ProcEquipmentGroupParamViewDto> procEquipmentGroupParamViewDtos= new List<ProcEquipmentGroupParamViewDto>();
            if (pagedInfo.Data != null && pagedInfo.Data.Any()) 
            {
                //查询相关的信息
                //var products = await _procMaterialRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProductId).ToArray());
                //var procedures = await _procProcedureRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.ProcedureId).ToArray());
                var procEquipmentGroups = await _procProcessEquipmentGroupRepository.GetByIdsAsync(pagedInfo.Data.Select(x => x.EquipmentGroupId).ToArray());

                foreach (var item in pagedInfo.Data)
                {
                    var procEquipmentGroupParamViewDto = item.ToModel<ProcEquipmentGroupParamViewDto>();
                    //procEquipmentGroupParamViewDto.MaterialCode = products.FirstOrDefault(x => x.Id == item.ProductId)?.MaterialCode??"";
                    //procEquipmentGroupParamViewDto.MaterialName = products.FirstOrDefault(x => x.Id == item.ProductId)?.MaterialName ?? "";
                    //procEquipmentGroupParamViewDto.ProcedureCode = procedures.FirstOrDefault(x => x.Id == item.ProcedureId)?.Code ?? "";
                    //procEquipmentGroupParamViewDto.ProcedureName = procedures.FirstOrDefault(x => x.Id == item.ProcedureId)?.Name ?? "";
                    procEquipmentGroupParamViewDto.MaterialNameVersion = item.MaterialName + "/" + item.MaterialVersion;
                    procEquipmentGroupParamViewDto.EquipmentGroupCode = procEquipmentGroups.FirstOrDefault(x => x.Id == item.EquipmentGroupId)?.Code ?? "";
                    procEquipmentGroupParamViewDto.EquipmentGroupName = procEquipmentGroups.FirstOrDefault(x => x.Id == item.EquipmentGroupId)?.Name ?? "";

                    procEquipmentGroupParamViewDtos.Add(procEquipmentGroupParamViewDto);

                }
            }

            return new PagedInfo<ProcEquipmentGroupParamViewDto>(procEquipmentGroupParamViewDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcEquipmentGroupParamDto> PrepareProcEquipmentGroupParamDtos(PagedInfo<ProcEquipmentGroupParamEntity>   pagedInfo)
        {
            var procEquipmentGroupParamDtos = new List<ProcEquipmentGroupParamDto>();
            foreach (var procEquipmentGroupParamEntity in pagedInfo.Data)
            {
                var procEquipmentGroupParamDto = procEquipmentGroupParamEntity.ToModel<ProcEquipmentGroupParamDto>();
                procEquipmentGroupParamDtos.Add(procEquipmentGroupParamDto);
            }

            return procEquipmentGroupParamDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procEquipmentGroupParamModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcEquipmentGroupParamAsync(ProcEquipmentGroupParamModifyDto procEquipmentGroupParamModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procEquipmentGroupParamModifyDto);

            //DTO转换实体
            var procEquipmentGroupParamEntity = procEquipmentGroupParamModifyDto.ToEntity<ProcEquipmentGroupParamEntity>();
            procEquipmentGroupParamEntity.UpdatedBy = _currentUser.UserName;
            procEquipmentGroupParamEntity.UpdatedOn = HymsonClock.Now();

            #region 验证状态
            var entity = await _procEquipmentGroupParamRepository.GetByIdAsync(procEquipmentGroupParamModifyDto.Id) ?? throw new BusinessException(nameof(ErrorCode.MES18701)); ;
            //if (entity.Status != SysDataStatusEnum.Build && procEquipmentGroupParamModifyDto.Status == SysDataStatusEnum.Build)
            //{
            //    throw new CustomerValidationException(nameof(ErrorCode.MES18713));
            //}
            switch (entity.Status)
            {
                case SysDataStatusEnum.Enable:
                case SysDataStatusEnum.Retain:
                case SysDataStatusEnum.Abolish:
                    if (procEquipmentGroupParamModifyDto.Status == SysDataStatusEnum.Build) throw new CustomerValidationException(nameof(ErrorCode.MES12510));
                    if (entity.Status == SysDataStatusEnum.Enable) throw new CustomerValidationException(nameof(ErrorCode.MES10123));
                    break;
                case SysDataStatusEnum.Build:
                default:
                    break;
            }
            #endregion

            //验证是否 逻辑 产品、工序、工艺组唯一性
            var entityOne = await _procEquipmentGroupParamRepository.GetByRelatesInformationAsync(new ProcEquipmentGroupParamRelatesInformationQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                ProductId = procEquipmentGroupParamEntity.ProductId,
                ProcedureId = procEquipmentGroupParamEntity.ProcedureId,
                EquipmentGroupId = procEquipmentGroupParamEntity.EquipmentGroupId
            });
            if (entityOne != null && entityOne.Id!= procEquipmentGroupParamModifyDto.Id)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18724));
            }

            #region 处理 参数数据
            var procEquipmentGroupParamDetailEntitys = new List<ProcEquipmentGroupParamDetailEntity>();
            if (procEquipmentGroupParamModifyDto.ParamList.Any())
            {
                foreach (var item in procEquipmentGroupParamModifyDto.ParamList)
                {
                    //验证数据
                    await _paramDetailValidationCreateRules.ValidateAndThrowAsync(item);

                    procEquipmentGroupParamDetailEntitys.Add(new ProcEquipmentGroupParamDetailEntity()
                    {
                        RecipeId = procEquipmentGroupParamEntity.Id,
                        ParamId = item.ParamId,
                        //ParamValue = item.ParamValue,
                        CenterValue = item.CenterValue,
                        MaxValue = item.MaxValue,
                        MinValue = item.MinValue,
                        DecimalPlaces = item.DecimalPlaces,


                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0,
                    });
                }
            }
            #endregion


            using (TransactionScope ts = new TransactionScope())
            {
                await _procEquipmentGroupParamRepository.UpdateAsync(procEquipmentGroupParamEntity);

                await _procEquipmentGroupParamDetailRepository.DeleteTrueByRecipeIdsAsync(new long[] { procEquipmentGroupParamEntity.Id });
                if (procEquipmentGroupParamDetailEntitys.Any())
                    await _procEquipmentGroupParamDetailRepository.InsertsAsync(procEquipmentGroupParamDetailEntitys);

                ts.Complete();
            }
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcEquipmentGroupParamViewDto> QueryProcEquipmentGroupParamByIdAsync(long id) 
        {
           var procEquipmentGroupParamEntity = await _procEquipmentGroupParamRepository.GetByIdAsync(id);
           if (procEquipmentGroupParamEntity != null) 
           {
                var dto= procEquipmentGroupParamEntity.ToModel<ProcEquipmentGroupParamViewDto>();

                //查询相关的信息
                var product = await _procMaterialRepository.GetByIdAsync(procEquipmentGroupParamEntity.ProductId);
                var procedure = await _procProcedureRepository.GetByIdAsync(procEquipmentGroupParamEntity.ProcedureId);
                var procEquipmentGroup = await _procProcessEquipmentGroupRepository.GetByIdAsync(procEquipmentGroupParamEntity.EquipmentGroupId);

                dto.MaterialCode=product?.MaterialCode??"";
                dto.MaterialName = product?.MaterialName??"";
                dto.MaterialNameVersion = product?.MaterialName +"/"+ product?.Version??"";
                dto.ProcedureCode=procedure?.Code??"";
                dto.ProcedureName=procedure?.Name??"";
                dto.EquipmentGroupCode = procEquipmentGroup?.Code ?? "";
                dto.EquipmentGroupName = procEquipmentGroup?.Name ?? "";

                return dto;
            }
            throw new CustomerValidationException(nameof(ErrorCode.MES18701));
        }

        /// <summary>
        /// 获取载具类型验证根据vehicleTypeId查询
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcEquipmentGroupParamDetailDto>> QueryProcEquipmentGroupParamDetailByRecipeIdAsync(long recipeId)
        {
            var procEquipmentGroupParamDetailEntitys = await _procEquipmentGroupParamDetailRepository.GetEntitiesByRecipeIdsAsync(new long[] { recipeId });

            var procEquipmentGroupParamDetailDtos = new List<ProcEquipmentGroupParamDetailDto>();
            #region 处理数据
            if (procEquipmentGroupParamDetailEntitys.Any())
            {
                //批量查询
                var paramEntitys = await _procParameterRepository.GetByIdsAsync(procEquipmentGroupParamDetailEntitys.Select(x => x.ParamId).Distinct().ToArray());

                foreach (var item in procEquipmentGroupParamDetailEntitys)
                {
                    var procEquipmentGroupParamDetailDto = item.ToModel<ProcEquipmentGroupParamDetailDto>();
                    var paramEntity = paramEntitys.FirstOrDefault(x => x.Id == item.ParamId);
                    procEquipmentGroupParamDetailDto.ParameterCode = paramEntity?.ParameterCode??"";
                    procEquipmentGroupParamDetailDto.ParameterName = paramEntity?.ParameterName??"";
                    procEquipmentGroupParamDetailDto.ParameterUnit = paramEntity?.ParameterUnit;
                    procEquipmentGroupParamDetailDto.DataType = paramEntity?.DataType;

                    procEquipmentGroupParamDetailDtos.Add(procEquipmentGroupParamDetailDto);
                }
            }

            #endregion

            return procEquipmentGroupParamDetailDtos;
        }
    }
}
