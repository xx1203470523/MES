using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.HttpClients;
using Hymson.MES.HttpClients.Requests.Print;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Newtonsoft.Json;

namespace Hymson.MES.Services.Services.Process.LabelTemplate
{
    /// <summary>
    /// 仓库标签模板 服务
    /// </summary>
    public class ProcLabelTemplateService : IProcLabelTemplateService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 仓库标签模板 仓储
        /// </summary>
        private readonly IProcLabelTemplateRepository _procLabelTemplateRepository;
        private readonly AbstractValidator<ProcLabelTemplateCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcLabelTemplateModifyDto> _validationModifyRules;
        private readonly ILabelPrintRequest _labelPrintRequest;

        public ProcLabelTemplateService(ICurrentUser currentUser, ICurrentSite currentSite
            , IProcLabelTemplateRepository procLabelTemplateRepository
            , AbstractValidator<ProcLabelTemplateCreateDto> validationCreateRules
            , AbstractValidator<ProcLabelTemplateModifyDto> validationModifyRules, ILabelPrintRequest labelPrintRequest)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _procLabelTemplateRepository = procLabelTemplateRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _labelPrintRequest = labelPrintRequest;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procLabelTemplateCreateDto"></param>
        /// <returns></returns>
        public async Task CreateProcLabelTemplateAsync(ProcLabelTemplateCreateDto procLabelTemplateCreateDto)
        {
            procLabelTemplateCreateDto.Name = procLabelTemplateCreateDto.Name.ToTrimSpace();
            procLabelTemplateCreateDto.Path = procLabelTemplateCreateDto.Path.ToTrimSpace();
            procLabelTemplateCreateDto.Remark = procLabelTemplateCreateDto.Remark ?? "".Trim();
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
            procLabelTemplateEntity.SiteId = _currentSite.SiteId ?? 123456;

            /*
            //同步模板文件到打印服务器
            if (!string.IsNullOrEmpty(procLabelTemplateEntity.Path))
            {
                var result = await _labelPrintRequest.GetTemplateContextAsync(procLabelTemplateEntity.Path);
                if (!result.result)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10356)).WithData("name", procLabelTemplateEntity.Name);
                }
                procLabelTemplateEntity.Content = result.data;
            }
            */

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

        public async Task<(string base64Str, bool result)> PreviewProcLabelTemplateAsync(long id)
        {
            var foo = await _procLabelTemplateRepository.GetByIdAsync(id);
            return await PreviewProcLabelTemplateAsync(foo.Content);


        }
        public async Task<(string base64Str, bool result)> PreviewProcLabelTemplateAsync(string content)
        {
            PreviewRequest request;
            if (string.IsNullOrEmpty(content))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10350));
            }
            try
            {
                request = JsonConvert.DeserializeObject<PreviewRequest>(content);
            }
            catch
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES17005));
            }
            var result = await _labelPrintRequest.PreviewFromImageBase64Async(request ?? new PreviewRequest());
            if (!result.result)
                throw new CustomerValidationException(nameof(ErrorCode.MES17004));
            return result;


        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procLabelTemplatePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcLabelTemplateDto>> GetPageListAsync(ProcLabelTemplatePagedQueryDto procLabelTemplatePagedQueryDto)
        {
            var procLabelTemplatePagedQuery = procLabelTemplatePagedQueryDto.ToQuery<ProcLabelTemplatePagedQuery>();
            procLabelTemplatePagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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
            procLabelTemplateModifyDto.Content = procLabelTemplateModifyDto.Content ?? "".Trim();
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

            /*
            //同步模板文件到打印服务器
            if (!string.Equals(foo.Path, procLabelTemplateEntity.Path, StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(procLabelTemplateEntity.Path))
                {
                    var result = await _labelPrintRequest.GetTemplateContextAsync(procLabelTemplateEntity.Path);
                    if (!result.result)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10356)).WithData("name", procLabelTemplateEntity.Name);
                    }
                    procLabelTemplateEntity.Content = result.data;
                }
            }
            */

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
            return await _procLabelTemplateRepository.GetByNameAsync(new ProcLabelTemplateByNameQuery() { SiteId = _currentSite.SiteId ?? 123456, Name = name });

        }
    }
}
