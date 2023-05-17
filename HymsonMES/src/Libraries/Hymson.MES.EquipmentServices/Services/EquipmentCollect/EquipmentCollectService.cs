using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Core.Domain.Manufacture;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Enums;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Manufacture;
using Hymson.MES.Data.Repositories.Process;
using Hymson.MES.EquipmentServices.Bos;
using Hymson.MES.EquipmentServices.Dtos.EquipmentCollect;
using Hymson.Snowflake;
using Hymson.Utils;
using Hymson.Utils.Tools;
using Hymson.Web.Framework.WorkContext;

namespace Hymson.MES.EquipmentServices.Services.EquipmentCollect
{
    /// <summary>
    /// 设备服务
    /// </summary>
    public class EquipmentCollectService : IEquipmentCollectService
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICurrentEquipment _currentEquipment;

        /// <summary>
        /// 仓储（设备心跳）
        /// </summary>
        private readonly IEquHeartbeatRepository _equipmentHeartbeatRepository;

        /// <summary>
        /// 仓储（设备报警）
        /// </summary>
        private readonly IEquAlarmRepository _equipmentAlarmRepository;

        /// <summary>
        /// 仓储（设备状态）
        /// </summary>
        private readonly IEquStatusRepository _equipmentStatusRepository;

        /// <summary>
        /// 仓储（设备生产参数）
        /// </summary>
        private readonly IEquProductParameterRepository _equProductParameterRepository;

        /// <summary>
        /// 仓储（资源维护）
        /// </summary>
        private readonly IProcResourceRepository _procResourceRepository;

        /// <summary>
        /// 仓储（标准参数）
        /// </summary>
        private readonly IProcParameterRepository _procParameterRepository;

        /// <summary>
        /// 仓储（标准参数）
        /// </summary>
        private readonly IManuProductParameterRepository _manuProductParameterRepository;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="currentEquipment"></param>
        /// <param name="equipmentHeartbeatRepository"></param>
        /// <param name="equipmentAlarmRepository"></param>
        /// <param name="equipmentStatusRepository"></param>
        /// <param name="equProductParameterRepository"></param>
        /// <param name="procResourceRepository"></param>
        /// <param name="procParameterRepository"></param>
        /// <param name="manuProductParameterRepository"></param>
        public EquipmentCollectService(ICurrentEquipment currentEquipment,
            IEquHeartbeatRepository equipmentHeartbeatRepository,
            IEquAlarmRepository equipmentAlarmRepository,
            IEquStatusRepository equipmentStatusRepository,
            IEquProductParameterRepository equProductParameterRepository,
            IProcResourceRepository procResourceRepository,
            IProcParameterRepository procParameterRepository,
            IManuProductParameterRepository manuProductParameterRepository)
        {
            _currentEquipment = currentEquipment;
            _equipmentHeartbeatRepository = equipmentHeartbeatRepository;
            _equipmentAlarmRepository = equipmentAlarmRepository;
            _equipmentStatusRepository = equipmentStatusRepository;
            _equProductParameterRepository = equProductParameterRepository;
            _procResourceRepository = procResourceRepository;
            _procParameterRepository = procParameterRepository;
            _manuProductParameterRepository = manuProductParameterRepository;
        }


        /// <summary>
        /// 设备心跳
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentHeartbeatAsync(EquipmentHeartbeatDto request)
        {
            // TODO
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();

            var entity = new EquHeartbeatEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                Status = request.IsOnline,
                LastOnLineTime = request.LocalTime
            };

