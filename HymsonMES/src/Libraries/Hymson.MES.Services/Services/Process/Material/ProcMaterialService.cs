/*
 *creator: Karl
 *
 *describe: 物料维护    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-08 04:47:44
 */
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
using Hymson.MES.Services.Dtos.Process;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils;
using Org.BouncyCastle.Crypto;
using System.Security.Policy;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// 物料维护 服务
    /// </summary>
    public class ProcMaterialService : IProcMaterialService
    {
        /// <summary>
        /// 物料维护 仓储
        /// </summary>
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly AbstractValidator<ProcMaterialCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcMaterialModifyDto> _validationModifyRules;

        private readonly IProcReplaceMaterialRepository _procReplaceMaterialRepository;

        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        public ProcMaterialService(ICurrentUser currentUser, IProcMaterialRepository procMaterialRepository, AbstractValidator<ProcMaterialCreateDto> validationCreateRules, AbstractValidator<ProcMaterialModifyDto> validationModifyRules, IProcReplaceMaterialRepository procReplaceMaterialRepository, ICurrentSite currentSite)
        {
            _currentUser = currentUser;
            _procMaterialRepository = procMaterialRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;

            _procReplaceMaterialRepository = procReplaceMaterialRepository;
            _currentSite = currentSite;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procMaterialDto"></param>
        /// <returns></returns>
        public async Task CreateProcMaterialAsync(ProcMaterialCreateDto procMaterialCreateDto)
        {
            #region 参数校验
            //// 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                //responseDto.Msg = "站点码获取失败，请重新登录！";
                //return responseDto;
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procMaterialCreateDto);

            procMaterialCreateDto.MaterialCode = procMaterialCreateDto.MaterialCode.ToUpper();
            procMaterialCreateDto.Origin = MaterialOriginEnum.ManualEntry; // ERP/EIS（sys_source_type）

            //判断编号是否已存在
            var haveEntity = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery()
            {
                SiteId= _currentSite.SiteId,
                MaterialCode = procMaterialCreateDto.MaterialCode,
                Version = procMaterialCreateDto.Version
            });
            //存在则抛异常
            if (haveEntity != null && haveEntity.Count() > 0)
            {
                throw new CustomerValidationException( nameof(ErrorCode.MES10201) ).WithData("materialCode", procMaterialCreateDto.MaterialCode).WithData("version", procMaterialCreateDto.Version);
            }
            #endregion

            #region 组装数据
            //DTO转换实体
            var procMaterialEntity = procMaterialCreateDto.ToEntity<ProcMaterialEntity>();
            procMaterialEntity.Id= IdGenProvider.Instance.CreateId();
            procMaterialEntity.CreatedBy = _currentUser.UserName;
            procMaterialEntity.UpdatedBy = _currentUser.UserName;
            procMaterialEntity.CreatedOn = HymsonClock.Now();
            procMaterialEntity.UpdatedOn = HymsonClock.Now();
            procMaterialEntity.SiteId = _currentSite.SiteId??0;

            //替代品数据
            List<ProcReplaceMaterialEntity> addProcReplaceList = new List<ProcReplaceMaterialEntity>();
            if (procMaterialCreateDto.DynamicList != null && procMaterialCreateDto.DynamicList.Count > 0)
            {
                foreach (var item in procMaterialCreateDto.DynamicList)
                {
                    ProcReplaceMaterialEntity procReplaceMaterial=item.ToEntity<ProcReplaceMaterialEntity>();
                    procReplaceMaterial.Id= IdGenProvider.Instance.CreateId();
                    procReplaceMaterial.IsUse = item.IsEnabled;
                    procReplaceMaterial.ReplaceMaterialId = (long)item.MaterialId;
                    procReplaceMaterial.MaterialId = procMaterialEntity.Id;
                    procReplaceMaterial.CreatedBy = _currentUser.UserName;
                    procReplaceMaterial.CreatedOn = HymsonClock.Now();
                    procReplaceMaterial.SiteId = _currentSite.SiteId ?? 0;
                    addProcReplaceList.Add(procReplaceMaterial);
                }
            }
            #endregion

            #region 保存到数据库
            using (TransactionScope ts = new TransactionScope())
            {
                var response = 0;

                //入库
                response = await _procMaterialRepository.InsertAsync(procMaterialEntity);

                if (response == 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10202));
                }

                if (procMaterialCreateDto.DynamicList != null && procMaterialCreateDto.DynamicList.Count > 0)
                {
                    response = await _procReplaceMaterialRepository.InsertsAsync(addProcReplaceList);

                    if (response != procMaterialCreateDto.DynamicList.Count)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10202));
                    }
                }

                ts.Complete();
            }

            #endregion

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteProcMaterialAsync(long id)
        {
            await _procMaterialRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="idsArr"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcMaterialAsync(long[] idsArr)
        {
            #region 参数校验
            if (idsArr.Length < 1)
            {
                throw new ValidationException(nameof(ErrorCode.MES10213));
            }

            //var statusArr = new int[] { 2, 3 }; //可下达和保留 时无法删除
            //判断这些ID 对应的物料是否在 可下达和保留中  （1:新建;2:可下达;3:保留;4:废除）
            var entitys =  await _procMaterialRepository.GetByIdsAsync(idsArr);
            if (entitys.Where(x => (SysDataStatusEnum.Enable|SysDataStatusEnum.Retain).HasFlag(x.Status)).ToList().Count>0) 
            {
                throw new BusinessException(nameof(ErrorCode.MES10212));
            }

            #endregion

            return await _procMaterialRepository.DeletesAsync(new DeleteCommand { Ids = idsArr, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procMaterialPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialDto>> GetPageListAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto)
        {
            procMaterialPagedQueryDto.SiteId = _currentSite.SiteId ?? 0;

            var procMaterialPagedQuery = procMaterialPagedQueryDto.ToQuery<ProcMaterialPagedQuery>();
            var pagedInfo = await _procMaterialRepository.GetPagedInfoAsync(procMaterialPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcMaterialDto> procMaterialDtos = PrepareProcMaterialDtos(pagedInfo);
            return new PagedInfo<ProcMaterialDto>(procMaterialDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcMaterialDto> PrepareProcMaterialDtos(PagedInfo<ProcMaterialEntity>   pagedInfo)
        {
            var procMaterialDtos = new List<ProcMaterialDto>();
            foreach (var procMaterialEntity in pagedInfo.Data)
            {
                var procMaterialDto = procMaterialEntity.ToModel<ProcMaterialDto>();
                procMaterialDtos.Add(procMaterialDto);
            }

            return procMaterialDtos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="procMaterialPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcMaterialDto>> GetPageListForGroupAsync(ProcMaterialPagedQueryDto procMaterialPagedQueryDto) 
        {
            procMaterialPagedQueryDto.SiteId = _currentSite.SiteId ?? 0;
            var procMaterialPagedQuery = procMaterialPagedQueryDto.ToQuery<ProcMaterialPagedQuery>();
            var pagedInfo = await _procMaterialRepository.GetPagedInfoForGroupAsync(procMaterialPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcMaterialDto> procMaterialDtos = PrepareProcMaterialDtos(pagedInfo);
            return new PagedInfo<ProcMaterialDto>(procMaterialDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procMaterialDto"></param>
        /// <returns></returns>
        public async Task ModifyProcMaterialAsync(ProcMaterialModifyDto procMaterialModifyDto)
        {
            //DTO转换实体
            var procMaterialEntity = procMaterialModifyDto.ToEntity<ProcMaterialEntity>();
            procMaterialEntity.UpdatedBy = _currentUser.UserName;
            procMaterialEntity.UpdatedOn = HymsonClock.Now();

            #region 校验
            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procMaterialModifyDto);

            var modelOrigin = await _procMaterialRepository.GetByIdAsync(procMaterialModifyDto.Id, _currentSite.SiteId??0);
            if (modelOrigin == null)
            {
                throw new NotFoundException(nameof(ErrorCode.MES10204));
            }

            if (procMaterialModifyDto.Origin != modelOrigin.Origin)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES10205));
            }

            // 判断替代品是否包含当前物料
            var replaceMaterialList = ConvertProcReplaceMaterialList(procMaterialModifyDto.DynamicList, procMaterialEntity);
            if (replaceMaterialList.Any(a => a.ReplaceMaterialId == procMaterialEntity.Id) == true)
            {
                throw new BusinessException(nameof(ErrorCode.MES10206));
            }

            // 判断编号是否已存在
            var existsList = await _procMaterialRepository.GetProcMaterialEntitiesAsync(new ProcMaterialQuery()
            {
                SiteId = _currentSite.SiteId,
                MaterialCode = procMaterialEntity.MaterialCode,
                Version = procMaterialEntity.Version
            });
            if (existsList != null && existsList.Where(x => x.Id != procMaterialEntity.Id).Any()) 
            {
                throw new BusinessException(nameof(ErrorCode.MES10201)).WithData("materialCode", procMaterialEntity.MaterialCode).WithData("version", procMaterialEntity.Version);
            }

            #endregion

            #region 组装数据
            //替代品数据
            List<ProcReplaceMaterialEntity> addProcReplaceList = new List<ProcReplaceMaterialEntity>();
            List<ProcReplaceMaterialEntity> updateProcReplaceList = new List<ProcReplaceMaterialEntity>();
            List<long> deleteeProcReplaceIds = new List<long>();
            if (procMaterialModifyDto.DynamicList != null && procMaterialModifyDto.DynamicList.Count() > 0)
            {
                foreach (var item in procMaterialModifyDto.DynamicList)
                {
                    ProcReplaceMaterialEntity procReplaceMaterial = new ProcReplaceMaterialEntity();
                    //switch (item.OperationType)
                    //{
                        //TODO  不管是新增，更新，删除    都是先真删除之后再新增
                        //case OperateTypeEnum.Add:
                        //    procReplaceMaterial = item.ToEntity<ProcReplaceMaterialEntity>();
                        //    procReplaceMaterial.Id= IdGenProvider.Instance.CreateId();
                        //    procReplaceMaterial.CreatedBy = _currentUser.UserName;
                        //    procReplaceMaterial.CreatedOn = HymsonClock.Now();
                        //    procReplaceMaterial.IsUse = item.IsEnabled;
                        //    procReplaceMaterial.ReplaceMaterialId = (long)item.MaterialId;
                        //    procReplaceMaterial.MaterialId = procMaterialEntity.Id;
                        //    procReplaceMaterial.SiteId = _currentSite.SiteId??0;
                        //    addProcReplaceList.Add(procReplaceMaterial);
                        //    break;
                        //case OperateTypeEnum.Edit:
                        //    procReplaceMaterial = item.ToEntity<ProcReplaceMaterialEntity>();  //item.Adapt<ProcReplaceMaterial>().ToUpdate();
                        //    procReplaceMaterial.UpdatedBy = _currentUser.UserName;
                        //    procReplaceMaterial.UpdatedOn = HymsonClock.Now();
                        //    if (item.Id == item.MaterialId)
                        //    {
                        //        var modelReplaceMaterial = await _procReplaceMaterialRepository.GetByIdAsync(item.Id);
                        //        if (modelReplaceMaterial != null)
                        //        {
                        //            procReplaceMaterial.ReplaceMaterialId = modelReplaceMaterial.ReplaceMaterialId;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        procReplaceMaterial.ReplaceMaterialId = (long)item.MaterialId;
                        //    }
                        //    procReplaceMaterial.IsUse = item.IsEnabled;
                        //    procReplaceMaterial.MaterialId = procMaterialEntity.Id;
                        //    updateProcReplaceList.Add(procReplaceMaterial);
                        //    break;
                        //case 3:
                        //    if (item.MaterialId != null && item.MaterialId > 0)
                        //    {
                        //        deleteeProcReplaceIds.Add(item.MaterialId ?? 0);
                        //    }
                        //    break;
                    //}
                }
            }
            #endregion 

            #region 操作数据库
            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                if (procMaterialEntity.IsDefaultVersion == true)
                {
                    // 先将同编码的其他物料设置为非当前版本
                    await _procMaterialRepository.UpdateSameMaterialCodeToNoVersionAsync(new ProcMaterialEntity()
                    {
                        MaterialCode = procMaterialEntity.MaterialCode,
                    });
                }

                response = await _procMaterialRepository.UpdateAsync(new ProcMaterialEntity()
                {
                    Id= procMaterialEntity.Id,
                    GroupId = procMaterialEntity.GroupId,
                    MaterialName = procMaterialEntity.MaterialName,
                    Status = procMaterialEntity.Status,
                    Origin = procMaterialEntity.Origin,
                    Version = procMaterialEntity.Version,
                    Remark = procMaterialEntity.Remark,
                    BuyType = procMaterialEntity.BuyType,
                    ProcessRouteId = procMaterialEntity.ProcessRouteId,
                    ProcedureBomId = procMaterialEntity.ProcedureBomId,
                    Batch = procMaterialEntity.Batch,
                    Unit = procMaterialEntity.Unit,
                    SerialNumber = procMaterialEntity.SerialNumber,
                    BaseTime = procMaterialEntity.BaseTime,
                    ConsumptionTolerance = procMaterialEntity.ConsumptionTolerance,
                    IsDefaultVersion = procMaterialEntity.IsDefaultVersion,
                    ValidationMaskGroup = procMaterialEntity.ValidationMaskGroup,
                    UpdatedBy = procMaterialEntity.UpdatedBy,
                    UpdatedOn = procMaterialEntity.UpdatedOn,
                });

                if (response == 0)
                {
                    throw new BusinessException(nameof(ErrorCode.MES10208));
                }

                //替代组设置数据
                if (addProcReplaceList != null && addProcReplaceList.Count > 0)
                {
                    if ((await _procReplaceMaterialRepository.InsertsAsync(addProcReplaceList))<=0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10209));
                    }
                }

                if (updateProcReplaceList != null && updateProcReplaceList.Count > 0)
                {
                    if ((await _procReplaceMaterialRepository.UpdatesAsync(updateProcReplaceList))<=0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10210));
                    }
                }

                if (deleteeProcReplaceIds != null && deleteeProcReplaceIds.Count > 0)
                {
                    if (await _procReplaceMaterialRepository.DeletesAsync(new DeleteCommand { Ids = deleteeProcReplaceIds.ToArray(), DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName }) <= 0)
                    {
                        throw new BusinessException(nameof(ErrorCode.MES10211));
                    }
                }

                ts.Complete();
            }


            #endregion

            //await _procMaterialRepository.UpdateAsync(procMaterialEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcMaterialViewDto> QueryProcMaterialByIdAsync(long id) 
        {
            var procMaterialView = await _procMaterialRepository.GetByIdAsync(id, _currentSite.SiteId??0);   
            if (procMaterialView != null) 
           {
               return procMaterialView.ToModel<ProcMaterialViewDto>();
           }
            return null;
        }

        #region 业务扩展方法
        /// <summary>
        /// 转换集合（物料替代品）
        /// </summary>
        /// <param name="dynamicList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static List<ProcReplaceMaterialEntity> ConvertProcReplaceMaterialList(IEnumerable<ProcMaterialReplaceDto> dynamicList, ProcMaterialEntity model)
        {
            if (dynamicList == null || dynamicList.Any() == false) return new List<ProcReplaceMaterialEntity> { };
            return dynamicList.Select(s => new ProcReplaceMaterialEntity
            {
                MaterialId = model.Id,
                ReplaceMaterialId = (long)s.MaterialId,
                IsUse = s.IsEnabled,
                CreatedBy = model.UpdatedBy,
                CreatedOn = model.UpdatedOn ?? HymsonClock.Now()
            }).ToList();
        }
        #endregion
    }
}
