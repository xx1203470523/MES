/*
 *creator: Karl
 *
 *describe: 开机参数采集表    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-07-12 04:58:46
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

namespace Hymson.MES.EquipmentServices.Services.Process
{
    /// <summary>
    /// 开机参数采集表 服务
    /// </summary>
    public class ProcBootupparamrecordService : IProcBootupparamrecordService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 开机参数采集表 仓储
        /// </summary>
        private readonly IProcBootupparamrecordRepository _procBootupparamrecordRepository;
        private readonly AbstractValidator<ProcBootupparamrecordCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcBootupparamrecordModifyDto> _validationModifyRules;

        public ProcBootupparamrecordService(ICurrentUser currentUser, ICurrentSite currentSite, IProcBootupparamrecordRepository procBootupparamrecordRepository, AbstractValidator<ProcBootupparamrecordCreateDto> validationCreateRules, AbstractValidator<ProcBootupparamrecordModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procBootupparamrecordRepository = procBootupparamrecordRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procBootupparamrecordCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcBootupparamrecordAsync(ProcBootupparamrecordCreateDto procBootupparamrecordCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procBootupparamrecordCreateDto);

            //DTO转换实体
            var procBootupparamrecordEntity = procBootupparamrecordCreateDto.ToEntity<ProcBootupparamrecordEntity>();
            procBootupparamrecordEntity.Id= IdGenProvider.Instance.CreateId();
            procBootupparamrecordEntity.CreatedBy = _currentUser.UserName;
            procBootupparamrecordEntity.UpdatedBy = _currentUser.UserName;
            procBootupparamrecordEntity.CreatedOn = HymsonClock.Now();
            procBootupparamrecordEntity.UpdatedOn = HymsonClock.Now();
            procBootupparamrecordEntity.SiteId = _currentSite.SiteId ?? 123456;

            //入库
            await _procBootupparamrecordRepository.InsertAsync(procBootupparamrecordEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcBootupparamrecordAsync(long id)
        {
            await _procBootupparamrecordRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcBootupparamrecordAsync(long[] ids)
        {
            return await _procBootupparamrecordRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procBootupparamrecordPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBootupparamrecordDto>> GetPagedListAsync(ProcBootupparamrecordPagedQueryDto procBootupparamrecordPagedQueryDto)
        {
            var procBootupparamrecordPagedQuery = procBootupparamrecordPagedQueryDto.ToQuery<ProcBootupparamrecordPagedQuery>();
            var pagedInfo = await _procBootupparamrecordRepository.GetPagedInfoAsync(procBootupparamrecordPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcBootupparamrecordDto> procBootupparamrecordDtos = PrepareProcBootupparamrecordDtos(pagedInfo);
            return new PagedInfo<ProcBootupparamrecordDto>(procBootupparamrecordDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcBootupparamrecordDto> PrepareProcBootupparamrecordDtos(PagedInfo<ProcBootupparamrecordEntity>   pagedInfo)
        {
            var procBootupparamrecordDtos = new List<ProcBootupparamrecordDto>();
            foreach (var procBootupparamrecordEntity in pagedInfo.Data)
            {
                var procBootupparamrecordDto = procBootupparamrecordEntity.ToModel<ProcBootupparamrecordDto>();
                procBootupparamrecordDtos.Add(procBootupparamrecordDto);
            }

            return procBootupparamrecordDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procBootupparamrecordDto"></param>
        /// <returns></returns>
        public async Task ModifyProcBootupparamrecordAsync(ProcBootupparamrecordModifyDto procBootupparamrecordModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procBootupparamrecordModifyDto);

            //DTO转换实体
            var procBootupparamrecordEntity = procBootupparamrecordModifyDto.ToEntity<ProcBootupparamrecordEntity>();
            procBootupparamrecordEntity.UpdatedBy = _currentUser.UserName;
            procBootupparamrecordEntity.UpdatedOn = HymsonClock.Now();

            await _procBootupparamrecordRepository.UpdateAsync(procBootupparamrecordEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBootupparamrecordDto> QueryProcBootupparamrecordByIdAsync(long id) 
        {
           var procBootupparamrecordEntity = await _procBootupparamrecordRepository.GetByIdAsync(id);
           if (procBootupparamrecordEntity != null) 
           {
               return procBootupparamrecordEntity.ToModel<ProcBootupparamrecordDto>();
           }
            return null;
        }
    }
}
