/*
 *creator: Karl
 *
 *describe: 载具注册表    服务 | 代码由框架生成
 *builder:  Karl
 *build datetime: 2023-07-14 10:03:53
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
using Hymson.MES.Core.Domain.Plan;
using Hymson.MES.Core.Enums.Manufacture;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Integrated;
using Hymson.MES.Services.Dtos.Integrated;
using Hymson.MES.Services.Dtos.Manufacture.ManuMainstreamProcessDto.ManuCreateBarcodeDto;
using Hymson.Snowflake;
using Hymson.Utils;
using IdGen;
using Minio.DataModel;
using System.Transactions;
using System.Drawing.Drawing2D;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.Utils.Tools;
using System.Reflection.Metadata;

namespace Hymson.MES.Services.Services.Integrated
{
    /// <summary>
    /// 载具注册表 服务
    /// </summary>
    public class InteVehicleService : IInteVehicleService
    {
        private readonly ICurrentUser _currentUser;
        private readonly ICurrentSite _currentSite;


        /// <summary>
        /// 载具注册表 仓储
        /// </summary>
        private readonly IInteVehicleRepository _inteVehicleRepository;
        private readonly AbstractValidator<InteVehicleCreateDto> _validationCreateRules;
        private readonly AbstractValidator<InteVehicleModifyDto> _validationModifyRules;

        private readonly IInteVehicleTypeRepository _inteVehicleTypeRepository;
        private readonly IInteVehicleVerifyRepository _inteVehicleVerifyRepository;
        private readonly IInteVehicleFreightRepository _inteVehicleFreightRepository;
        private readonly IInteVehiceFreightStackRepository _inteVehiceFreightStackRepository;
        private readonly IInteVehicleFreightRecordRepository _inteVehicleFreightRecordRepository;
        private readonly IManuSfcProduceRepository _manuSfcProduceRepository;

        public InteVehicleService(ICurrentUser currentUser, ICurrentSite currentSite, IInteVehicleRepository inteVehicleRepository, AbstractValidator<InteVehicleCreateDto> validationCreateRules, AbstractValidator<InteVehicleModifyDto> validationModifyRules, IInteVehicleTypeRepository inteVehicleTypeRepository,
            IInteVehicleVerifyRepository inteVehicleVerifyRepository,
            IInteVehiceFreightStackRepository inteVehiceFreightStackRepository,
            IInteVehicleFreightRecordRepository inteVehicleFreightRecordRepository,
            IManuSfcProduceRepository manuSfcProduceRepository,
            IInteVehicleFreightRepository inteVehicleFreightRepository)
        {
            _currentUser = currentUser;
            _currentSite = currentSite;
            _inteVehicleRepository = inteVehicleRepository;
            _validationCreateRules = validationCreateRules;
            _validationModifyRules = validationModifyRules;
            _inteVehicleTypeRepository = inteVehicleTypeRepository;
            _inteVehicleVerifyRepository = inteVehicleVerifyRepository;
            _inteVehicleFreightRepository = inteVehicleFreightRepository;
            _inteVehiceFreightStackRepository = inteVehiceFreightStackRepository;
            _inteVehicleFreightRecordRepository = inteVehicleFreightRecordRepository;
            _manuSfcProduceRepository = manuSfcProduceRepository;
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="inteVehicleCreateDto"></param>
        /// <returns></returns>
        public async Task CreateInteVehicleAsync(InteVehicleCreateDto inteVehicleCreateDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationCreateRules.ValidateAndThrowAsync(inteVehicleCreateDto);

            //DTO转换实体
            var inteVehicleEntity = inteVehicleCreateDto.ToEntity<InteVehicleEntity>();
            inteVehicleEntity.Id = IdGenProvider.Instance.CreateId();
            inteVehicleEntity.CreatedBy = _currentUser.UserName;
            inteVehicleEntity.UpdatedBy = _currentUser.UserName;
            inteVehicleEntity.CreatedOn = HymsonClock.Now();
            inteVehicleEntity.UpdatedOn = HymsonClock.Now();
            inteVehicleEntity.SiteId = _currentSite.SiteId ?? 0;

            //验证是否编码唯一
            var entity = await _inteVehicleRepository.GetByCodeAsync(new InteVehicleCodeQuery
            {
                Code = inteVehicleEntity.Code.Trim(),
                SiteId = _currentSite.SiteId ?? 0
            });
            if (entity != null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18602));
            }

            #region 处理 载具验证数据
            InteVehicleVerifyEntity? inteVehicleVerify = null;

            if (inteVehicleCreateDto.InteVehicleVerify != null && inteVehicleCreateDto.InteVehicleVerify.ExpirationDate.HasValue)
            {
                inteVehicleVerify = new InteVehicleVerifyEntity()
                {
                    VehicleId = inteVehicleEntity.Id,
                    ExpirationDate = inteVehicleCreateDto.InteVehicleVerify.ExpirationDate.Value,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentSite.SiteId ?? 0
                };
            }

            #endregion

            #region 处理 载具装载数据
            var inteVehicleFreightEntitys = new List<InteVehicleFreightEntity>();

            if (inteVehicleCreateDto.InteVehicleFreights != null && inteVehicleCreateDto.InteVehicleFreights.Any())
            {
                foreach (var item in inteVehicleCreateDto.InteVehicleFreights)
                {
                    inteVehicleFreightEntitys.Add(new InteVehicleFreightEntity()
                    {
                        VehicleId = inteVehicleEntity.Id,
                        Column = item.Column,
                        Row = item.Row,
                        Location = item.Location,
                        Status = item.Status,
                        Qty = 0,
                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }
            }
            #endregion

            using (TransactionScope ts = new TransactionScope())
            {
                //入库
                await _inteVehicleRepository.InsertAsync(inteVehicleEntity);

                if (inteVehicleVerify != null)
                {
                    await _inteVehicleVerifyRepository.InsertAsync(inteVehicleVerify);
                }

                if (inteVehicleFreightEntitys.Any())
                {
                    await _inteVehicleFreightRepository.InsertsAsync(inteVehicleFreightEntitys);
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteInteVehicleAsync(long id)
        {
            await _inteVehicleRepository.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesInteVehicleAsync(long[] ids)
        {
            //校验数据 
            var inteVehicleFreights = await _inteVehicleFreightRepository.GetByVehicleIdsAsync(ids);
            var hasBinds = inteVehicleFreights.Where(x => x.Qty > 0);
            if (hasBinds.Any())
            {
                //查询是哪些载具
                var vehicleIds = hasBinds.Select(x => x.VehicleId).Distinct().ToArray();
                var hasBindVehicles = await _inteVehicleRepository.GetByIdsAsync(vehicleIds);

                throw new CustomerValidationException(nameof(ErrorCode.MES18603)).WithData("codes", string.Join(", ", hasBindVehicles.Select(x => x.Code).Distinct().ToArray()));
            }

            return await _inteVehicleRepository.DeletesAsync(new DeleteCommand { Ids = ids, DeleteOn = HymsonClock.Now(), UserId = _currentUser.UserName });
        }

        /// <summary>
        /// 根据查询条件获取分页数据
        /// </summary>
        /// <param name="inteVehiclePagedQueryDto"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteVehicleViewDto>> GetPagedListAsync(InteVehiclePagedQueryDto inteVehiclePagedQueryDto)
        {
            var inteVehiclePagedQuery = inteVehiclePagedQueryDto.ToQuery<InteVehiclePagedQuery>();
            inteVehiclePagedQuery.SiteId = _currentSite.SiteId ?? 0;
            var pagedInfo = await _inteVehicleRepository.GetPagedInfoAsync(inteVehiclePagedQuery);

            //实体到DTO转换 装载数据
            List<InteVehicleViewDto> inteVehicleDtos = PrepareInteVehicleDtos(pagedInfo);
            return new PagedInfo<InteVehicleViewDto>(inteVehicleDtos, pagedInfo.PageIndex, pagedInfo.PageSize, pagedInfo.TotalCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        private static List<InteVehicleViewDto> PrepareInteVehicleDtos(PagedInfo<InteVehicleView> pagedInfo)
        {
            var inteVehicleDtos = new List<InteVehicleViewDto>();
            foreach (var inteVehicleView in pagedInfo.Data)
            {
                var inteVehicleDto = inteVehicleView.ToModel<InteVehicleViewDto>();
                inteVehicleDtos.Add(inteVehicleDto);
            }

            return inteVehicleDtos;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="inteVehicleModifyDto"></param>
        /// <returns></returns>
        public async Task ModifyInteVehicleAsync(InteVehicleModifyDto inteVehicleModifyDto)
        {
            // 判断是否有获取到站点码 
            if (_currentSite.SiteId == 0)
            {
                throw new ValidationException(nameof(ErrorCode.MES10101));
            }

            //验证DTO
            await _validationModifyRules.ValidateAndThrowAsync(inteVehicleModifyDto);

            //DTO转换实体
            var inteVehicleEntity = inteVehicleModifyDto.ToEntity<InteVehicleEntity>();
            inteVehicleEntity.UpdatedBy = _currentUser.UserName;
            inteVehicleEntity.UpdatedOn = HymsonClock.Now();

            #region 处理 载具验证数据
            InteVehicleVerifyEntity? inteVehicleVerify = null;

            if (inteVehicleModifyDto.InteVehicleVerify != null && inteVehicleModifyDto.InteVehicleVerify.ExpirationDate.HasValue)
            {
                inteVehicleVerify = new InteVehicleVerifyEntity()
                {
                    VehicleId = inteVehicleEntity.Id,
                    ExpirationDate = inteVehicleModifyDto.InteVehicleVerify.ExpirationDate.Value,

                    Id = IdGenProvider.Instance.CreateId(),
                    CreatedBy = _currentUser.UserName,
                    UpdatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    UpdatedOn = HymsonClock.Now(),
                    SiteId = _currentSite.SiteId ?? 0
                };
            }

            #endregion

            #region 处理 载具装载数据
            var inteVehicleFreightEntitys = new List<InteVehicleFreightEntity>();

            if (inteVehicleModifyDto.InteVehicleFreights != null && inteVehicleModifyDto.InteVehicleFreights.Any())
            {
                foreach (var item in inteVehicleModifyDto.InteVehicleFreights)
                {
                    inteVehicleFreightEntitys.Add(new InteVehicleFreightEntity()
                    {
                        VehicleId = inteVehicleEntity.Id,
                        Column = item.Column,
                        Row = item.Row,
                        Location = item.Location,
                        Status = item.Status,

                        Id = IdGenProvider.Instance.CreateId(),
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0
                    });
                }
            }
            #endregion


            using (TransactionScope ts = new TransactionScope())
            {
                await _inteVehicleRepository.UpdateAsync(inteVehicleEntity);

                await _inteVehicleVerifyRepository.DeletesTrueByVehicleIdsAsync(new long[] { inteVehicleEntity.Id });

                if (inteVehicleVerify != null)
                {
                    await _inteVehicleVerifyRepository.InsertAsync(inteVehicleVerify);
                }

                await _inteVehicleFreightRepository.DeletesTrueByVehicleIdsAsync(new long[] { inteVehicleEntity.Id });
                if (inteVehicleFreightEntitys.Any())
                {
                    await _inteVehicleFreightRepository.InsertsAsync(inteVehicleFreightEntitys);
                }

                ts.Complete();
            }
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteVehicleDto> QueryInteVehicleByIdAsync(long id)
        {
            var inteVehicleEntity = await _inteVehicleRepository.GetByIdAsync(id);
            if (inteVehicleEntity != null)
            {
                var inteVehicleView = inteVehicleEntity.ToModel<InteVehicleViewDto>();
                if (inteVehicleView.VehicleTypeId > 0)
                {
                    //查询编码规则类型
                    var inteVehicleTypeEntity = await _inteVehicleTypeRepository.GetByIdAsync(inteVehicleView.VehicleTypeId);
                    inteVehicleView.VehicleTypeCode = inteVehicleTypeEntity?.Code ?? "";
                    inteVehicleView.VehicleTypeName = inteVehicleTypeEntity?.Name ?? "";
                }

                return inteVehicleView;
            }
            throw new CustomerValidationException(nameof(ErrorCode.MES18601));
        }

        /// <summary>
        /// 获取载具验证
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public async Task<InteVehicleVerifyDto> QueryVehicleVerifyByVehicleIdAsync(long vehicleId)
        {
            var inteVehicleVerify = await _inteVehicleVerifyRepository.GetByVehicleIdAsync(vehicleId);
            if (inteVehicleVerify != null)
            {
                return inteVehicleVerify.ToModel<InteVehicleVerifyDto>();
            }
            else
                return null;
        }

        /// <summary>
        /// 获取载具装载
        /// </summary>
        /// <param name="vehicleId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteVehicleFreightDto>> QueryVehicleFreightByVehicleIdAsync(long vehicleId)
        {
            var inteVehicleFreights = await _inteVehicleFreightRepository.GetByVehicleIdsAsync(new long[] { vehicleId });
            var inteVehicleFreightDtos = new List<InteVehicleFreightDto>();
            if (inteVehicleFreights != null && inteVehicleFreights.Any())
            {
                foreach (var item in inteVehicleFreights)
                {
                    inteVehicleFreightDtos.Add(item.ToModel<InteVehicleFreightDto>());
                }
            }
            return inteVehicleFreightDtos;
        }

        public async Task<InteVehicleStackView> QueryVehicleFreightByPalletNoAsync(string palletNo)
        {
            InteVehicleStackView view = new InteVehicleStackView();
            var inteVehicle = await _inteVehicleRepository.GetByCodeAsync(new InteVehicleCodeQuery()
            {
                Code = palletNo,
                SiteId = _currentSite.SiteId ?? 0
            });
            if (inteVehicle == null || inteVehicle.Status != DisableOrEnableEnum.Enable)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18619));
            }
            var baseobj = await _inteVehicleTypeRepository.GetByIdAsync(inteVehicle.VehicleTypeId);
            view.VehicleType = baseobj;
            view.Capacity = baseobj.CellQty;
            view.Vehicle = inteVehicle;
            var inteVehicleFreights = await _inteVehicleFreightRepository.GetByVehicleIdsAsync(new long[] { inteVehicle.Id });
            var inteVehicleFreightDtos = new List<InteVehicleFreightDto>();
            if (inteVehicleFreights != null && inteVehicleFreights.Any())
            {
                foreach (var item in inteVehicleFreights)
                {
                    inteVehicleFreightDtos.Add(item.ToModel<InteVehicleFreightDto>());
                }
                //获取托盘所有条码记录
                var vsr = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
                {
                    VehicleId = inteVehicle.Id,
                    SiteId = _currentSite.SiteId.Value
                });
                foreach (var item in inteVehicleFreightDtos)
                {
                    var lst = vsr.Where(i => i.LocationId == item.Id).ToList();
                    item.Stacks = lst;
                }
            }
            view.Stacks = inteVehicleFreightDtos;
            return view;
        }

        /// <summary>
        /// 载具操作
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task VehicleOperationAsync(InteVehicleOperationDto dto)
        {
            //校验托盘是否可用
            var v = await _inteVehicleRepository.GetByCodeAsync(new InteVehicleCodeQuery()
            {
                Code = dto.PalletNo,
                SiteId = _currentSite.SiteId??123456
            });

            if (v == null || v.Status == DisableOrEnableEnum.Disable)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18617));
            }

            switch (dto.OperationType)
            {
                case  Core.Enums.Integrated.VehicleOperationEnum.Bind: {  await VehicleBindOperationAsync(dto);  } break;
                case Core.Enums.Integrated.VehicleOperationEnum.Unbind : { await VehicleUnBindOperationAsync(dto); } break;
                case Core.Enums.Integrated.VehicleOperationEnum.Clear: { await VehicleClearAsync(dto); } break;
            }
            ThreadPool.QueueUserWorkItem(async o =>
            {
                var foo = new InteVehicleFreightRecordEntity()
                {
                    CreatedBy = _currentUser.UserName,
                    CreatedOn = HymsonClock.Now(),
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = _currentSite.SiteId ?? 123456,
                    VehicleId = v.Id
                };
                switch (dto.OperationType)
                {
                    case Core.Enums.Integrated.VehicleOperationEnum.Bind: 
                        { 
                            var bind = dto as InteVehicleBindOperationDto;
                            foo.BarCode = bind.SFC;
                            foo.LocationId = bind.LocationId;
                            foo.OperateType = (int)Core.Enums.Integrated.VehicleOperationEnum.Bind;

                        }
                        break;
                    case Core.Enums.Integrated.VehicleOperationEnum.Unbind:
                        {
                            var bind = dto as InteVehicleUnbindOperationDto;
                            foo.BarCode = string.Join(",", bind.StackIds);
                            foo.LocationId = bind.LocationId;
                            foo.OperateType = (int)Core.Enums.Integrated.VehicleOperationEnum.Unbind;
                        }
                        break;
                    case Core.Enums.Integrated.VehicleOperationEnum.Clear:
                        {
                            var bind = dto as InteVehicleClearOperationDto;
                            foo.OperateType = (int)Core.Enums.Integrated.VehicleOperationEnum.Clear;
                        }
                        break;
                }

                await _inteVehicleFreightRecordRepository.InsertAsync(foo);
            });
        }

        /// <summary>
        /// 绑盘操作
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private async Task VehicleBindOperationAsync(InteVehicleOperationDto ivo)
        {
            /* 指定位置绑定条码
             * inte_vehicle_freight表 更新已装载数量信息
             * 条码存放在inte_vehice_freight_stack表中
             */
            // 条码 是否在制品校验
          
            var  dto = ivo as InteVehicleBindOperationDto;
            //验证DTO
          //  await _validationCreateRules.ValidateAndThrowAsync(dto);
            var manuSfcProduceEntity = await _manuSfcProduceRepository.GetBySFCAsync(new ManuSfcProduceBySfcQuery()
            {
                Sfc = dto.SFC,
                SiteId = _currentSite.SiteId??123456
            });

            if (manuSfcProduceEntity == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES19918)).WithData("SFC", dto.SFC);
            }
            //绑盘前校验 该条码是否已绑盘
            var check1 = await _inteVehiceFreightStackRepository.GetBySFCAsync(dto.SFC);
            if (check1 != null)
            {
                var v1 = await _inteVehicleRepository.GetByIdAsync(check1.VehicleId);
                throw new CustomerValidationException(nameof(ErrorCode.MES18616)).WithData("sfc", dto.SFC).WithData("palletNo", v1.Code);
            }


            var inteVehicleEntity = await _inteVehicleRepository.GetByCodeAsync(new InteVehicleCodeQuery()
            {
                Code = dto.PalletNo,
                SiteId = _currentSite.SiteId??123456
            });
            var vtr = await _inteVehicleTypeRepository.GetByIdAsync(inteVehicleEntity.VehicleTypeId);

            //获取托盘所有条码记录
            var vsr = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
            {
                VehicleId = inteVehicleEntity.Id,
                SiteId = _currentSite.SiteId??123456
            });
            var foo = await _inteVehicleFreightRepository.GetByVehicleIdsAsync(new long[] { inteVehicleEntity.Id });
            var count = foo.Where(i => i.Status == true).ToList().Count();
            if (vsr.Count() >= count * vtr.CellQty)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES18613));
            }
            else
            {
                //获取指定位置信息
                var stack = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
                {
                    LocationId = dto.LocationId,
                    SiteId = _currentSite.SiteId ?? 123456
                });
                if (stack.Count() >= vtr.CellQty)
                {
                    throw new CustomerValidationException(nameof(ErrorCode.MES18614));
                }
                else
                {
                    var stackentity = new InteVehicleFreightStackEntity()
                    {
                        BarCode = dto.SFC,
                        CreatedBy = _currentUser.UserName,
                        UpdatedBy = _currentUser.UserName,
                        CreatedOn = HymsonClock.Now(),
                        UpdatedOn = HymsonClock.Now(),
                        SiteId = _currentSite.SiteId ?? 0,
                        Id = IdGenProvider.Instance.CreateId(),
                        LocationId = dto.LocationId,
                        VehicleId = inteVehicleEntity.Id,
                        IsDeleted = 0
                    };
                    await _inteVehiceFreightStackRepository.InsertAsync(stackentity);
                }
            }

            var loc = await _inteVehicleFreightRepository.GetByIdAsync(dto.LocationId);

            loc.Qty += 1;
            loc.UpdatedBy = _currentUser.UserName;
            loc.UpdatedOn = HymsonClock.Now();
            await _inteVehicleFreightRepository.UpdateAsync(loc);
        }

        /// <summary>
        /// 载具解绑
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private async Task VehicleUnBindOperationAsync(InteVehicleOperationDto ivo)
        {
            var dto = ivo as InteVehicleUnbindOperationDto;
            var inteVehicleEntity = await _inteVehicleRepository.GetByCodeAsync(new InteVehicleCodeQuery()
            {
                Code = dto.PalletNo,
                SiteId = _currentSite.SiteId ?? 123456
            });
            var vtr = await _inteVehicleTypeRepository.GetByIdAsync(inteVehicleEntity.VehicleTypeId);

            //获取指定位置信息
            var loc = await _inteVehicleFreightRepository.GetByIdAsync(dto.LocationId);
            var stack = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
            {
                LocationId = dto.LocationId,
                SiteId = _currentSite.SiteId ?? 123456
            });
            if(dto.StackIds.Any())
            {
                using var trans = TransactionHelper.GetTransactionScope();
                await _inteVehiceFreightStackRepository.DeletesAsync(dto.StackIds.ToArray());

                loc.Qty -= stack.Count(s=>dto.StackIds.Contains(s.Id));
                loc.UpdatedBy = _currentUser.UserName;
                loc.UpdatedOn = HymsonClock.Now();
                await _inteVehicleFreightRepository.UpdateAsync(loc);
                trans.Complete();
            }

        }

        /// <summary>
        /// 载具清盘
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private async Task VehicleClearAsync(InteVehicleOperationDto ivo)
        {
            var dto = ivo as InteVehicleClearOperationDto;
            var inteVehicleEntity = await _inteVehicleRepository.GetByCodeAsync(new InteVehicleCodeQuery()
            {
                Code = dto.PalletNo,
                SiteId = _currentSite.SiteId??123456
            });
            var vtr = await _inteVehicleTypeRepository.GetByIdAsync(inteVehicleEntity.VehicleTypeId);


            var vsr = await _inteVehiceFreightStackRepository.GetInteVehiceFreightStackEntitiesAsync(new InteVehiceFreightStackQuery()
            {
                VehicleId = inteVehicleEntity.Id,
                SiteId = _currentSite.SiteId??123456
            });

            if (vsr != null && vsr.Any())
            {
                await _inteVehiceFreightStackRepository.DeletesAsync(vsr.Select(v => v.Id).ToArray());
            }

            var foo = await _inteVehicleFreightRepository.GetByVehicleIdsAsync(new long[] { inteVehicleEntity.Id });
            if (foo != null && foo.Any())
            {
                var bar = foo.ToList();
                bar.ForEach(i =>
                {
                    i.Qty = 0;
                    i.UpdatedBy = _currentUser.UserName;
                    i.UpdatedOn = HymsonClock.Now();
                });
                await _inteVehicleFreightRepository.UpdatesAsync((bar));
            }
        }


    }
}
