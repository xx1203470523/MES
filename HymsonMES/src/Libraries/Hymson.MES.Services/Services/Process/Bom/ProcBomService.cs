/*
 *creator: Karl
 *
 *describe: BOM表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-02-14 10:04:25
 */
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
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
using Hymson.Utils;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace Hymson.MES.Services.Services.Process
{
    /// <summary>
    /// BOM表 服务
    /// </summary>
    public class ProcBomService : IProcBomService
    {
        /// <summary>
        /// BOM表 仓储
        /// </summary>
        private readonly IProcBomRepository _procBomRepository;
        private readonly AbstractValidator<ProcBomCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ProcBomModifyDto> _validationModifyRules;

        private readonly IProcBomDetailRepository _procBomDetailRepository;
        private readonly IProcBomDetailReplaceMaterialRepository _procBomDetailReplaceMaterialRepository;

        private readonly ICurrentUser _currentUser;

        public ProcBomService(ICurrentUser currentUser,IProcBomRepository procBomRepository, AbstractValidator<ProcBomCreateDto> validationCreateRules, AbstractValidator<ProcBomModifyDto> validationModifyRules, IProcBomDetailRepository procBomDetailRepository, IProcBomDetailReplaceMaterialRepository procBomDetailReplaceMaterialRepository)
        {
            _currentUser = currentUser;
            _procBomRepository = procBomRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _procBomDetailRepository = procBomDetailRepository;
            _procBomDetailReplaceMaterialRepository = procBomDetailReplaceMaterialRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="procBomDto"></param>
        /// <returns></returns>
        public async Task CreateProcBomAsync(ProcBomCreateDto procBomCreateDto)
        {
            if (procBomCreateDto == null) 
            {
                throw new ValidationException(ErrorCode.MES10503);
            }

            var list = procBomCreateDto.MaterialList.ToList();
            list.ForEach(a =>
            {
                if (string.IsNullOrWhiteSpace(a.ProcedureBomId))
                {
                    a.ProcedureBomId = "0";
                }
            });

            procBomCreateDto.SiteCode = "";//TODO App.GetSite();

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(procBomCreateDto);

            //DTO转换实体
            var procBomEntity = procBomCreateDto.ToEntity<ProcBomEntity>();
            procBomEntity.Id= IdGenProvider.Instance.CreateId();
            procBomEntity.CreatedBy = _currentUser.UserName;
            procBomEntity.UpdatedBy = _currentUser.UserName;
            procBomEntity.CreatedOn = HymsonClock.Now();
            procBomEntity.UpdatedOn = HymsonClock.Now();
            procBomEntity.BomCode = procBomEntity.BomCode.ToUpperInvariant();


            //验证是否存在
            var exists = await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery()
            {
                SiteCode = procBomEntity.SiteCode,
                BomCode = procBomEntity.BomCode,
                Version = procBomEntity.Version,
            });
            if (exists != null && exists.Count() > 0) 
            {
                throw new BusinessException(ErrorCode.MES10601).WithData("bomCode", procBomEntity.BomCode).WithData("version", procBomEntity.Version);
            }

            var materialList = procBomCreateDto.MaterialList.ToList();
            var bomDetails = new List<ProcBomDetailEntity>();
            var bomReplaceDetails = new List<ProcBomDetailReplaceMaterialEntity>();
            if (materialList.Count > 0)
            {
                if (materialList.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode)))
                {
                    throw new ValidationException(ErrorCode.MES10603);
                }
                if (materialList.Any(a => a.IsMain != 1 && string.IsNullOrWhiteSpace(a.ReplaceMaterialId)))
                {
                    throw new ValidationException(ErrorCode.MES10604);
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"替代物料编码不能为空!";
                    //return apiResult;
                }
                var mainList = materialList.Where(a => a.IsMain == 1).ToList();
                if (mainList.Any(a => string.IsNullOrWhiteSpace(a.Code) || a.ProcedureBomId == "0"))
                {
                    throw new ValidationException(ErrorCode.MES10605);
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"工序不能为空!";
                    //return apiResult;
                }
                if (mainList.GroupBy(m => new { m.MaterialId, m.ProcedureBomId }).Where(g => g.Count() > 1).Count() > 0)
                {
                    throw new ValidationException(ErrorCode.MES10606);
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"主物料编码+工序不能重复!";
                    //return apiResult;
                }
                if (materialList.Any(a => a.MaterialId == a.ReplaceMaterialId))
                {
                    throw new ValidationException(ErrorCode.MES10607);
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"替代物料不能跟主物料重复!";
                    //return apiResult;
                }

                var replaceList = materialList.Where(a => a.IsMain == 0).ToList();
                if (replaceList.GroupBy(m => new { m.MaterialId, m.ReplaceMaterialId }).Where(g => g.Count() > 1).Count() > 0)
                {
                    throw new ValidationException(ErrorCode.MES10608);
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"主物料关联的替代物料不能重复!";
                    //return apiResult;
                }


                long mainId = 0;
                foreach (var material in materialList)
                {
                    if (material.IsMain == 1)
                    {
                        var bomdeail = new ProcBomDetailEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteCode = procBomCreateDto.SiteCode,
                            BomId = procBomEntity.Id,
                            MaterialId = material.MaterialId.ParseToLong(),
                            ProcedureBomId = material.ProcedureBomId.ParseToLong(),
                            ReferencePoint = material.ReferencePoint,
                            Usages = material.Usages,
                            Loss = material.Loss ?? 0,
                            CreatedBy = procBomEntity.CreatedBy,
                            CreatedOn = procBomEntity.CreatedOn
                        };
                        mainId = bomdeail.Id;
                        bomDetails.Add(bomdeail);
                    }
                    else
                    {
                        var bomReplacedeail = new ProcBomDetailReplaceMaterialEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteCode = procBomCreateDto.SiteCode,
                            BomId = procBomEntity.Id,
                            BomDetailId = mainId,
                            ReplaceMaterialId = material.ReplaceMaterialId.ParseToLong(),
                            ReferencePoint = material.ReferencePoint,
                            Usages = material.Usages,
                            Loss = material.Loss ?? 0,
                            CreatedBy = procBomEntity.CreatedBy,
                            CreatedOn = procBomEntity.CreatedOn
                        };
                        bomReplaceDetails.Add(bomReplacedeail);
                    }
                }
            }

            #region 操作数据库
            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                if (procBomEntity.IsCurrentVersion)
                {
                    var currentProcBom = (await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery()
                    {
                        SiteCode = procBomEntity.SiteCode,
                        BomCode = procBomEntity.BomCode,
                    })).Where(x => x.IsCurrentVersion = true).First(); ;//a => a.SiteCode == parm.SiteCode && a.BomCode == procBom.BomCode && a.IsCurrentVersion == true
                    if (currentProcBom != null)
                    {
                        await _procBomRepository.UpdateIsCurrentVersionIsFalseAsync(new long[] { currentProcBom.Id });
                    }
                }

                response = await _procBomRepository.InsertAsync(procBomEntity);

                if (response == 0)
                {
                    throw new BusinessException(ErrorCode.MES10602);
                }

                if (bomDetails.Count > 0)
                {
                    response = await _procBomDetailRepository.InsertsAsync(bomDetails);
                    if (response == 0)
                    {
                        throw new BusinessException(ErrorCode.MES10602);
                    }
                }
                if (bomReplaceDetails.Count > 0)
                {
                    response = await _procBomDetailReplaceMaterialRepository.InsertsAsync(bomReplaceDetails);
                    if (response == 0)
                    {
                        throw new BusinessException(ErrorCode.MES10602);
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
        public async Task DeleteProcBomAsync(long id)
        {
            await _procBomRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesProcBomAsync(string ids)
        {
            var updateBy = _currentUser.UserName;
            var idsArr = StringExtension.SpitLongArrary(ids);

            if (idsArr.Length < 1)
            {
                throw new ValidationException(ErrorCode.MES10610);
            }

            //判断需要删除的Bom是否是启用状态
            var bomList = await _procBomRepository.GetByIdsAsync(idsArr);
            if (bomList.Any(x => x.Status == "1")) 
            {
                throw new BusinessException(ErrorCode.MES10611);
            }

            return await _procBomRepository.DeletesAsync(idsArr);
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="procBomPagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcBomDto>> GetPageListAsync(ProcBomPagedQueryDto procBomPagedQueryDto)
        {
            procBomPagedQueryDto.SiteCode = "";//TODO

            var procBomPagedQuery = procBomPagedQueryDto.ToQuery<ProcBomPagedQuery>();
            var pagedInfo = await _procBomRepository.GetPagedInfoAsync(procBomPagedQuery);

            //实体到DTO转换 装载数据
            List<ProcBomDto> procBomDtos = PrepareProcBomDtos(pagedInfo);
            return new PagedInfo<ProcBomDto>(procBomDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ProcBomDto> PrepareProcBomDtos(PagedInfo<ProcBomEntity>   pagedInfo)
        {
            var procBomDtos = new List<ProcBomDto>();
            foreach (var procBomEntity in pagedInfo.Data)
            {
                var procBomDto = procBomEntity.ToModel<ProcBomDto>();
                procBomDtos.Add(procBomDto);
            }

            return procBomDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="procBomDto"></param>
        /// <returns></returns>
        public async Task ModifyProcBomAsync(ProcBomModifyDto procBomModifyDto)
        {
            if (procBomModifyDto == null)
            {
                throw new ValidationException(ErrorCode.MES10100);
            }

            var list = procBomModifyDto.MaterialList.ToList();
            list.ForEach(a =>
            {
                if (string.IsNullOrWhiteSpace(a.ProcedureBomId))
                {
                    a.ProcedureBomId = "0";
                }
            });
            procBomModifyDto.SiteCode = "TODO";

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(procBomModifyDto);

            //DTO转换实体
            var procBomEntity = procBomModifyDto.ToEntity<ProcBomEntity>();
            procBomEntity.UpdatedBy = _currentUser.UserName;
            procBomEntity.UpdatedOn = HymsonClock.Now();

            //判断该bom是否还存在
            var modelOrigin = await _procBomRepository.GetByIdAsync(procBomEntity.Id);
            if (modelOrigin == null)
            {

                //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                //apiResult.Msg = $"此Bom在系统中不存在!";
                //return apiResult;
            }

            //验证是否存在
            var exists = (await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery()
            {
                SiteCode = procBomEntity.SiteCode,
                BomCode = procBomEntity.BomCode,
                Version = procBomEntity.Version,
            })).Where(x=>x.Id!= procBomEntity.Id && x.IsDeleted==false) ;
            if (exists != null && exists.Count() > 0)
            {
                throw new BusinessException(ErrorCode.MES10601).WithData("bomCode", procBomEntity.BomCode).WithData("version", procBomEntity.Version);
            }

            var materialList = procBomModifyDto.MaterialList.ToList();
            var bomDetails = new List<ProcBomDetailEntity>();
            var bomReplaceDetails = new List<ProcBomDetailReplaceMaterialEntity>();
            if (materialList.Count > 0)
            {
                if (materialList.Any(a => string.IsNullOrWhiteSpace(a.MaterialCode)))
                {
                    throw new ValidationException(ErrorCode.MES10603);
                }
                if (materialList.Any(a => a.IsMain != 1 && string.IsNullOrWhiteSpace(a.ReplaceMaterialId)))
                {
                    throw new ValidationException(ErrorCode.MES10604);
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"替代物料编码不能为空!";
                    //return apiResult;
                }
                var mainList = materialList.Where(a => a.IsMain == 1).ToList();
                if (mainList.Any(a => string.IsNullOrWhiteSpace(a.Code) || a.ProcedureBomId == "0"))
                {
                    throw new ValidationException(ErrorCode.MES10605);
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"工序不能为空!";
                    //return apiResult;
                }
                if (mainList.GroupBy(m => new { m.MaterialId, m.ProcedureBomId }).Where(g => g.Count() > 1).Count() > 0)
                {
                    throw new ValidationException(ErrorCode.MES10606);
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"主物料编码+工序不能重复!";
                    //return apiResult;
                }
                if (materialList.Any(a => a.MaterialId == a.ReplaceMaterialId))
                {
                    throw new ValidationException(ErrorCode.MES10607);
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"替代物料不能跟主物料重复!";
                    //return apiResult;
                }

                var replaceList = materialList.Where(a => a.IsMain == 0).ToList();
                if (replaceList.GroupBy(m => new { m.MaterialId, m.ReplaceMaterialId }).Where(g => g.Count() > 1).Count() > 0)
                {
                    throw new ValidationException(ErrorCode.MES10608);
                    //apiResult.Code = (int)ResultCode.PARAM_ERROR;
                    //apiResult.Msg = $"主物料关联的替代物料不能重复!";
                    //return apiResult;
                }

                long mainId = 0;
                foreach (var material in materialList)
                {
                    if (material.IsMain == 1)
                    {
                        var bomdeail = new ProcBomDetailEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteCode = procBomModifyDto.SiteCode,
                            BomId = procBomEntity.Id,
                            MaterialId = material.MaterialId.ParseToLong(),
                            ProcedureBomId = material.ProcedureBomId.ParseToLong(),
                            ReferencePoint = material.ReferencePoint,
                            Usages = material.Usages,
                            Loss = material.Loss ?? 0,
                            CreatedBy = procBomEntity.CreatedBy,
                            CreatedOn = procBomEntity.CreatedOn
                        };
                        mainId = bomdeail.Id;
                        bomDetails.Add(bomdeail);
                    }
                    else
                    {
                        var bomReplacedeail = new ProcBomDetailReplaceMaterialEntity
                        {
                            Id = IdGenProvider.Instance.CreateId(),
                            SiteCode = procBomModifyDto.SiteCode,
                            BomId = procBomEntity.Id,
                            BomDetailId = mainId,
                            ReplaceMaterialId = material.ReplaceMaterialId.ParseToLong(),
                            ReferencePoint = material.ReferencePoint,
                            Usages = material.Usages,
                            Loss = material.Loss ?? 0,
                            CreatedBy = procBomEntity.CreatedBy,
                            CreatedOn = procBomEntity.CreatedOn
                        };
                        bomReplaceDetails.Add(bomReplacedeail);
                    }
                }
            }

            #region 操作数据库
            using (TransactionScope ts = new TransactionScope())
            {
                int response = 0;
                if (procBomEntity.IsCurrentVersion)
                {
                    var currentProcBom = (await _procBomRepository.GetProcBomEntitiesAsync(new ProcBomQuery()
                    {
                        SiteCode = procBomEntity.SiteCode,
                        BomCode = procBomEntity.BomCode,
                    })).Where(x => x.IsCurrentVersion = true).First(); ;//a => a.SiteCode == parm.SiteCode && a.BomCode == procBom.BomCode && a.IsCurrentVersion == true
                    if (currentProcBom != null)
                    {
                        await _procBomRepository.UpdateIsCurrentVersionIsFalseAsync(new long[] { currentProcBom.Id });
                    }
                }

                //编码+版本唯一在修改时两者均不可修改
                response = await _procBomRepository.UpdateAsync(procBomEntity);

                if (response == 0) 
                {
                    throw new BusinessException(ErrorCode.MES10609);
                }

                await _procBomDetailRepository.DeleteBomIDAsync(new long[] { procBomEntity.Id });
                await _procBomDetailReplaceMaterialRepository.DeleteBomIDAsync(new long[] { procBomEntity.Id });

                if (bomDetails.Count > 0)
                {
                    response = await _procBomDetailRepository.InsertsAsync(bomDetails);

                    if (response == 0)
                    {
                        throw new BusinessException(ErrorCode.MES10609);
                    }
                }
                if (bomReplaceDetails.Count > 0)
                {
                    response = await _procBomDetailReplaceMaterialRepository.InsertsAsync(bomReplaceDetails);
                    if (response == 0)
                    {
                        throw new BusinessException(ErrorCode.MES10609);
                    }
                }

                ts.Complete();
            }

            #endregion
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcBomDto> QueryProcBomByIdAsync(long id) 
        {
           var procBomEntity = await _procBomRepository.GetByIdAsync(id);
           if (procBomEntity != null) 
           {
               return procBomEntity.ToModel<ProcBomDto>();
           }
            return null;
        }

        /// <summary>
        /// 根据ID查询Bom 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ProcBomDetailView>> GetProcBomMaterialAsync(long id)
        {
            var procBomDetailViews=new List<ProcBomDetailView>();
            var mainBomDetails = await _procBomDetailRepository.GetListMainAsync(id);
            var replaceBomDetails = await _procBomDetailRepository.GetListReplaceAsync(id);

            if (mainBomDetails.Count() > 0)
            {
                procBomDetailViews.AddRange(mainBomDetails);
            }
            if (replaceBomDetails.Count() > 0)
            {
                procBomDetailViews.AddRange(replaceBomDetails);
            }

            return procBomDetailViews;
        }
    }
}