            using var trans = TransactionHelper.GetTransactionScope();
            await _equipmentHeartbeatRepository.InsertAsync(entity);
            await _equipmentHeartbeatRepository.InsertRecordAsync(new EquHeartbeatRecordEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = entity.SiteId,
                CreatedBy = entity.CreatedBy,
                CreatedOn = entity.CreatedOn,
                UpdatedBy = entity.UpdatedBy,
                UpdatedOn = entity.UpdatedOn,
                EquipmentId = entity.EquipmentId,
                LocalTime = request.LocalTime,
                Status = entity.Status
            });
            trans.Complete();
        }

        /// <summary>
        /// 设备状态监控
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentStateAsync(EquipmentStateDto request)
        {
            // TODO
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();

            await UpdateEquipmentStatusAsync(new EquStatusEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                LocalTime = request.LocalTime,
                EquipmentStatus = request.StateCode
            });
        }

        /// <summary>
        /// 设备报警
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentAlarmAsync(EquipmentAlarmDto request)
        {
            // TODO
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();

            await _equipmentAlarmRepository.InsertAsync(new EquAlarmEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                LocalTime = request.LocalTime,
                FaultCode = request.AlarmCode,
                AlarmMsg = request.AlarmMsg ?? "",
                Status = request.Status
            });
        }

        /// <summary>
        /// 设备停机原因
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentDownReasonAsync(EquipmentDownReasonDto request)
        {
            // TODO
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();

            await UpdateEquipmentStatusAsync(new EquStatusEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = _currentEquipment.SiteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                LocalTime = request.LocalTime,
                EquipmentStatus = EquipmentStateEnum.DownNormal, // 暂定为正常停机
                LossRemark = request.DownReasonCode.GetDescription(),
                BeginTime = request.BeginTime,
                EndTime = request.EndTime
            });
        }



        /// <summary>
        /// 设备过程参数采集(无在制品条码)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentProcessParamAsync(EquipmentProcessParamDto request)
        {
            // TODO
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();
            var siteId = _currentEquipment.SiteId;

            if (request.ParamList == null || request.ParamList.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES19107));

            // 查询设备参数
            var paramCodes = request.ParamList.Select(s => s.ParamCode);
            var parameterEntities = await _procParameterRepository.GetByCodesAsync(new EntityByCodesQuery
            {
                Site = siteId,
                Codes = paramCodes,
            });

            // 找出不在数据库里面的Code
            var noIncludeCodes = paramCodes.Where(w => parameterEntities.Select(s => s.ParameterCode).Contains(w) == false);
            if (noIncludeCodes.Any() == true) throw new CustomerValidationException(nameof(ErrorCode.MES19108)).WithData("Code", string.Join(',', noIncludeCodes));

            // 查询资源
            var resourceEntity = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = siteId,
                Code = request.ResourceCode,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19109)).WithData("Code", request.ResourceCode);

            var entitis = request.ParamList.Select(s => new EquProductParameterEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = siteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                LocalTime = request.LocalTime,

                //ProcedureId = 0,
                ResourceId = resourceEntity.Id,
                ParameterId = GetParameterIdByParameterCode(s.ParamCode, parameterEntities),
                ParamValue = s.ParamValue,
                Timestamp = s.Timestamp
            });

            await _equProductParameterRepository.InsertsAsync(entitis);
        }

        /// <summary>
        /// 设备产品过程参数采集(无在制品条码)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentProductProcessParamInNotCanSFCAsync(EquipmentProductProcessParamInNotCanSFCDto request)
        {
            // TODO
            await Task.CompletedTask;
        }

        /// <summary>
        /// 设备产品过程参数采集
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task EquipmentProductProcessParamAsync(EquipmentProductProcessParamDto request)
        {
            // TODO
            var userCode = request.EquipmentCode; //_currentEquipment.Code
            var nowTime = HymsonClock.Now();
            var siteId = _currentEquipment.SiteId;

            if (request.SFCParams == null || request.SFCParams.Any() == false) throw new CustomerValidationException(nameof(ErrorCode.MES19110));
            if (request.SFCParams.Any(a => a.ParamList == null || a.ParamList.Any() == false)) throw new CustomerValidationException(nameof(ErrorCode.MES19107));

            // 查询设备参数
            List<EquipmentProductParamBo> paramList = new();
            foreach (var item in request.SFCParams)
            {
                paramList.AddRange(item.ParamList.Select(s => new EquipmentProductParamBo
                {
                    SFC = item.SFC,
                    ParamCode = s.ParamCode,
                    ParamValue = s.ParamValue,
                    Timestamp = s.Timestamp
                }));
            }

            var paramCodes = paramList.Select(s => s.ParamCode);
            var parameterEntities = await _procParameterRepository.GetByCodesAsync(new EntityByCodesQuery
            {
                Site = siteId,
                Codes = paramCodes,
            });

            // 找出不在数据库里面的Code
            var noIncludeCodes = paramCodes.Where(w => parameterEntities.Select(s => s.ParameterCode).Contains(w) == false);
            if (noIncludeCodes.Any() == true) throw new CustomerValidationException(nameof(ErrorCode.MES19108)).WithData("Code", string.Join(',', noIncludeCodes));

            // 查询资源
            var resourceEntity = await _procResourceRepository.GetByCodeAsync(new EntityByCodeQuery
            {
                Site = siteId,
                Code = request.ResourceCode,
            }) ?? throw new CustomerValidationException(nameof(ErrorCode.MES19109)).WithData("Code", request.ResourceCode);

            var entitis = paramList.Select(s => new ManuProductParameterEntity
            {
                Id = IdGenProvider.Instance.CreateId(),
                SiteId = siteId,
                CreatedBy = userCode,
                CreatedOn = nowTime,
                UpdatedBy = userCode,
                UpdatedOn = nowTime,
                EquipmentId = _currentEquipment.Id ?? 0,
                LocalTime = request.LocalTime,

                //ProcedureId = 0,
                SFC = s.SFC,
                ResourceId = resourceEntity.Id,
                ParameterId = GetParameterIdByParameterCode(s.ParamCode, parameterEntities),
                ParamValue = s.ParamValue,
                Timestamp = s.Timestamp
            });

            await _manuProductParameterRepository.InsertsAsync(entitis);
        }



        #region 内部方法
        /// <summary>
        /// 更新设备状态
        /// </summary>
        /// <param name="currentStatusEntity"></param>
        /// <returns></returns>
        private async Task UpdateEquipmentStatusAsync(EquStatusEntity currentStatusEntity)
        {
            // 最近的状态记录
            var lastStatusEntity = await _equipmentStatusRepository.GetLastEntityByEquipmentIdAsync(currentStatusEntity.EquipmentId);

            // 开启事务
            using var trans = TransactionHelper.GetTransactionScope();

            // 更新设备当前状态
            await _equipmentStatusRepository.InsertAsync(currentStatusEntity);

            // 更新统计表
            if (lastStatusEntity != null && lastStatusEntity.EquipmentStatus != lastStatusEntity.EquipmentStatus)
            {
                await _equipmentStatusRepository.InsertStatisticsAsync(new EquStatusStatisticsEntity
                {
                    Id = IdGenProvider.Instance.CreateId(),
                    SiteId = currentStatusEntity.SiteId,
                    CreatedBy = currentStatusEntity.CreatedBy,
                    CreatedOn = currentStatusEntity.CreatedOn,
                    UpdatedBy = currentStatusEntity.UpdatedBy,
                    UpdatedOn = currentStatusEntity.UpdatedOn,
                    EquipmentId = currentStatusEntity.EquipmentId,
                    EquipmentStatus = lastStatusEntity.EquipmentStatus,
                    SwitchEquipmentStatus = currentStatusEntity.EquipmentStatus,
                    BeginTime = lastStatusEntity.LocalTime,
                    EndTime = currentStatusEntity.LocalTime
                });
            }

            trans.Complete();
        }

        /// <summary>
        /// 根据参数编码获取参数Id
        /// </summary>
        /// <param name="parameterCode"></param>
        /// <param name="parameterEntities"></param>
        /// <returns></returns>
        private static long GetParameterIdByParameterCode(string parameterCode, IEnumerable<ProcParameterEntity> parameterEntities)
        {
            var entity = parameterEntities.FirstOrDefault(f => f.ParameterCode == parameterCode);
            if (entity == null) return 0;

            return entity.Id;
        }
        #endregion


    }
}
