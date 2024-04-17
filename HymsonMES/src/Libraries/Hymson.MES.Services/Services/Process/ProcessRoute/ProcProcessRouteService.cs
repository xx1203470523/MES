using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.Localization.Services;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Constants.Process;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Data.Repositories.Process.ProcessRoute.Command;
using Hymson.MES.Data.Repositories.Process.ProcessRoute.Query;
using Hymson.MES.Services.Dtos.Common;
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process.ProcessRoute
{
    /// <summary>
    /// 工艺路线表 服务
    /// </summary>
    public class ProcProcessRouteService : IProcProcessRouteService
    {
        /// <summary>
        /// 节点ID（开始）
        /// </summary>
        const int StartNodeId = 0;
        /// <summary>
        /// 节点ID（结束）
        /// </summary>
        const int EndNodeId = 999999999;

        /// <summary>
        /// 当前登录用户对象
        /// </summary>
        private readonly ICurrentUser _currentUser;
        /// <summary>
        /// 当前站点
        /// </summary>
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 验证器
        /// </summary>
        private readonly AbstractValidator<ProcProcessRouteCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcProcessRouteModifyDto> _validationModifyRules;

        private readonly AbstractValidator<FlowDynamicLinkDto> _validationFlowDynamicLinkRules;
        private readonly AbstractValidator<FlowDynamicNodeDto> _validationFlowDynamicNodeRules;

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

        private readonly ILocalizationService _localizationService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="currentSite"></param>
        /// <param name="validationCreateRules"></param>
        /// <param name="validationModifyRules"></param>
        /// <param name="procProcessRouteRepository"></param>
        /// <param name="procProcessRouteNodeRepository"></param>
        /// <param name="procProcessRouteLinkRepository"></param>
        /// <param name="validationFlowDynamicLinkRules"></param>
        /// <param name="validationFlowDynamicNodeRules"></param>
        /// <param name="localizationService"></param>
        public ProcProcessRouteService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<ProcProcessRouteCreateDto> validationCreateRules,
            AbstractValidator<ProcProcessRouteModifyDto> validationModifyRules,
            IProcProcessRouteRepository procProcessRouteRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteLinkRepository,
            AbstractValidator<FlowDynamicLinkDto> validationFlowDynamicLinkRules,
            AbstractValidator<FlowDynamicNodeDto> validationFlowDynamicNodeRules,
            ILocalizationService localizationService)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procProcessRouteRepository = procProcessRouteRepository;
            _procProcessRouteNodeRepository = procProcessRouteNodeRepository;
            _procProcessRouteLinkRepository = procProcessRouteLinkRepository;
            _validationFlowDynamicLinkRules = validationFlowDynamicLinkRules;
            _validationFlowDynamicNodeRules = validationFlowDynamicNodeRules;
            _localizationService = localizationService;
        }


        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcessRoutePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcessRouteDto>> GetPageListAsync(ProcProcessRoutePagedQueryDto procProcessRoutePagedQueryDto)
        {
            var procProcessRoutePagedQuery = procProcessRoutePagedQueryDto.ToQuery<ProcProcessRoutePagedQuery>();
            procProcessRoutePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _procProcessRouteRepository.GetPagedInfoAsync(procProcessRoutePagedQuery);

            // 实体到DTO转换 装载数据
            var procProcessRouteDtos = pagedInfo.Data.Select(s => s.ToModel<ProcProcessRouteDto>());
            return new PagedInfo<ProcProcessRouteDto>(procProcessRouteDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CustomProcessRouteDto> GetCustomProcProcessRouteAsync(long id)
        {
            CustomProcessRouteDto model = new();
            var processRoute = await _procProcessRouteRepository.GetByIdAsync(id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10439));

            model.Info = processRoute.ToModel<ProcProcessRouteDto>();

            var nodes = await _procProcessRouteNodeRepository.GetListAsync(new ProcProcessRouteDetailNodeQuery { ProcessRouteId = id });
            nodes = nodes.OrderBy(x => x.ManualSortNumber);
            model.Nodes = nodes.Select(s =>
            {
                // 实体转换
                var nodeViewDto = s.ToModel<ProcProcessRouteDetailNodeViewDto>();
                nodeViewDto.ProcessType = s.Type;
                nodeViewDto.Code = ConvertProcedureCode(nodeViewDto.ProcedureId, nodeViewDto.Code);
                nodeViewDto.Name = ConvertProcedureName(nodeViewDto.ProcedureId, nodeViewDto.Name);
                return nodeViewDto;
            });

            var links = await _procProcessRouteLinkRepository.GetProcessRouteDetailLinksByProcessRouteIdAsync(id);
            model.Links = links.Select(s => s.ToModel<ProcProcessRouteDetailLinkDto>());

            return model;
        }

        /// <summary>
        /// 分页查询工艺路线的工序列表
        /// </summary>
        /// <param name="processRouteProcedureQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcedureDto>> GetPagedInfoByProcessRouteIdAsync(ProcessRouteProcedureQueryDto processRouteProcedureQueryDto)
        {
            var procProcedurePagedQuery = processRouteProcedureQueryDto.ToQuery<ProcessRouteProcedureQuery>();
            var pagedInfo = await _procProcessRouteNodeRepository.GetProcedureListByProcessRouteIdAsync(procProcedurePagedQuery);

            //实体到DTO转换 装载数据
            var procProcedureDtos = new List<ProcProcedureDto>();
            foreach (var procProcedureEntity in pagedInfo.Data)
            {
                var procProcedureDto = procProcedureEntity.ToModel<ProcProcedureDto>();
                procProcedureDtos.Add(procProcedureDto);
            }
            return new PagedInfo<ProcProcedureDto>(procProcedureDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 根据ID查询工艺路线工序列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDetailNodeViewDto>> GetNodesByRouteIdAsync(long id)
        {
            var nodeQuery = new ProcProcessRouteDetailNodeQuery { ProcessRouteId = id };
            var nodes = await _procProcessRouteNodeRepository.GetListAsync(nodeQuery);

            List<ProcProcessRouteDetailNodeViewDto> detailNodeViewDtos = new();
            foreach (var node in nodes)
            {
                // 实体转换
                var nodeViewDto = node.ToModel<ProcProcessRouteDetailNodeViewDto>();
                nodeViewDto.ProcessType = node.Type;
                nodeViewDto.Code = ConvertProcedureCode(nodeViewDto.ProcedureId, nodeViewDto.Code);
                nodeViewDto.Name = ConvertProcedureName(nodeViewDto.ProcedureId, nodeViewDto.Name);
                if (!string.IsNullOrWhiteSpace(nodeViewDto.Code)) detailNodeViewDtos.Add(nodeViewDto);
            }

            return detailNodeViewDtos;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procProcessRouteDto"></param>
        /// <returns></returns>
        public async Task<long> AddProcProcessRouteAsync(ProcProcessRouteCreateDto procProcessRouteDto)
        {
            #region 验证
            if (procProcessRouteDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procProcessRouteDto);

            // 验证工序集合
            if (procProcessRouteDto.DynamicData != null)
            {
                if (procProcessRouteDto.DynamicData.Links != null && procProcessRouteDto.DynamicData.Links.Any())
                {
                    foreach (var item in procProcessRouteDto.DynamicData.Links)
                    {
                        await _validationFlowDynamicLinkRules.ValidateAndThrowAsync(item);
                    }
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10454));
                }

                if (procProcessRouteDto.DynamicData.Nodes != null && procProcessRouteDto.DynamicData.Nodes.Any())
                {
                    foreach (var item in procProcessRouteDto.DynamicData.Nodes)
                    {
                        if (item.ProcedureId <= 0)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES10461));
                        }

                        if (item.ProcedureId != EndNodeId) //不是结束节点
                        {
                            await _validationFlowDynamicNodeRules.ValidateAndThrowAsync(item);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(item.ManualSortNumber))
                                throw new CustomerValidationException(nameof(ErrorCode.MES10474));
                            if (item.ManualSortNumber.Length > 18)
                                throw new CustomerValidationException(nameof(ErrorCode.MES10475));
                            if (string.IsNullOrEmpty(item.Extra1))
                                throw new CustomerValidationException(nameof(ErrorCode.MES10468));
                        }
                    }
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10455));
                }


            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10453));
            }

            procProcessRouteDto.Code = procProcessRouteDto.Code.ToTrimSpace().ToUpperInvariant();
            procProcessRouteDto.Name = procProcessRouteDto.Name.Trim();
            procProcessRouteDto.Remark = procProcessRouteDto.Remark.Trim();

            // DTO转换实体
            var procProcessRouteEntity = procProcessRouteDto.ToEntity<ProcProcessRouteEntity>();
            procProcessRouteEntity.Id = IdGenProvider.Instance.CreateId();
            procProcessRouteEntity.SiteId = _currentSite.SiteId ?? 0;
            procProcessRouteEntity.CreatedBy = _currentUser.UserName;
            procProcessRouteEntity.UpdatedBy = _currentUser.UserName;

            procProcessRouteEntity.Status = SysDataStatusEnum.Build;

            var links = ConvertProcessRouteLinkList(procProcessRouteDto.DynamicData.Links, procProcessRouteEntity);
            var nodes = ConvertProcessRouteNodeList(procProcessRouteDto.DynamicData.Nodes, links, procProcessRouteEntity);

            // 判断是否存在多个首工序
            var firstProcessCount = nodes.Count(w => w.IsFirstProcess == (int)YesOrNoEnum.Yes);
            if (firstProcessCount == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10435));
            if (firstProcessCount > 1) throw new CustomerValidationException(nameof(ErrorCode.MES10436));

            // 工艺路线编码和版本唯一
            var isExistsCode = await _procProcessRouteRepository.IsExistsAsync(new ProcProcessRouteQuery
            {
                SiteId = _currentSite.SiteId ?? 0,
                Code = procProcessRouteEntity.Code,
                Version = procProcessRouteEntity.Version
            });
            if (isExistsCode) throw new CustomerValidationException(nameof(ErrorCode.MES10437)).WithData("Code", procProcessRouteDto.Code).WithData("Version", procProcessRouteDto.Version);
            #endregion

            using TransactionScope trans = TransactionHelper.GetTransactionScope();

            // 只允许保存一个当前版本
            if (procProcessRouteEntity.IsCurrentVersion)
            {
                await _procProcessRouteRepository.ResetCurrentVersionAsync(new ResetCurrentVersionCommand
                {
                    SiteId = procProcessRouteEntity.SiteId,
                    Code = procProcessRouteDto.Code,
                    UpdatedOn = procProcessRouteEntity.UpdatedOn,
                    UpdatedBy = procProcessRouteEntity.UpdatedBy
                });
            }

            // 入库
            await _procProcessRouteRepository.InsertAsync(procProcessRouteEntity);

            if (nodes != null && nodes.Any()) await _procProcessRouteNodeRepository.InsertRangeAsync(nodes);
            if (links != null && links.Any()) await _procProcessRouteLinkRepository.InsertRangeAsync(links);

            trans.Complete();
            return procProcessRouteEntity.Id;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procProcessRouteDto"></param>
        /// <returns></returns>
        public async Task UpdateProcProcessRouteAsync(ProcProcessRouteModifyDto procProcessRouteDto)
        {
            #region 验证
            if (procProcessRouteDto == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            procProcessRouteDto.Name = procProcessRouteDto.Name.Trim();
            procProcessRouteDto.Remark = procProcessRouteDto.Remark.Trim();

            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procProcessRouteDto);

            //验证工序集合
            if (procProcessRouteDto.DynamicData != null)
            {
                if (procProcessRouteDto.DynamicData.Links != null && procProcessRouteDto.DynamicData.Links.Any())
                {
                    foreach (var item in procProcessRouteDto.DynamicData.Links)
                    {
                        await _validationFlowDynamicLinkRules.ValidateAndThrowAsync(item);
                    }
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10454));
                }

                if (procProcessRouteDto.DynamicData.Nodes != null && procProcessRouteDto.DynamicData.Nodes.Any())
                {
                    foreach (var item in procProcessRouteDto.DynamicData.Nodes)
                    {
                        if (item.ProcedureId <= 0)
                        {
                            throw new CustomerValidationException(nameof(ErrorCode.MES10461));
                        }

                        if (item.ProcedureId != EndNodeId) //不是结束节点
                        {
                            await _validationFlowDynamicNodeRules.ValidateAndThrowAsync(item);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(item.ManualSortNumber))
                                throw new CustomerValidationException(nameof(ErrorCode.MES10474));
                            if (item.ManualSortNumber.Length > 18)
                                throw new CustomerValidationException(nameof(ErrorCode.MES10475));
                            if (string.IsNullOrEmpty(item.Extra1))
                                throw new CustomerValidationException(nameof(ErrorCode.MES10468));
                        }
                    }
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10455));
                }


            }
            else
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10453));
            }

            // 判断是否存在
            var processRoute = await _procProcessRouteRepository.GetByIdAsync(procProcessRouteDto.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10438));

            // 验证某些状态是不能编辑的
            var canEditStatusEnum = new SysDataStatusEnum[] { SysDataStatusEnum.Build, SysDataStatusEnum.Retain };
            if (!canEditStatusEnum.Any(x => x == processRoute.Status))
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10129));
            }

            // DTO转换实体
            var procProcessRouteEntity = procProcessRouteDto.ToEntity<ProcProcessRouteEntity>();
            procProcessRouteEntity.UpdatedBy = _currentUser.UserName;
            procProcessRouteEntity.SiteId = processRoute.SiteId;

            var links = ConvertProcessRouteLinkList(procProcessRouteDto.DynamicData.Links, procProcessRouteEntity);
            var nodes = ConvertProcessRouteNodeList(procProcessRouteDto.DynamicData.Nodes, links, procProcessRouteEntity);

            // 判断是否存在多个首工序
            var firstProcessCount = nodes.Count(w => w.IsFirstProcess == (int)YesOrNoEnum.Yes);
            if (firstProcessCount == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10435));
            if (firstProcessCount > 1) throw new CustomerValidationException(nameof(ErrorCode.MES10436));

            // 工艺路线编码和版本唯一
            var isExistsCode = await _procProcessRouteRepository.IsExistsAsync(new ProcProcessRouteQuery
            {
                SiteId = procProcessRouteEntity.SiteId,
                Code = processRoute.Code,
                Version = processRoute.Version,
                Id = processRoute.Id,
            });
            if (isExistsCode) throw new CustomerValidationException(nameof(ErrorCode.MES10437)).WithData("Code", processRoute.Code).WithData("Version", processRoute.Version);
            #endregion

            // TODO 现在关联表批量删除批量新增，后面再修改
            using TransactionScope trans = TransactionHelper.GetTransactionScope();

            // 只允许保存一个当前版本
            if (procProcessRouteEntity.IsCurrentVersion)
            {
                // 取消其他记录为"非当前版本"
                await _procProcessRouteRepository.ResetCurrentVersionAsync(new ResetCurrentVersionCommand
                {
                    SiteId = processRoute.SiteId,
                    Code = processRoute.Code,
                    UpdatedOn = procProcessRouteEntity.UpdatedOn,
                    UpdatedBy = procProcessRouteEntity.UpdatedBy
                });
            }

            // 入库
            await _procProcessRouteRepository.UpdateAsync(procProcessRouteEntity);

            await _procProcessRouteNodeRepository.DeleteByProcessRouteIdAsync(procProcessRouteEntity.Id);
            if (nodes != null && nodes.Any()) await _procProcessRouteNodeRepository.InsertRangeAsync(nodes);

            await _procProcessRouteLinkRepository.DeleteByProcessRouteIdAsync(procProcessRouteEntity.Id);
            if (links != null && links.Any()) await _procProcessRouteLinkRepository.InsertRangeAsync(links);

            trans.Complete();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsAr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcProcessRouteAsync(long[] idsAr)
        {
            if (idsAr.Length < 1) throw new CustomerValidationException(nameof(ErrorCode.MES10102));

            #region 参数校验
            // 有生产中工单引用当前工艺路线，不能删除！
            // 状态（0:新建;1:启用;2:保留;3:废除）
            var resourceList = await _procProcessRouteRepository.IsIsExistsEnabledAsync(new ProcProcessRouteQuery
            {
                Ids = idsAr,
                StatusArr = new int[] { (int)SysDataStatusEnum.Enable, (int)SysDataStatusEnum.Retain, (int)SysDataStatusEnum.Abolish }
            });
            if (resourceList != null) throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            #endregion

            return await _procProcessRouteRepository.DeleteRangeAsync(new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsAr
            });
        }

        /// <summary>
        /// 根据不合个工艺路线Id查询不合格工艺路线列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProcProcessRouteDto>> GetListByIdsAsync(long[] ids)
        {
            var list = await _procProcessRouteRepository.GetByIdsAsync(ids);
            return list.Select(s => s.ToModel<ProcProcessRouteDto>());
        }


        #region 业务扩展方法
        /// <summary>
        /// 转换节点工序（编码）
        /// </summary>
        /// <param name="procedureId"></param>
        /// <param name="defaultCode"></param>
        /// <returns></returns>
        public static string ConvertProcedureCode(long procedureId, string defaultCode)
        {
            return procedureId switch
            {
                StartNodeId => "",
                EndNodeId => "",
                _ => defaultCode,
            };
        }

        /// <summary>
        /// 转换节点工序（名称）
        /// </summary>
        /// <param name="procedureId"></param>
        /// <param name="defaultName"></param>
        /// <returns></returns>
        public static string ConvertProcedureName(long procedureId, string defaultName)
        {
            return procedureId switch
            {
                StartNodeId => "",    // 开始
                EndNodeId => "",  // 结束
                _ => defaultName,
            };
        }

        /// <summary>
        /// 转换集合（工艺路线-连线）
        /// </summary>
        /// <param name="linkList"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<ProcProcessRouteDetailLinkEntity> ConvertProcessRouteLinkList(IEnumerable<FlowDynamicLinkDto> linkList, ProcProcessRouteEntity entity)
        {
            if (linkList == null || !linkList.Any()) return new List<ProcProcessRouteDetailLinkEntity> { };

            return linkList.Select(s => new ProcProcessRouteDetailLinkEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                SerialNo = s.SerialNo,
                ProcessRouteId = entity.Id,
                PreProcessRouteDetailId = s.PreProcessRouteDetailId,
                ProcessRouteDetailId = s.ProcessRouteDetailId,
                Extra1 = s.Extra1,
                CreatedBy = entity?.UpdatedBy ?? "",
                UpdatedBy = entity?.UpdatedBy ?? ""
            });
        }

        /// <summary>
        /// 转换集合（工艺路线-节点）
        /// </summary>
        /// <param name="nodeList"></param>
        /// <param name="links"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<ProcProcessRouteDetailNodeEntity> ConvertProcessRouteNodeList(IEnumerable<FlowDynamicNodeDto> nodeList, IEnumerable<ProcProcessRouteDetailLinkEntity> links, ProcProcessRouteEntity entity)
        {
            List<ProcProcessRouteDetailNodeEntity> list = new();

            if (entity == null) return list;
            if (nodeList == null || !nodeList.Any()) return list;

            // 判断是否有重复工序
            var procedureIds = nodeList.Select(s => s.ProcedureId).Distinct();
            if (procedureIds.Count() < nodeList.Count()) throw new CustomerValidationException(nameof(ErrorCode.MES10449));

            var saveNodes = nodeList.Select(s => new ProcProcessRouteDetailNodeEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                ProcessRouteId = entity.Id,
                //SerialNo = "",
                ManualSortNumber = s.ManualSortNumber,
                ProcedureId = s.ProcedureId,
                CheckType = s.CheckType,
                CheckRate = s.CheckRate,
                IsWorkReport = s.IsWorkReport,
                IsFirstProcess = s.IsFirstProcess,
                Extra1 = s.Extra1,
                CreatedBy = entity.UpdatedBy ?? "",
                UpdatedBy = entity.UpdatedBy
            });

            // 排序之后的节点集合
            List<ProcProcessRouteDetailNodeEntity> newNodes = new();

            // 先移除尾工序的干扰
            var nodesWithEndNode = saveNodes.Where(w => w.ProcedureId != EndNodeId);

            // 开始排序
            SortNodes(ref newNodes, nodesWithEndNode, links, Array.Empty<ProcProcessRouteDetailNodeEntity>());

            // 重新排序
            newNodes.ForEach(f => f.SerialNo = newNodes.IndexOf(f) + 1);

            // 补回尾工序
            var lastNode = saveNodes.FirstOrDefault(w => w.ProcedureId == EndNodeId);
            if (lastNode != null)
            {
                lastNode.SerialNo = newNodes.Count + 1;
                newNodes.Add(lastNode);
            }

            return newNodes;
        }

        /// <summary>
        /// 对节点进行排序
        /// </summary>
        /// <param name="nodesOfSorted"></param>
        /// <param name="allNodes"></param>
        /// <param name="allLinks"></param>
        /// <param name="childNodes"></param>
        public static void SortNodes(ref List<ProcProcessRouteDetailNodeEntity> nodesOfSorted,
            IEnumerable<ProcProcessRouteDetailNodeEntity> allNodes,
            IEnumerable<ProcProcessRouteDetailLinkEntity> allLinks,
            IEnumerable<ProcProcessRouteDetailNodeEntity> childNodes)
        {
            var targetNodes = childNodes;
            if (!nodesOfSorted.Any())
            {
                // 首工序
                var firstNode = allNodes.FirstOrDefault(f => f.IsFirstProcess == 1);
                if (firstNode == null) return;

                targetNodes = new List<ProcProcessRouteDetailNodeEntity> { firstNode };
            }

            List<ProcProcessRouteDetailNodeEntity> nextNodes = new();
            foreach (var node in targetNodes)
            {
                node.SerialNo = nodesOfSorted.Count + 1;

                nodesOfSorted.RemoveAll(f => f.ProcedureId == node.ProcedureId);
                nodesOfSorted.Add(node);

                // 当前节点的直属下级连线
                var linksTemp = allLinks.Where(w => w.PreProcessRouteDetailId == node.ProcedureId);
                foreach (var link in linksTemp)
                {
                    var nodeTemp = allNodes.FirstOrDefault(f => f.ProcedureId == link.ProcessRouteDetailId);
                    if (nodeTemp != null) nextNodes.Add(nodeTemp);
                }
            }

            if (!nextNodes.Any()) return;
            if (nodesOfSorted.Count >= allNodes.Count()) return;

            SortNodes(ref nodesOfSorted, allNodes, allLinks, nextNodes);
        }
        #endregion

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
            var entity = await _procProcessRouteRepository.GetByIdAsync(changeStatusCommand.Id);
            if (entity == null || entity.IsDeleted != 0)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10439));
            }
            if (entity.Status == changeStatusCommand.Status)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10127)).WithData("status", _localizationService.GetResource($"{typeof(SysDataStatusEnum).FullName}.{Enum.GetName(typeof(SysDataStatusEnum), entity.Status)}"));
            }
            #endregion

            #region 操作数据库
            await _procProcessRouteRepository.UpdateStatusAsync(changeStatusCommand);
            #endregion
        }

        #endregion
    }
}
