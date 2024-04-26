using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Text.RegularExpressions;

namespace Hymson.MES.Services.Services.Process.PrintConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcPrintConfigService : IProcPrintConfigService
    {
        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;
        private readonly IProcResourceConfigPrintRepository _resourceRepository;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;
        /// <summary>
        /// 打印配置仓储
        /// </summary>
        private readonly IProcPrintConfigRepository _printConfigRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly AbstractValidator<ProcPrinterDto> _validationCreateRules;
        private readonly AbstractValidator<ProcPrinterUpdateDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcPrintConfigService(ICurrentUser currentUser, ICurrentSite currentSite,
                  IProcPrintConfigRepository printConfigRepository,
                  IProcResourceConfigPrintRepository resourceRepository,
                  AbstractValidator<ProcPrinterDto> validationCreateRules,
                 AbstractValidator<ProcPrinterUpdateDto> validationModifyRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _resourceRepository = resourceRepository;
            _printConfigRepository = printConfigRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcPrinterDto> GetByIdAsync(long id)
        {
            var entity = await _printConfigRepository.GetByIdAsync(id);
            return entity?.ToModel<ProcPrinterDto>() ?? new ProcPrinterDto();
        }

        /// <summary>
        /// 通过打印机名称获取指定打印机
        /// </summary>
        /// <param name="printName"></param>
        /// <returns></returns>
        public async Task<ProcPrinterDto> GetByPrintNameAsync(string printName)
        {
            var entity = await _printConfigRepository.GetByPrintNameAsync(new EntityByCodeQuery { Code= printName,Site=_currentSite.SiteId??0});
            return entity?.ToModel<ProcPrinterDto>() ?? new ProcPrinterDto();
        }

        /// <summary>
        /// 查询打印配置表列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcPrinterDto>> GetPageListAsync(ProcPrinterPagedQueryDto query)
        {
            var printPagedQuery = query.ToQuery<ProcPrinterPagedQuery>();
            printPagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _printConfigRepository.GetPagedListAsync(printPagedQuery);

            //实体到DTO转换 装载数据
            var procprintDtos = new List<ProcPrinterDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceViewDto = entity.ToModel<ProcPrinterDto>();
                procprintDtos.Add(resourceViewDto);
            }
            return new PagedInfo<ProcPrinterDto>(procprintDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 获取打印配置分页列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcPrinterDto>> GetListAsync(ProcPrinterPagedQueryDto query)
        {
            var resourcePagedQuery = query.ToQuery<ProcPrinterPagedQuery>();
            resourcePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _printConfigRepository.GetListAsync(resourcePagedQuery);

            //实体到DTO转换 装载数据
            var procResourceDtos = new List<ProcPrinterDto>();
            foreach (var entity in pagedInfo.Data)
            {
                var resourceTypeDto = entity.ToModel<ProcPrinterDto>();
                procResourceDtos.Add(resourceTypeDto);
            }
            return new PagedInfo<ProcPrinterDto>(procResourceDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 添加打印配置数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task AddProcPrintConfigAsync(ProcPrinterDto param)
        {
            param.PrintIp = param.PrintIp.ToTrimSpace();
            param.Remark = param.Remark?.Trim();

            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(param);

            if (!IsValidIP(param.PrintIp))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10379));
            }

                var userName = _currentUser.UserName;
            var entity = new ProcPrinterEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentSite.SiteId ?? 0,
                CreatedBy = userName,
                UpdatedBy = userName,
                Remark = param.Remark ?? "",
                PrintName = param.PrintName,
                PrintIp = param.PrintIp
            };

            var nameEntity = await _printConfigRepository.GetByPrintNameAsync(new EntityByCodeQuery { Site=_currentSite.SiteId??0,Code= param.PrintName });
            if (nameEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10341));

            // 检查IP是否重复
            var ipEntity = await _printConfigRepository.GetByPrintIpAsync(new EntityByCodeQuery
            {
                Site = entity.SiteId,
                Code = entity.PrintIp
            });
            if (ipEntity != null) throw new CustomerValidationException(nameof(ErrorCode.MES10348));

            // 新增
            await _printConfigRepository.InsertAsync(entity);
        }

        /// <summary>
        /// 修改资源类型数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateProcPrintConfigAsync(ProcPrinterUpdateDto param)
        {
           
            param.Remark = param.Remark.Trim();

            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(param);

            // 读取实体
            var entity = await _printConfigRepository.GetByIdAsync(param.Id)
                ?? throw new NotFoundException(nameof(ErrorCode.MES10309));

            entity.PrintName = param.PrintName;
            entity.Remark = param.Remark;
            entity.UpdatedBy = _currentUser.UserName;
            entity.UpdatedOn = HymsonClock.Now();

            // 更新
            await _printConfigRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcPrintConfigAsync(long[] idsAr)
        {
            if (idsAr.Length < 1)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10102));
            }

            //查询资源类型是否关联资源
            var query = new ProcResourceConfigPrintQuery
            {
                Ids = idsAr
            };
            var resourceList = await _resourceRepository.GetByPrintIdAsync(query);
            if (resourceList != null && resourceList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10312));
            }

            var command = new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsAr
            };
            return await _printConfigRepository.DeletesAsync(command);
        }

        private bool IsValidIP(string ipstr)
        {
            // IP 地址的正则表达式
            string pattern = @"^([01]?\d\d?|2[0-4]\d|25[0-5])\.([01]?\d\d?|2[0-4]\d|25[0-5])\.([01]?\d\d?|2[0-4]\d|25[0-5])\.([01]?\d\d?|2[0-4]\d|25[0-5])$";

            // 创建 Regex 对象
            Regex regex = new Regex(pattern);

            // 使用正则表达式测试输入的字符串
            return regex.IsMatch(ipstr);
        }

    }
}
