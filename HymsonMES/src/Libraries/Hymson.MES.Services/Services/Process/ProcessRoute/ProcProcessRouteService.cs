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
using Hymson.MES.Data.Repositories.Process.ProcessRoute.Command;
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
        public ProcProcessRouteService(ICurrentUser currentUser, ICurrentSite currentSite,
            AbstractValidator<ProcProcessRouteCreateDto> validationCreateRules,
            AbstractValidator<ProcProcessRouteModifyDto> validationModifyRules,
            IProcProcessRouteRepository procProcessRouteRepository,
            IProcProcessRouteDetailNodeRepository procProcessRouteNodeRepository,
            IProcProcessRouteDetailLinkRepository procProcessRouteLinkRepository, AbstractValidator<FlowDynamicLinkDto> validationFlowDynamicLinkRules, AbstractValidator<FlowDynamicNodeDto> validationFlowDynamicNodeRules)
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
        }


        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procProcessRoutePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcProcessRouteDto>> GetPageListAsync(ProcProcessRoutePagedQueryDto procProcessRoutePagedQueryDto)
        {
            var procProcessRoutePagedQuery = procProcessRoutePagedQueryDto.ToQuery<ProcProcessRoutePagedQuery>();
            procProcessRoutePagedQuery.SiteId = _currentSite.SiteId ?? 123456;
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

            var links = await _procProcessRouteLinkRepository.GetListAsync(new ProcProcessRouteDetailLinkQuery { ProcessRouteId = id });
            model.Links = links.Select(s => s.ToModel<ProcProcessRouteDetailLinkDto>());

            return model;
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
                if (string.IsNullOrWhiteSpace(nodeViewDto.Code) == false) detailNodeViewDtos.Add(nodeViewDto);
            }

            return detailNodeViewDtos;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task AddProcProcessRouteAsync(ProcProcessRouteCreateDto parm)
        {
            #region 验证
            if (parm == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            // 验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(parm);

            //验证工序集合
            if (parm.DynamicData != null)
            {
                if (parm.DynamicData.Links != null && parm.DynamicData.Links.Any())
                {
                    foreach (var item in parm.DynamicData.Links)
                    {
                        await _validationFlowDynamicLinkRules.ValidateAndThrowAsync(item);
                    }
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10454));
                }

                if (parm.DynamicData.Nodes != null && parm.DynamicData.Nodes.Any())
                {
                    foreach (var item in parm.DynamicData.Nodes)
                    {
                        if (item.ProcedureId<=0) 
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
                            if(string.IsNullOrEmpty(item.Extra1))
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

            parm.Code = parm.Code.ToTrimSpace().ToUpperInvariant();
            parm.Name = parm.Name.Trim();
            parm.Remark = parm.Remark.Trim();

            // DTO转换实体
            var procProcessRouteEntity = parm.ToEntity<ProcProcessRouteEntity>();
            procProcessRouteEntity.Id = IdGenProvider.Instance.CreateId();
            procProcessRouteEntity.SiteId = _currentSite.SiteId ?? 123456;
            procProcessRouteEntity.CreatedBy = _currentUser.UserName;
            procProcessRouteEntity.UpdatedBy = _currentUser.UserName;

            var links = ConvertProcessRouteLinkList(parm.DynamicData.Links, procProcessRouteEntity);
            var nodes = ConvertProcessRouteNodeList(parm.DynamicData.Nodes, links, procProcessRouteEntity);

            // 判断是否存在多个首工序
            var firstProcessCount = nodes.Where(w => w.IsFirstProcess == (int)YesOrNoEnum.Yes).Count();
            if (firstProcessCount == 0) throw new CustomerValidationException(nameof(ErrorCode.MES10435));
            if (firstProcessCount > 1) throw new CustomerValidationException(nameof(ErrorCode.MES10436));

            // 工艺路线编码和版本唯一
            var isExistsCode = await _procProcessRouteRepository.IsExistsAsync(new ProcProcessRouteQuery
            {
                SiteId = _currentSite.SiteId ?? 123456,
                Code = procProcessRouteEntity.Code,
                Version = procProcessRouteEntity.Version
            });
            if (isExistsCode == true) throw new CustomerValidationException(nameof(ErrorCode.MES10437)).WithData("Code", parm.Code).WithData("Version", parm.Version);
            #endregion

            using TransactionScope trans = TransactionHelper.GetTransactionScope();

            // 只允许保存一个当前版本
            if (procProcessRouteEntity.IsCurrentVersion == 1)
            {
                await _procProcessRouteRepository.ResetCurrentVersionAsync(new ResetCurrentVersionCommand
                {
                    SiteId = procProcessRouteEntity.SiteId,
                    UpdatedOn = procProcessRouteEntity.UpdatedOn,
                    UpdatedBy = procProcessRouteEntity.UpdatedBy
                });
            }

            // 入库
            await _procProcessRouteRepository.InsertAsync(procProcessRouteEntity);

            if (nodes != null && nodes.Any() == true) await _procProcessRouteNodeRepository.InsertRangeAsync(nodes);
            if (links != null && links.Any() == true) await _procProcessRouteLinkRepository.InsertRangeAsync(links);

            trans.Complete();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public async Task UpdateProcProcessRouteAsync(ProcProcessRouteModifyDto parm)
        {
            #region 验证
            if (parm == null) throw new CustomerValidationException(nameof(ErrorCode.MES10100));

            parm.Name = parm.Name.Trim();
            parm.Remark = parm.Remark.Trim();

            // 验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(parm);

            //验证工序集合
            if (parm.DynamicData != null)
            {
                if (parm.DynamicData.Links != null && parm.DynamicData.Links.Any())
                {
                    foreach (var item in parm.DynamicData.Links)
                    {
                        await _validationFlowDynamicLinkRules.ValidateAndThrowAsync(item);
                    }
                }
                else
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES10454));
                }

                if (parm.DynamicData.Nodes != null && parm.DynamicData.Nodes.Any())
                {
                    foreach (var item in parm.DynamicData.Nodes)
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
            var processRoute = await _procProcessRouteRepository.GetByIdAsync(parm.Id)
                ?? throw new CustomerValidationException(nameof(ErrorCode.MES10438));
            if (processRoute.Status != SysDataStatusEnum.Build && parm.Status == SysDataStatusEnum.Build)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10108));
            }

            // DTO转换实体
            var procProcessRouteEntity = parm.ToEntity<ProcProcessRouteEntity>();
            procProcessRouteEntity.UpdatedBy = _currentUser.UserName;

            var links = ConvertProcessRouteLinkList(parm.DynamicData.Links, procProcessRouteEntity);
            var nodes = ConvertProcessRouteNodeList(parm.DynamicData.Nodes, links, procProcessRouteEntity);

            // 判断是否存在多个首工序
            var firstProcessCount = nodes.Where(w => w.IsFirstProcess == (int)YesOrNoEnum.Yes).Count();
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
            if (isExistsCode == true) throw new CustomerValidationException(nameof(ErrorCode.MES10437)).WithData("Code", processRoute.Code).WithData("Version", processRoute.Version);
            #endregion

            // TODO 现在关联表批量删除批量新增，后面再修改
            using TransactionScope trans = TransactionHelper.GetTransactionScope();

            // 只允许保存一个当前版本
            if (procProcessRouteEntity.IsCurrentVersion == 1)
            {
                // 取消其他记录为"非当前版本"
                await _procProcessRouteRepository.ResetCurrentVersionAsync(new ResetCurrentVersionCommand
                {
                    SiteId = processRoute.SiteId,
                    UpdatedOn = procProcessRouteEntity.UpdatedOn,
                    UpdatedBy = procProcessRouteEntity.UpdatedBy
                });
            }

            // 入库
            await _procProcessRouteRepository.UpdateAsync(procProcessRouteEntity);

            await _procProcessRouteNodeRepository.DeleteByProcessRouteIdAsync(procProcessRouteEntity.Id);
            if (nodes != null && nodes.Any() == true) await _procProcessRouteNodeRepository.InsertRangeAsync(nodes);

            await _procProcessRouteLinkRepository.DeleteByProcessRouteIdAsync(procProcessRouteEntity.Id);
            if (links != null && links.Any() == true) await _procProcessRouteLinkRepository.InsertRangeAsync(links);

            trans.Complete();
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeleteProcProcessRouteAsync(long[] idsArr)
        {
            if (idsArr.Length < 1) throw new CustomerValidationException(nameof(ErrorCode.MES10102));

            #region 参数校验
            // 有生产中工单引用当前工艺路线，不能删除！
            // 状态（0:新建;1:启用;2:保留;3:废除）
            var resourceList = await _procProcessRouteRepository.IsIsExistsEnabledAsync(new ProcProcessRouteQuery
            {
                Ids = idsArr,
                StatusArr = new int[] { (int)SysDataStatusEnum.Enable, (int)SysDataStatusEnum.Retain, (int)SysDataStatusEnum.Abolish }
            });
            if (resourceList != null) throw new CustomerValidationException(nameof(ErrorCode.MES10106));
            #endregion

            return await _procProcessRouteRepository.DeleteRangeAsync(new DeleteCommand
            {
                UserId = _currentUser.UserName,
                DeleteOn = HymsonClock.Now(),
                Ids = idsArr
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
            if (linkList == null || linkList.Any() == false) return new List<ProcProcessRouteDetailLinkEntity> { };

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
            if (nodeList == null || nodeList.Any() == false) return list;

            // 判断是否有重复工序
            var procedureIds = nodeList.Select(s => s.ProcedureId).Distinct();
            if (procedureIds.Count() < nodeList.Count()) throw new CustomerValidationException(nameof(ErrorCode.MES10449));

            var saveNodes = nodeList.Select(s => new ProcProcessRouteDetailNodeEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                ProcessRouteId = entity.Id,
                SerialNo = "",
                ManualSortNumber=s.ManualSortNumber,
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
            newNodes.ForEach(f => f.SerialNo = $"{newNodes.IndexOf(f) + 1}");

            // 补回尾工序
            var lastNode = saveNodes.FirstOrDefault(w => w.ProcedureId == EndNodeId);
            if (lastNode != null)
            {
                lastNode.SerialNo = $"{newNodes.Count + 1}";
                newNodes.Add(lastNode);
            }

            return newNodes;
        }

        /// <summary>
        /// 对节点进行排序
        /// </summary>
        /// <param name="nodesOfSorted"></param>
        /// <param name="childNodes"></param>
        /// <param name="allNodes"></param>
        /// <param name="allLinks"></param>
        public static void SortNodes(ref List<ProcProcessRouteDetailNodeEntity> nodesOfSorted,
            IEnumerable<ProcProcessRouteDetailNodeEntity> allNodes,
            IEnumerable<ProcProcessRouteDetailLinkEntity> allLinks,
            IEnumerable<ProcProcessRouteDetailNodeEntity> childNodes)
        {
            var targetNodes = childNodes;
            if (nodesOfSorted.Any() == false)
            {
                // 首工序
                var firstNode = allNodes.FirstOrDefault(f => f.IsFirstProcess == 1);
                if (firstNode == null) return;

                targetNodes = new List<ProcProcessRouteDetailNodeEntity> { firstNode };
            }

            childNodes = UpdateNodesSortAndGetChildNodes(ref nodesOfSorted, allNodes, allLinks, targetNodes);
            if (childNodes.Any() == false) return;
            if (nodesOfSorted.Count() >= allNodes.Count()) return;

            SortNodes(ref nodesOfSorted, allNodes, allLinks, childNodes);
        }

        /// <summary>
        /// 更新节点排序号并获取子节点
        /// </summary>
        /// <param name="nodesOfSorted"></param>
        /// <param name="nodes"></param>
        /// <param name="allNodes"></param>
        /// <param name="allLinks"></param>
        public static IEnumerable<ProcProcessRouteDetailNodeEntity> UpdateNodesSortAndGetChildNodes(ref List<ProcProcessRouteDetailNodeEntity> nodesOfSorted,
            IEnumerable<ProcProcessRouteDetailNodeEntity> allNodes,
            IEnumerable<ProcProcessRouteDetailLinkEntity> allLinks,
            IEnumerable<ProcProcessRouteDetailNodeEntity> nodes)
        {
            List<ProcProcessRouteDetailNodeEntity> childNodes = new();
            foreach (var node in nodes)
            {
                node.SerialNo = $"{nodesOfSorted.Count + 1}";

                nodesOfSorted.RemoveAll(f => f.ProcedureId == node.ProcedureId);
                nodesOfSorted.Add(node);

                // 当前节点的所有下级节点
                childNodes.AddRange(GetChildNodes(node, allNodes, allLinks));
            }
            return childNodes;
        }

        /// <summary>
        /// 获取某节点的子节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodes"></param>
        /// <param name="links"></param>
        /// <returns></returns>
        public static IEnumerable<ProcProcessRouteDetailNodeEntity> GetChildNodes(ProcProcessRouteDetailNodeEntity node,
            IEnumerable<ProcProcessRouteDetailNodeEntity> nodes,
            IEnumerable<ProcProcessRouteDetailLinkEntity> links)
        {
            // 当前节点的直属下级连线
            links = links.Where(w => w.PreProcessRouteDetailId == node.ProcedureId);
            return nodes.Where(w => links.Select(s => s.ProcessRouteDetailId).Contains(w.ProcedureId));
        }
        #endregion

    }
}
