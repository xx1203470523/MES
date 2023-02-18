/*
 *creator: Karl
 *
 *describe: 上料点表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-17 08:57:53
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 上料点表 服务
    /// </summary>
    public class ProcLoadPointService : IProcLoadPointService
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 上料点表 仓储
        /// </summary>
        private readonly IProcLoadPointRepository _procLoadPointRepository;
        private readonly AbstractValidator<ProcLoadPointCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcLoadPointModifyDto> _validationModifyRules;

        public ProcLoadPointService(ICurrentUser currentUser,IProcLoadPointRepository procLoadPointRepository, AbstractValidator<ProcLoadPointCreateDto> validationCreateRules, AbstractValidator<ProcLoadPointModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _procLoadPointRepository = procLoadPointRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procLoadPointDto"></param>
        /// <returns></returns>
        public async Task CreateProcLoadPointAsync(ProcLoadPointCreateDto procLoadPointCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procLoadPointCreateDto);

            //DTO转换实体
            var procLoadPointEntity = procLoadPointCreateDto.ToEntity<ProcLoadPointEntity>();
            procLoadPointEntity.Id= IdGenProvider.Instance.CreateId();
            procLoadPointEntity.CreatedBy = _currentUser.UserName;
            procLoadPointEntity.UpdatedBy = _currentUser.UserName;
            procLoadPointEntity.CreatedOn = HymsonClock.Now();
            procLoadPointEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _procLoadPointRepository.InsertAsync(procLoadPointEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcLoadPointAsync(long id)
        {
            await _procLoadPointRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcLoadPointAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            return await _procLoadPointRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procLoadPointPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLoadPointDto>> GetPageListAsync(ProcLoadPointPagedQueryDto procLoadPointPagedQueryDto)
        {
            var procLoadPointPagedQuery = procLoadPointPagedQueryDto.ToQuery<ProcLoadPointPagedQuery>();
            var pagedInfo = await _procLoadPointRepository.GetPagedInfoAsync(procLoadPointPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcLoadPointDto> procLoadPointDtos = PrepareProcLoadPointDtos(pagedInfo);
            return new PagedInfo<ProcLoadPointDto>(procLoadPointDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcLoadPointDto> PrepareProcLoadPointDtos(PagedInfo<ProcLoadPointEntity>   pagedInfo)
        {
            var procLoadPointDtos = new List<ProcLoadPointDto>();
            foreach (var procLoadPointEntity in pagedInfo.Data)
            {
                var procLoadPointDto = procLoadPointEntity.ToModel<ProcLoadPointDto>();
                procLoadPointDtos.Add(procLoadPointDto);
            }

            return procLoadPointDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLoadPointDto"></param>
        /// <returns></returns>
        public async Task ModifyProcLoadPointAsync(ProcLoadPointModifyDto procLoadPointModifyDto)
        {
             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procLoadPointModifyDto);

            //DTO转换实体
            var procLoadPointEntity = procLoadPointModifyDto.ToEntity<ProcLoadPointEntity>();
            procLoadPointEntity.UpdatedBy = _currentUser.UserName;
            procLoadPointEntity.UpdatedOn = HymsonClock.Now();

            await _procLoadPointRepository.UpdateAsync(procLoadPointEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcLoadPointDto> QueryProcLoadPointByIdAsync(long id) 
        {
           var procLoadPointEntity = await _procLoadPointRepository.GetByIdAsync(id);
           if (procLoadPointEntity != null) 
           {
               return procLoadPointEntity.ToModel<ProcLoadPointDto>();
           }
            return null;
        }
    }
}
