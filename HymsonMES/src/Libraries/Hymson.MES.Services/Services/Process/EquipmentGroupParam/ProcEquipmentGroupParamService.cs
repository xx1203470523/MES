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

        public ProcEquipmentGroupParamService(ICurrentUser currentUser, ICurrentSite currentSite, IProcEquipmentGroupParamRepository procEquipmentGroupParamRepository, AbstractValidator<ProcEquipmentGroupParamCreateDto> validationCreateRules, AbstractValidator<ProcEquipmentGroupParamModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procEquipmentGroupParamRepository = procEquipmentGroupParamRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
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

            //入库
            await _procEquipmentGroupParamRepository.InsertAsync(procEquipmentGroupParamEntity);
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
            return await _procEquipmentGroupParamRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procEquipmentGroupParamPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcEquipmentGroupParamDto>> GetPagedListAsync(ProcEquipmentGroupParamPagedQueryDto procEquipmentGroupParamPagedQueryDto)
        {
            var procEquipmentGroupParamPagedQuery = procEquipmentGroupParamPagedQueryDto.ToQuery<ProcEquipmentGroupParamPagedQuery>();
            var pagedInfo = await _procEquipmentGroupParamRepository.GetPagedInfoAsync(procEquipmentGroupParamPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcEquipmentGroupParamDto> procEquipmentGroupParamDtos = PrepareProcEquipmentGroupParamDtos(pagedInfo);
            return new PagedInfo<ProcEquipmentGroupParamDto>(procEquipmentGroupParamDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
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
        /// <param name="procEquipmentGroupParamDto"></param>
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

            await _procEquipmentGroupParamRepository.UpdateAsync(procEquipmentGroupParamEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcEquipmentGroupParamDto> QueryProcEquipmentGroupParamByIdAsync(long id) 
        {
           var procEquipmentGroupParamEntity = await _procEquipmentGroupParamRepository.GetByIdAsync(id);
           if (procEquipmentGroupParamEntity != null) 
           {
               return procEquipmentGroupParamEntity.ToModel<ProcEquipmentGroupParamDto>();
           }
            return null;
        }
    }
}
