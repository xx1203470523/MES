/*
 *creator: Karl
 *
 *describe: 容器条码表    服务 | 代码由框架生成
 *builder:  wxk
 *build datetime: 2023-04-12 02:29:23
 */
using FluentValidation;
using Hymson.Authentication;
using Hymson.Authentication.JwtBearer.Security;
using Hymson.Infrastructure;
using Hymson.Infrastructure.Exceptions;
using Hymson.Infrastructure.Mapper;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Enums.Integrated;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated.InteContainer;
using Hymson.MES.Data.Repositories.Integrated.InteContainer.Query;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.Services.Dtos.Manufacture;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuGenerateBarcodeDto;
using Hymson.MES.Services.Services.Manufacture.ManuMainstreamProcess.GenerateBarcode;
using Hymson.Snowflake;
using Hymson.Utils;
using System.Transactions;

namespace Hymson.MES.Services.Services.Manufacture
{
    /// <summary>
    /// 容器条码表 服务
    /// </summary>
    public class ManuContainerBarcodeService : IManuContainerBarcodeService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;

        /// <summary>
        /// 容器条码表 仓储
        /// </summary>
        private readonly IManuContainerBarcodeRepository _manuContainerBarcodeRepository;
        private readonly IManuContainerPackRepository _manuContainerPackRepository;
        private readonly IInteContainerRepository   _inteContainerRepository;
        private readonly IManuContainerPackRecordRepository _manuContainerPackRecordRepository;
        private readonly IManuSfcRepository _manuSfcRepository;
        private readonly IManuSfcInfoRepository _manuSfcInfoRepository;
        private readonly IManuGenerateBarcodeService _manuGenerateBarcodeService;
        private readonly IProcMaterialRepository _procMaterialRepository;
        private readonly AbstractValidator<ManuContainerBarcodeCreateDto> _validationCreateRules;
        private readonly AbstractValidator<ManuContainerBarcodeModifyDto> _validationModifyRules;

