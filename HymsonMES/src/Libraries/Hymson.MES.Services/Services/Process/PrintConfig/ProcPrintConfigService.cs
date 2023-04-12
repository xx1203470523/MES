using FluentValidation;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Authentication;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils.Tools;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process.PrintConfig
{

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

        private readonly AbstractValidator<ProcPrinterDto> _validationCreateRules;
        //private readonly AbstractValidator<ProcResourceModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcPrintConfigService(ICurrentUser currentUser, ICurrentSite currentSite,
                  IProcPrintConfigRepository printConfigRepository,
                  IProcResourceConfigPrintRepository resourceRepository, AbstractValidator<ProcPrinterDto> validationRules)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _resourceRepository = resourceRepository;
            _printConfigRepository = printConfigRepository;
            _validationCreateRules = validationRules;
            //_validationModifyRules = validationModifyRules;
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
            var entity = await _printConfigRepository.GetByPrintNameAsync(printName);
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
            //验证DTO
            //var dto = new ProcResourceTypeDto();
            await _validationCreateRules.ValidateAndThrowAsync(param);
            if (param == null || string.IsNullOrEmpty(param.PrintName))
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }

            var userName = _currentUser.UserName;
            var siteId = _currentSite.SiteId ?? 0;
            //DTO转换实体
            var id = IdGenProvider.Instance.CreateId();

            var entity = new ProcPrinterEntity
            {
                Id = id,
                SiteId = siteId,
                CreatedBy = userName,
                UpdatedBy = userName,
                Remark = param.Remark ?? "",
                PrintName = param.PrintName,
                PrintIp = param.PrintIp ?? ""
            };


            var foo = await _printConfigRepository.GetByPrintNameAsync(param.PrintName);
            if (foo != null)
            {
                throw new BusinessException(nameof(ErrorCode.MES10306)).WithData("PrintName", param.PrintName);
            }


            //入库
            await _printConfigRepository.InsertAsync(entity);

        }

        /// <summary>
        /// 修改资源类型数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task UpdateProcPrintConfigAsync(ProcPrinterUpdateDto param)
        {
            //验证DTO
            //var dto = new ProcResourceTypeDto();
            //await _validationRules.ValidateAndThrowAsync(dto);
            if (param == null)
            {
                throw new ValidationException(nameof(ErrorCode.MES10100));
            }
            var entity = await _printConfigRepository.GetByIdAsync(param?.Id ?? 0);
            if (entity == null)
            {
                throw new NotFoundException(nameof(ErrorCode.MES10309));
            }

            var userName = _currentUser.UserName;
            //DTO转换实体
            var updateEntity = new ProcPrinterEntity
            {
                Id = param.Id,
                Remark = param.Remark ?? "",
                PrintName = param.PrintName ?? "",
                PrintIp = param.PrintIp ?? "",
                UpdatedBy = userName
            };

            //var resources = _procResourceRepository.GetProcResrouces(ids, parm.Id);
            //if (resources.Count > 0)
            //{
            //    apiResult.Code = (int)ResultCode.PARAM_ERROR;
            //    apiResult.Msg = "一个资源只能关联一个资源类型！";
            //    return apiResult;
            //}


            //更新
            await _printConfigRepository.UpdateAsync(updateEntity);



        }

        /// <summary>
        /// 批量删除资源类型数据
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcPrintConfigAsync(long[] idsArr)
        {
            if (idsArr.Length < 1)
            {
                throw new ValidationException(nameof(ErrorCode.MES10102));
            }

            //查询资源类型是否关联资源
            var siteId = _currentSite.SiteId ?? 0;
            var query = new ProcResourceConfigPrintQuery
            {
                Ids = idsArr
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
                Ids = idsArr
            };
            return await _printConfigRepository.DeletesAsync(command);
        }

    }
}
