using Dapper;
using Hymson.DbConnection.Abstractions;
using Hymson.MES.Core.Domain.Equipment;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.Query;
using Hymson.MES.Data.Repositories.Equipment.EquEquipment.View;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Equipment.EquEquipment
{
    /// <summary>
    /// 设备表仓储
    /// </summary>
    public partial class EquEquipmentRepository
    {
        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源，资源类型，工序，线体，车间 基础信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquEquipmentResAllView> GetEquResAllAsync(EquResAllQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentResAllView>(GetEquResAllSql, query);
        }

        /// <summary>
        /// 查多个-根据设备编码+资源编码查询 设备，资源，资源类型，工序，线体，车间 基础信息
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentResAllView>> GetMultEquResAllAsync(MultEquResAllQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquEquipmentResAllView>(GetMultEquResAllSql, query);
        }

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquEquipmentResAllView> GetEquResAsync(EquResAllQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentResAllView>(GetEquResSql, query);
        }

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<EquEquipmentResAllView> GetEquResLineAsync(EquResAllQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentResAllView>(GetEquResLineSql, query);
        }

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源，工序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<EquEquipmentResAllView> GetEquResProcedureAsync(EquResAllQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<EquEquipmentResAllView>(GetEquResProcedureSql, query);
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EquEquipmentEntity>> GetBySiteIdAsync(EquQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<EquEquipmentEntity>(GetEquBySiteIdSql, query);
        }

        /// <summary>
        /// 获取设备列表
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<string> GetEquTokenSqlAsync(EquResAllQuery query)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstAsync<string>(GetEquTokenSql, query);
        }
    }

    /// <summary>
    /// 顷刻能源设备功能
    /// </summary>
    public partial class EquEquipmentRepository
    {
        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源，资源类型，工序，线体，车间 基础信息
        /// </summary>
        const string GetEquResAllSql = $@"
            select  t1.id EquipmentId, t1.EquipmentCode, t1.EquipmentName ,t1.SiteId,
		            t3.id ResId, t3.ResCode ,t3.ResName ,
		            t4.id ResTypeId, t4.ResType ,t4.ResTypeName ,
		            t5.id procedureId ,t5.Code procedureCode, t5.Name procedureName  ,
		            t7.id LineId, t7.Code LineWorkCenterCode, t7.Name LineWorkCenterName,
	                t9.id WorkShopId, t9.Code WorkShopCode, t9.Name WorkShopName
            from equ_equipment t1
            inner join proc_resource_equipment_bind t2 on t1.Id = t2.EquipmentId and t2.IsDeleted = 0 and t2.IsMain = 1
            inner join proc_resource t3 on t3.Id = t2.ResourceId and t3.IsDeleted = 0
            inner join proc_resource_type t4 on t4.Id = t3.ResTypeId and t4.IsDeleted = 0
            inner join proc_procedure t5 on t5.ResourceTypeId = t3.ResTypeId and t5.IsDeleted = 0
            inner join inte_work_center_resource_relation t6 on t6.ResourceId = t3.Id and t6.IsDeleted = 0
            inner join inte_work_center t7 on t7.Id = t6.WorkCenterId and t7.IsDeleted = 0  
            inner join inte_work_center_relation t8 on t8.SubWorkCenterId = t7.Id and t8.IsDeleted = 0
            inner join inte_work_center t9 on t9.Id = t8.WorkCenterId and t8.IsDeleted = 0
            where t1.EquipmentCode = @EquipmentCode
            and t1.IsDeleted = 0
            and t3.ResCode = @ResCode
        ";

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源，资源类型，工序，线体，车间 基础信息
        /// </summary>
        const string GetMultEquResAllSql = $@"
            select  t1.id EquipmentId, t1.EquipmentCode, t1.EquipmentName ,t1.SiteId,
		            t3.id ResId, t3.ResCode ,t3.ResName ,
		            t4.id ResTypeId, t4.ResType ,t4.ResTypeName ,
		            t5.id procedureId ,t5.Code procedureCode, t5.Name procedureName  ,
		            t7.id LineId, t7.Code LineWorkCenterCode, t7.Name LineWorkCenterName,
	                t9.id WorkShopId, t9.Code WorkShopCode, t9.Name WorkShopName
            from equ_equipment t1
            inner join proc_resource_equipment_bind t2 on t1.Id = t2.EquipmentId and t2.IsDeleted = 0 and t2.IsMain = 1
            inner join proc_resource t3 on t3.Id = t2.ResourceId and t3.IsDeleted = 0
            inner join proc_resource_type t4 on t4.Id = t3.ResTypeId and t4.IsDeleted = 0
            inner join proc_procedure t5 on t5.ResourceTypeId = t3.ResTypeId and t5.IsDeleted = 0
            inner join inte_work_center_resource_relation t6 on t6.ResourceId = t3.Id and t6.IsDeleted = 0
            inner join inte_work_center t7 on t7.Id = t6.WorkCenterId and t7.IsDeleted = 0  
            inner join inte_work_center_relation t8 on t8.SubWorkCenterId = t7.Id and t8.IsDeleted = 0
            inner join inte_work_center t9 on t9.Id = t8.WorkCenterId and t8.IsDeleted = 0
            where t1.EquipmentCode in @EquipmentCodeList
            and t1.IsDeleted = 0
            and t3.ResCode in @ResCodeList
        ";

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源
        /// </summary>
        const string GetEquResSql = $@"
            select  t1.id EquipmentId, t1.EquipmentCode, t1.EquipmentName ,t1.SiteId,
		            t3.id ResId, t3.ResCode ,t3.ResName
            from equ_equipment t1
            inner join proc_resource_equipment_bind t2 on t1.Id = t2.EquipmentId and t2.IsDeleted = 0 and t2.IsMain = 1
            inner join proc_resource t3 on t3.Id = t2.ResourceId and t3.IsDeleted = 0
            where t1.EquipmentCode = @EquipmentCode
            and t1.IsDeleted = 0
            and t3.ResCode = @ResCode
        ";

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源，线体，车间 基础信息
        /// </summary>
        const string GetEquResLineSql = $@"
            select  t1.id EquipmentId, t1.EquipmentCode, t1.EquipmentName ,t1.SiteId,
		            t3.id ResId, t3.ResCode ,t3.ResName ,
		            t7.id LineId, t7.Code LineWorkCenterCode, t7.Name LineWorkCenterName
            from equ_equipment t1
            inner join proc_resource_equipment_bind t2 on t1.Id = t2.EquipmentId and t2.IsDeleted = 0 and t2.IsMain = 1
            inner join proc_resource t3 on t3.Id = t2.ResourceId and t3.IsDeleted = 0
            inner join inte_work_center_resource_relation t6 on t6.ResourceId = t3.Id and t6.IsDeleted = 0
            inner join inte_work_center t7 on t7.Id = t6.WorkCenterId and t7.IsDeleted = 0  
            where t1.EquipmentCode = @EquipmentCode
            and t1.IsDeleted = 0
            and t3.ResCode = @ResCode
        ";

        /// <summary>
        /// 根据设备编码+资源编码查询 设备，资源，线体，车间 基础信息
        /// </summary>
        const string GetEquResProcedureSql = $@"
            select  t1.id EquipmentId, t1.EquipmentCode, t1.EquipmentName ,t1.SiteId,
		            t3.id ResId, t3.ResCode ,t3.ResName ,
		            t4.id ResTypeId, t4.ResType ,t4.ResTypeName ,
		            t5.id procedureId ,t5.Code procedureCode, t5.Name procedureName
            from equ_equipment t1
            inner join proc_resource_equipment_bind t2 on t1.Id = t2.EquipmentId and t2.IsDeleted = 0 and t2.IsMain = 1
            inner join proc_resource t3 on t3.Id = t2.ResourceId and t3.IsDeleted = 0
            inner join proc_resource_type t4 on t4.Id = t3.ResTypeId and t4.IsDeleted = 0
            inner join proc_procedure t5 on t5.ResourceTypeId = t3.ResTypeId and t5.IsDeleted = 0
            where t1.EquipmentCode = @EquipmentCode
            and t1.IsDeleted = 0
            and t3.ResCode = @ResCode
        ";

        /// <summary>
        /// 查询工厂所有设备
        /// </summary>
        const string GetEquBySiteIdSql = "select * from equ_equipment where SiteId = @SiteId and IsDeleted = 0";

        /// <summary>
        /// 获取设备token
        /// </summary>
        const string GetEquTokenSql = $@"
            select t1.Token
            from equ_equipment_token t1
            inner join equ_equipment t2 on t1.EquipmentId = t2.Id and t2.IsDeleted = 0
            where t1.IsDeleted = 0
            and t2.EquipmentCode = @EquipmentCode
        ";
    }
}
