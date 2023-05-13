/*
 *creator: Karl
 *
 *describe: 仓库标签模板    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-03-09 02:51:26
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

using System.Transactions;

namespace Hymson.MES.Services.Services.Process.LabelTemplate
{
    /// <summary>
    /// 仓库标签模板 服务
    /// </summary>
    public class ProcLabelTemplateService : IProcLabelTemplateService
    {
        private readonly ICurrentUser _currentUser;

        /// <summary>
        /// 仓库标签模板 仓储
        /// </summary>
        private readonly IProcLabelTemplateRepository _procLabelTemplateRepository;
        private readonly AbstractValidator<ProcLabelTemplateCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcLabelTemplateModifyDto> _validationModifyRules;

        public ProcLabelTemplateService(ICurrentUser currentUser, IProcLabelTemplateRepository procLabelTemplateRepository, AbstractValidator<ProcLabelTemplateCreateDto> validationCreateRules, AbstractValidator<ProcLabelTemplateModifyDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _procLabelTemplateRepository = procLabelTemplateRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procLabelTemplateCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcLabelTemplateAsync(ProcLabelTemplateCreateDto procLabelTemplateCreateDto)
        {
            procLabelTemplateCreateDto.Name = procLabelTemplateCreateDto.Name.ToTrimSpace();
            procLabelTemplateCreateDto.Path=procLabelTemplateCreateDto.Path.ToTrimSpace();
            procLabelTemplateCreateDto.Remark = procLabelTemplateCreateDto.Remark??"".Trim();
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procLabelTemplateCreateDto);

            //DTO转换实体
            var procLabelTemplateEntity = procLabelTemplateCreateDto.ToEntity<ProcLabelTemplateEntity>();

            //验证模板名称是否重复
            var foo = await QueryProcLabelTemplateByNameAsync(procLabelTemplateEntity.Name);
            if (foo != null)
            {
                throw new BusinessException(nameof(ErrorCode.MES10340)).WithData("Name", procLabelTemplateEntity.Name);
            }
            procLabelTemplateEntity.Id = IdGenProvider.Instance.CreateId();
            procLabelTemplateEntity.CreatedBy = _currentUser.UserName;
            procLabelTemplateEntity.UpdatedBy = _currentUser.UserName;
            procLabelTemplateEntity.CreatedOn = HymsonClock.Now();
            procLabelTemplateEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _procLabelTemplateRepository.InsertAsync(procLabelTemplateEntity);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcLabelTemplateAsync(long id)
        {
            await _procLabelTemplateRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcLabelTemplateAsync(long[] idsArr)
        {
            //  var idsArr = StringExtension.SpitLongArrary(ids);
            return await _procLabelTemplateRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procLabelTemplatePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLabelTemplateDto>> GetPageListAsync(ProcLabelTemplatePagedQueryDto procLabelTemplatePagedQueryDto)
        {
            var procLabelTemplatePagedQuery = procLabelTemplatePagedQueryDto.ToQuery<ProcLabelTemplatePagedQuery>();
            var pagedInfo = await _procLabelTemplateRepository.GetPagedInfoAsync(procLabelTemplatePagedQuery);

            //实体到DTO转换 装载数据
            List<ProcLabelTemplateDto> procLabelTemplateDtos = PrepareProcLabelTemplateDtos(pagedInfo);
            return new PagedInfo<ProcLabelTemplateDto>(procLabelTemplateDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcLabelTemplateDto> PrepareProcLabelTemplateDtos(PagedInfo<ProcLabelTemplateEntity> pagedInfo)
        {
            var procLabelTemplateDtos = new List<ProcLabelTemplateDto>();
            foreach (var procLabelTemplateEntity in pagedInfo.Data)
            {
                var procLabelTemplateDto = procLabelTemplateEntity.ToModel<ProcLabelTemplateDto>();
                procLabelTemplateDtos.Add(procLabelTemplateDto);
            }

            return procLabelTemplateDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procLabelTemplateModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyProcLabelTemplateAsync(ProcLabelTemplateModifyDto procLabelTemplateModifyDto)
        {
            procLabelTemplateModifyDto.Name = procLabelTemplateModifyDto.Name.ToTrimSpace();
            procLabelTemplateModifyDto.Path = procLabelTemplateModifyDto.Path.ToTrimSpace();
            procLabelTemplateModifyDto.Remark = procLabelTemplateModifyDto.Remark ?? "".Trim();
            //procLabelTemplateModifyDto.Content = procLabelTemplateModifyDto.Content.Trim();
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procLabelTemplateModifyDto);

            //DTO转换实体
            var procLabelTemplateEntity = procLabelTemplateModifyDto.ToEntity<ProcLabelTemplateEntity>();
            //验证模板名称是否重复
            var foo = await QueryProcLabelTemplateByNameAsync(procLabelTemplateEntity.Name);
            if (foo != null && foo.Id != procLabelTemplateEntity.Id)
            {
                throw new BusinessException(nameof(ErrorCode.MES10340)).WithData("Name", procLabelTemplateEntity.Name);
            }

            procLabelTemplateEntity.UpdatedBy = _currentUser.UserName;
            procLabelTemplateEntity.UpdatedOn = HymsonClock.Now();

            await _procLabelTemplateRepository.UpdateAsync(procLabelTemplateEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcLabelTemplateDto> QueryProcLabelTemplateByIdAsync(long id)
        {
            var procLabelTemplateEntity = await _procLabelTemplateRepository.GetByIdAsync(id);
            if (procLabelTemplateEntity != null)
            {
                return procLabelTemplateEntity.ToModel<ProcLabelTemplateDto>();
            }
            return null;
        }
        private async Task<ProcLabelTemplateEntity> QueryProcLabelTemplateByNameAsync(string name)
        {
            return await _procLabelTemplateRepository.GetByNameAsync(name);

        }
    }
}