        public ManuContainerBarcodeService(ICurrentUser currentUser, ICurrentSite currentSite, IManuContainerBarcodeRepository manuContainerBarcodeRepository, AbstractValidator<ManuContainerBarcodeCreateDto> validationCreateRules
            , AbstractValidator<ManuContainerBarcodeModifyDto> validationModifyRules
            , IManuContainerPackRepository manuContainerPackRepository
            , IInteContainerRepository ingiContainerRepository
            ,IManuContainerPackRecordRepository manuContainerPackRecordRepository
            ,IManuSfcRepository manuSfcRepository
            ,IManuSfcInfoRepository manuSfcInfoRepository
            ,IManuGenerateBarcodeService manuGenerateBarcodeService,IProcMaterialRepository procMaterialRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite; 
            _manuContainerBarcodeRepository = manuContainerBarcodeRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _manuContainerPackRepository = manuContainerPackRepository; 
            _inteContainerRepository = ingiContainerRepository;
            _manuContainerPackRecordRepository = manuContainerPackRecordRepository;
            _manuSfcRepository = manuSfcRepository;
            _manuSfcInfoRepository = manuSfcInfoRepository;
            _manuGenerateBarcodeService = manuGenerateBarcodeService;
            _procMaterialRepository = procMaterialRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="manuContainerBarcodeCreateDto"></param>
        /// <returns></returns>
        public async Task<ManuContainerBarcodeView> CreateManuContainerBarcodeAsync(ManuContainerBarcodeCreateDto manuContainerBarcodeCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(manuContainerBarcodeCreateDto);

            //DTO转换实体
            var manuContainerBarcodeEntity = manuContainerBarcodeCreateDto.ToEntity<ManuContainerBarcodeEntity>();
            manuContainerBarcodeEntity.Id= IdGenProvider.Instance.CreateId();
            manuContainerBarcodeEntity.CreatedBy = _currentUser.UserName;
            manuContainerBarcodeEntity.UpdatedBy = _currentUser.UserName;
            manuContainerBarcodeEntity.CreatedOn = HymsonClock.Now();
            manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();
            manuContainerBarcodeEntity.SiteId = _currentSite.SiteId ?? 0;

            /*根据条码判定是否有包装记录
             * Y 返回 view 
             * N ，判定包装码是否为空，
             *        Y  根据条码查找打开着的包装码 返回view
             *        N  返回这个包装码的view
             */
            var foo = await _manuContainerPackRepository.GetByLadeBarCodeAsync(manuContainerBarcodeCreateDto.BarCode);
            if(foo != null)
            {
                var barcodeobj = await _manuContainerBarcodeRepository.GetByIdAsync(foo.ContainerBarCodeId);
                var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                ManuContainerBarcodeView view = new ManuContainerBarcodeView()
                {
                    manuContainerBarcodeEntity = barcodeobj,
                    inteContainerEntity = inte
                };
                return view;
            }
            else
            {
                //查找包装码
                if (string.IsNullOrEmpty(manuContainerBarcodeCreateDto.ContainerCode))
                {
                    //查找条码信息
                    var sfcEntity = await _manuSfcRepository.GetBySFCAsync(manuContainerBarcodeCreateDto.BarCode);
                    if(sfcEntity != null)
                    {
                        var info = await _manuSfcInfoRepository.GetByIdAsync(sfcEntity.Id);//实际应该调用 GetBySFCAsync
                        var barcodeobj = await _manuContainerBarcodeRepository.GetByCodeAsync(info.ProductId, (int)ManuContainerBarcodeStatusEnum.Open);
                        if(barcodeobj != null) {
                            var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                            ManuContainerBarcodeView view = new ManuContainerBarcodeView()
                            {
                                manuContainerBarcodeEntity = barcodeobj,
                                inteContainerEntity = inte
                            };
                            return view;
                        }
                        else
                        {
                            var entityByRelation = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
                            {
                                DefinitionMethod = DefinitionMethodEnum.Material,
                                MaterialId = info.ProductId,
                                MaterialGroupId = 0
                            });
                            if (entityByRelation != null)
                            {
                                manuContainerBarcodeEntity.ContainerId = entityByRelation.Id;
                                manuContainerBarcodeEntity.ProductId = info.ProductId;
                                var barcodeList = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeDto
                                {
                                    CodeRuleId = manuContainerBarcodeEntity.Id,
                                    Count = 1
                                });
                                manuContainerBarcodeEntity.BarCode = barcodeList.First();
                                //入库
                                await _manuContainerBarcodeRepository.InsertAsync(manuContainerBarcodeEntity);
                                ManuContainerBarcodeView view = new ManuContainerBarcodeView()
                                {
                                    manuContainerBarcodeEntity = manuContainerBarcodeEntity,
                                    inteContainerEntity = entityByRelation
                                };
                                return view;
                            }
                            else
                            {
                                //根据物料组查找包装维护记录
                                var materialEntity = await _procMaterialRepository.GetByIdAsync(info.ProductId);
                                if(materialEntity != null)
                                {
                                    if(materialEntity.GroupId!=0)
                                    {
                                        var entityByRelation1 = await _inteContainerRepository.GetByRelationIdAsync(new InteContainerQuery
                                        {
                                            DefinitionMethod = DefinitionMethodEnum.MaterialGroup,
                                            MaterialId = info.ProductId,
                                            MaterialGroupId = materialEntity.GroupId
                                        });
                                        if(entityByRelation1 != null)
                                        {
                                            manuContainerBarcodeEntity.ContainerId = entityByRelation1.Id;
                                            manuContainerBarcodeEntity.ProductId = info.ProductId;
                                            var barcodeList = await _manuGenerateBarcodeService.GenerateBarcodeListByIdAsync(new GenerateBarcodeDto
                                            {
                                                CodeRuleId = manuContainerBarcodeEntity.Id,
                                                Count = 1
                                            });
                                            manuContainerBarcodeEntity.BarCode = barcodeList.First();
                                            //入库
                                            await _manuContainerBarcodeRepository.InsertAsync(manuContainerBarcodeEntity);
                                            ManuContainerBarcodeView view = new ManuContainerBarcodeView()
                                            {
                                                manuContainerBarcodeEntity = manuContainerBarcodeEntity,
                                                inteContainerEntity = entityByRelation1
                                            };
                                            return view;
                                        }
                                        else
                                        {
                                            throw new ValidationException(nameof(ErrorCode.MES16703));
                                        }
                                    }
                                    else
                                    {
                                        throw new ValidationException(nameof(ErrorCode.MES16703));
                                    }
                                }
                                else
                                {
                                    throw new ValidationException(nameof(ErrorCode.MES10204));
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new ValidationException(nameof(ErrorCode.MES16701));
                    }
                }
                else //已经存在包装码
                {
                    /*判定包装码与条码是否同一个包装
                     * Y  判定包装是否装满  Y 
                     */
                    //包装码与条码是否是同一个包装，Y  判定条码是否
                    var barcodeobj = await _manuContainerBarcodeRepository.GetByCodeAsync(manuContainerBarcodeCreateDto.ContainerCode);
                    var inte = await _inteContainerRepository.GetByIdAsync(barcodeobj.ContainerId);
                    var pack = await _manuContainerPackRepository.GetByContainerBarCodeIdAsync(barcodeobj.Id);//实际绑定集合
                    if (barcodeobj != null) //包装码存在
                    {
                        ManuContainerBarcodeView view = new ManuContainerBarcodeView()
                        {
                            manuContainerBarcodeEntity = barcodeobj,
                            inteContainerEntity = inte
                        };
                        return view;
                       
                    }
                    else
                    {
                        throw new ValidationException(nameof(ErrorCode.MES16702));
                    }

                }
            }

        }
        

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteManuContainerBarcodeAsync(long id)
        {
            await _manuContainerBarcodeRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesManuContainerBarcodeAsync(long[] ids)
        {
            return await _manuContainerBarcodeRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="manuContainerBarcodePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ManuContainerBarcodeDto>> GetPagedListAsync(ManuContainerBarcodePagedQueryDto manuContainerBarcodePagedQueryDto)
        {
            var manuContainerBarcodePagedQuery = manuContainerBarcodePagedQueryDto.ToQuery<ManuContainerBarcodePagedQuery>();
            var pagedInfo = await _manuContainerBarcodeRepository.GetPagedInfoAsync(manuContainerBarcodePagedQuery);

            //实体到DTO转换 装载数据
            List<ManuContainerBarcodeDto> manuContainerBarcodeDtos = PrepareManuContainerBarcodeDtos(pagedInfo);
            return new PagedInfo<ManuContainerBarcodeDto>(manuContainerBarcodeDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<ManuContainerBarcodeDto> PrepareManuContainerBarcodeDtos(PagedInfo<ManuContainerBarcodeEntity>   pagedInfo)
        {
            var manuContainerBarcodeDtos = new List<ManuContainerBarcodeDto>();
            foreach (var manuContainerBarcodeEntity in pagedInfo.Data)
            {
                var manuContainerBarcodeDto = manuContainerBarcodeEntity.ToModel<ManuContainerBarcodeDto>();
                manuContainerBarcodeDtos.Add(manuContainerBarcodeDto);
            }

            return manuContainerBarcodeDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="manuContainerBarcodeDto"></param>
        /// <returns></returns>
        public async Task ModifyManuContainerBarcodeAsync(ManuContainerBarcodeModifyDto manuContainerBarcodeModifyDto)
        {
             // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

             //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(manuContainerBarcodeModifyDto);

            //DTO转换实体
            var manuContainerBarcodeEntity = manuContainerBarcodeModifyDto.ToEntity<ManuContainerBarcodeEntity>();
            manuContainerBarcodeEntity.UpdatedBy = _currentUser.UserName;
            manuContainerBarcodeEntity.UpdatedOn = HymsonClock.Now();

            await _manuContainerBarcodeRepository.UpdateAsync(manuContainerBarcodeEntity);
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ManuContainerBarcodeDto> QueryManuContainerBarcodeByIdAsync(long id) 
        {
           var manuContainerBarcodeEntity = await _manuContainerBarcodeRepository.GetByIdAsync(id);
           if (manuContainerBarcodeEntity != null) 
           {
               return manuContainerBarcodeEntity.ToModel<ManuContainerBarcodeDto>();
           }
            return null;
        }
    }
}
