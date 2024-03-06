using Hymson.Infrastructure.Exceptions;
using Hymson.MES.Core.Constants;
using Hymson.MES.Data.Repositories.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Hymson.MES.EquipmentServices.Dtos.Qkny.Common;
using Hymson.MES.Services.Dtos.EquEquipmentLoginRecord;
using Hymson.MES.Services.Services.EquEquipmentLoginRecord;
using Hymson.Snowflake;
using Hymson.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Hymson.MES.EquipmentServices.Services.Qkny
{
    /// <summary>
    /// 顷刻设备服务
    /// </summary>
    public class QknyService : IQknyService
    {
        /// <summary>
        /// 仓储接口（设备注册）
        /// </summary>
        private readonly IEquEquipmentRepository _equEquipmentRepository;

        /// <summary>
        /// 设备验证
        /// </summary>
        private readonly IEquEquipmentVerifyRepository _equEquipmentVerifyRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IEquEquipmentLoginRecordService _equEquipmentLoginRecordService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public QknyService(IEquEquipmentRepository equEquipmentRepository,
            IEquEquipmentVerifyRepository equEquipmentVerifyRepository,
            IEquEquipmentLoginRecordService equEquipmentLoginRecordService)
        {
            _equEquipmentRepository = equEquipmentRepository;
            _equEquipmentVerifyRepository = equEquipmentVerifyRepository;
            _equEquipmentLoginRecordService = equEquipmentLoginRecordService;
        }

        /// <summary>
        /// 操作员登录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task OperatorLoginAsync(OperationLoginDto dto)
        {
            //1. 获取设备基础信息
            EquEquipmentResAllView equResModel = await GetEquResAllAsync(dto);
            //2. 校验用户名密码是否和设备匹配(equ_equipment_verify)
            var verifyList = await _equEquipmentVerifyRepository.GetEquipmentVerifyByEquipmentIdAsync(equResModel.EquipmentId);
            if (verifyList == null || !verifyList.Any())
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45011)).WithData("EquipmentCode", dto.EquipmentCode); ;
            }
            bool verifyCheck = verifyList.Where(m => m.Account == dto.OperatorUserID && m.Password == dto.OperatorPassword).Any();
            if (verifyCheck == false)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45012)).WithData("EquipmentCode", dto.EquipmentCode); ;
            }
            EquEquipmentLoginRecordSaveDto loginRecordDto = new EquEquipmentLoginRecordSaveDto();
            loginRecordDto.Id = IdGenProvider.Instance.CreateId();
            loginRecordDto.SiteId = equResModel.SiteId;
            loginRecordDto.Account = dto.OperatorUserID;
            loginRecordDto.Password = dto.OperatorPassword;
            loginRecordDto.EquipmentId = equResModel.EquipmentId;
            loginRecordDto.CreateOn = HymsonClock.Now();
            loginRecordDto.CreateBy = equResModel.EquipmentCode;
            loginRecordDto.UpdateOn = loginRecordDto.CreateOn;
            loginRecordDto.UpdateBy = equResModel.EquipmentCode;
            await _equEquipmentLoginRecordService.AddAsync(loginRecordDto);


            //TODO 业务逻辑
            //1. 校验用户名密码是否和设备匹配(equ_equipment_verify)
            //2. 新增 equ_equipment_login_record 表，记录用户登录时间，统计每个用户的使用时间
            //3. 新增 equ_equipment_newest_info 记录设备最后登录时间
        }

        /// <summary>
        /// 获取设备资源对应的基础信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task<EquEquipmentResAllView> GetEquResAllAsync(QknyBaseDto param)
        {
            EquResAllQuery query = new EquResAllQuery();
            //query.EquipmentCode = "XXLPACK04AE002";
            //query.ResCode = "ZY-0410021";
            query.EquipmentCode = param.EquipmentCode;
            query.ResCode = param.ResourceCode;
            EquEquipmentResAllView equResAllModel = await _equEquipmentRepository.GetEquResAllAsync(query);
            if (equResAllModel == null)
            {
                throw new CustomerValidationException(nameof(ErrorCode.MES45001));
            }
            return equResAllModel;
        }
    }
}
