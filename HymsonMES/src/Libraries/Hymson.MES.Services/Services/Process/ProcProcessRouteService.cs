/*
 *creator: Karl
 *
 *describe: 工艺路线表    服务 | 代码由框架生成
 *builder:  zhaoqing
 *build datetime: 2023-02-14 10:07:11
 */
using FluentValidation;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.Resource;
using Hymson.MES.Services.Dtos.Process;
using Hymson.MES.Services.Services.Process.IProcessService;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 工艺路线表 服务
    /// </summary>
    public class ProcProcessRouteService : IProcProcessRouteService
    {
        /// <summary>
        /// 工艺路线表 仓储
        /// </summary>
        private readonly IProcProcessRouteRepository _procProcessRouteRepository;
        /// <summary>
        /// 仓储（工艺路线节点）
        /// </summary>
        private readonly IProcProcessRouteDetailNodeRepository _procProcessRouteNodeRepository;
        /// <summary>
        /// 仓储（工艺路线连线）
        /// </summary>
        private readonly IProcProcessRouteDetailLinkRepository _procProcessRouteLinkRepository;
        private readonly AbstractValidator<ProcProcessRouteCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcProcessRouteModifyDto> _validationModifyRules;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcProcessRouteService(IProcProcessRouteRepository procProcessRouteRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteLinkRepository,
            AbstractValidator<ProcProcessRouteCreateDto> validationCreateRules,
            AbstractValidator<ProcProcessRouteModifyDto> validationModifyRules)
        {
            _procProcessRouteRepository = procProcessRouteRepository;
            _procProcessRouteNodeRepository = procProcessRouteNodeRepository;
            _procProcessRouteLinkRepository = procProcessRouteLinkRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcessRoutePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcessRouteDto>> GetPageListAsync(ProcProcessRoutePagedQueryDto procProcessRoutePagedQueryDto)
        {
            var procProcessRoutePagedQuery = procProcessRoutePagedQueryDto.ToQuery<ProcProcessRoutePagedQuery>();
            procProcessRoutePagedQuery.SiteCode = "TODO";
            var pagedInfo = await _procProcessRouteRepository.GetPagedInfoAsync(procProcessRoutePagedQuery);

            //实体到DTO转换 装载数据
            List<ProcProcessRouteDto> procProcessRouteDtos = PrepareProcProcessRouteDtos(pagedInfo);
            return new PagedInfo<ProcProcessRouteDto>(procProcessRouteDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CustomProcessRoute> GetCustomProcProcessRouteAsync(long id)
        {
            CustomProcessRoute model = null;
            var processRoute = await _procProcessRouteRepository.GetByIdAsync(id);
            if (processRoute != null)
            {
                model ??= new CustomProcessRoute { };
                model.Info = processRoute.ToModel<ProcProcessRouteDto>(); ;
            }

            var nodeQuery = new ProcProcessRouteDetailNodeQuery();
            var nodes = await _procProcessRouteNodeRepository.GetListAsync(nodeQuery);
            //实体转换
            return null;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procProcessRouteDto"></param>
        /// <returns></returns>
        public async Task CreateProcProcessRouteAsync(ProcProcessRouteCreateDto procProcessRouteCreateDto)
        {
            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procProcessRouteCreateDto);

            //DTO转换实体
            var procProcessRouteEntity = procProcessRouteCreateDto.ToEntity<ProcProcessRouteEntity>();
            procProcessRouteEntity.Id = IdGenProvider.Instance.CreateId();
            procProcessRouteEntity.CreatedBy = "TODO";
            procProcessRouteEntity.UpdatedBy = "TODO";
            procProcessRouteEntity.CreatedOn = HymsonClock.Now();
            procProcessRouteEntity.UpdatedOn = HymsonClock.Now();

            //入库
            await _procProcessRouteRepository.InsertAsync(procProcessRouteEntity);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procProcessRouteDto"></param>
        /// <returns></returns>
        public async Task ModifyProcProcessRouteAsync(ProcProcessRouteModifyDto procProcessRouteModifyDto)
        {
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procProcessRouteModifyDto);

            //DTO转换实体
            var procProcessRouteEntity = procProcessRouteModifyDto.ToEntity<ProcProcessRouteEntity>();
            procProcessRouteEntity.UpdatedBy = "TODO";
            procProcessRouteEntity.UpdatedOn = HymsonClock.Now();

            await _procProcessRouteRepository.UpdateAsync(procProcessRouteEntity);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcProcessRouteAsync(string ids)
        {
            var idsArr = StringExtension.SpitLongArrary(ids);
            if (idsArr.Length < 1)
            {
                throw new NotFoundException(ErrorCode.MES10102);
            }

            #region 参数校验
            //有生产中工单引用当前工艺路线，不能删除！
            // // 状态（0:新建;1:启用;2:保留;3:废除）
            //TODO 状态枚举由proc_process_status改为sys_data_status
            var statusArr = new string[] { SysDataStatusEnum.Enable.ToString(), SysDataStatusEnum.Retain.ToString() };
            var query = new ProcProcessRouteQuery
            {
                Ids = idsArr,
                StatusArr = statusArr
            };
            var resourceList = await _procProcessRouteRepository.IsIsExistsEnabledAsync(query);
            if (resourceList != null)
            {
                throw new CustomerValidationException(ErrorCode.MES10430);
            }
            #endregion

            return await _procProcessRouteRepository.DeleteRangeAsync(idsArr);
        }

        #region 业务扩展方法

        /// <summary>
        /// 分页查询实体转换
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcProcessRouteDto> PrepareProcProcessRouteDtos(PagedInfo<ProcProcessRouteEntity> pagedInfo)
        {
            var procProcessRouteDtos = new List<ProcProcessRouteDto>();
            foreach (var procProcessRouteEntity in pagedInfo.Data)
            {
                var procProcessRouteDto = procProcessRouteEntity.ToModel<ProcProcessRouteDto>();
                procProcessRouteDtos.Add(procProcessRouteDto);
            }

            return procProcessRouteDtos;
        }

        /// <summary>
        /// 转换节点工序（编码）
        /// </summary>
        /// <param name="procedureId"></param>
        /// <param name="defaultCode"></param>
        /// <returns></returns>
        //public static string ConvertProcedureBomCode(long procedureId, string defaultCode)
        //{
        //    return procedureId switch
        //    {
        //        StartNodeId => "",
        //        EndNodeId => "",
        //        _ => defaultCode,
        //    };
        //}

        /// <summary>
        /// 转换节点工序（名称）
        /// </summary>
        /// <param name="procedureId"></param>
        /// <param name="defaultName"></param>
        /// <returns></returns>
        //public static string ConvertProcedureBomName(long procedureId, string defaultName)
        //{
        //    return procedureId switch
        //    {
        //        StartNodeId => "",    // 开始
        //        EndNodeId => "",  // 结束
        //        _ => defaultName,
        //    };
        //}

        /// <summary>
        /// 转换集合（工艺路线-节点）
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //public static List<ProcProcessRouteDetailNodeDto> ConvertProcessRouteNodeList(IEnumerable<FlowDynamicNodeDto> nodeList, ProcProcessRoute model)
        //{
        //    if (nodeList == null || nodeList.Any() == false) return new List<ProcProcessRouteDetailNodeDto> { };

        //    return nodeList.Select(s => new ProcProcessRouteDetailNodeDto
        //    {
        //        SiteCode = model.SiteCode,
        //        ProcessRouteId = model.Id,
        //        SerialNo = s.SerialNo,
        //        ProcedureId = s.ProcedureBomId.ParseToLong(),
        //        CheckType = s.CheckType,
        //        CheckRate = s.CheckRate,
        //        IsWorkReport = s.IsWorkReport,
        //        IsFirstProcess = s.IsFirstProcess,
        //        Extra1 = s.Extra1,
        //        CreatedBy = model.UpdateBy,
        //        UpdatedBy = model.UpdateBy
        //    }).ToList();
        //}

        /// <summary>
        /// 转换集合（工艺路线-连线）
        /// </summary>
        /// <param name="linkList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        //public static List<ProcProcessRouteDetailLinkDto> ConvertProcessRouteLinkList(IEnumerable<FlowDynamicLinkDto> linkList, ProcProcessRoute model)
        //{
        //    if (linkList == null || linkList.Any() == false) return new List<ProcProcessRouteDetailLinkDto> { };

        //    return linkList.Select(s => new ProcProcessRouteDetailLinkDto
        //    {
        //        SiteCode = model.SiteCode,
        //        ProcessRouteId = model.Id,
        //        SerialNo = s.SerialNo,
        //        PreProcessRouteDetailId = s.PreProcessRouteDetailId,
        //        ProcessRouteDetailId = s.ProcessRouteDetailId,
        //        Extra1 = s.Extra1,
        //        CreatedBy = model.UpdateBy,
        //        UpdatedBy = model.UpdateBy
        //    }).ToList();
        //}
        #endregion
    }
}
