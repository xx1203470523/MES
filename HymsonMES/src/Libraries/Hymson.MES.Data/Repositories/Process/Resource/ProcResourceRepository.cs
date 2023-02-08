using Dapper;
using Google.Protobuf.WellKnownTypes;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.OnStock;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.OnStock;
using Hymson.MES.Data.Repositories.Process.ResourceType;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hymson.MES.Data.Repositories.Process
{
    /// <summary>
    /// 资源维护表仓储层处理
    /// @tableName proc_resource
    /// @author zhaoqing
    /// @date 2023-02-08
    /// </summary>
    public partial class ProcResourceRepository : IProcResourceRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionOptions"></param>
        public ProcResourceRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcResourceEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProcResourceEntity> GetProcResrouces(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<ProcResourceEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        ///  查询资源类型维护表列表(关联资源：一个类型被多个资源关联就展示多条)
        /// </summary>
        /// <param name="procResourceTypePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceTypeView>> GetPageListAsync(ProcResourceTypePagedQuery procResourceTypePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("a.IsDeleted=0");
            //sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.SiteCode))
            {
                sqlBuilder.Where("a.SiteCode=@SiteCode");
            }
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.ResType))
            {
                procResourceTypePagedQuery.ResType = $"%{procResourceTypePagedQuery.ResType}%";
                sqlBuilder.Where("ResType like @ResType");
            }
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.ResTypeName))
            {
                procResourceTypePagedQuery.ResTypeName = $"%{procResourceTypePagedQuery.ResTypeName}%";
                sqlBuilder.Where("ResTypeName like @ResTypeName");
            }
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.ResCode))
            {
                procResourceTypePagedQuery.ResCode = $"%{procResourceTypePagedQuery.ResCode}%";
                sqlBuilder.Where("ResCode like @ResCode");
            }
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.ResName))
            {
                procResourceTypePagedQuery.ResName = $"%{procResourceTypePagedQuery.ResName}%";
                sqlBuilder.Where("ResName like @ResName");
            }

            var offSet = (procResourceTypePagedQuery.PageIndex - 1) * procResourceTypePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procResourceTypePagedQuery.PageSize });
            sqlBuilder.AddParameters(procResourceTypePagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceTypeEntitiesTask = conn.QueryAsync<ProcResourceTypeView>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceTypeEntities = await procResourceTypeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceTypeView>(procResourceTypeEntities, procResourceTypePagedQuery.PageIndex, procResourceTypePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 获取资源类型分页列表
        /// </summary>
        /// <param name="procResourceTypePagedQuery"></param>
        /// <returns></returns>
        public async Task<PagedInfo<ProcResourceTypeEntity>> GetListAsync(ProcResourceTypePagedQuery procResourceTypePagedQuery)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedListSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedListCountSqlTemplate);
            sqlBuilder.Where("IsDeleted=0");
            sqlBuilder.Select("*");
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.SiteCode))
            {
                sqlBuilder.Where("SiteCode=@SiteCode");
            }
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.ResType))
            {
                procResourceTypePagedQuery.ResType = $"%{procResourceTypePagedQuery.ResType}%";
                sqlBuilder.Where("ResType like @ResType");
            }
            if (!string.IsNullOrWhiteSpace(procResourceTypePagedQuery.ResTypeName))
            {
                procResourceTypePagedQuery.ResTypeName = $"%{procResourceTypePagedQuery.ResTypeName}%";
                sqlBuilder.Where("ResTypeName like @ResTypeName");
            }
            var offSet = (procResourceTypePagedQuery.PageIndex - 1) * procResourceTypePagedQuery.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = procResourceTypePagedQuery.PageSize });
            sqlBuilder.AddParameters(procResourceTypePagedQuery);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var procResourceTypeEntitiesTask = conn.QueryAsync<ProcResourceTypeEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var procResourceTypeEntities = await procResourceTypeEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<ProcResourceTypeEntity>(procResourceTypeEntities, procResourceTypePagedQuery.PageIndex, procResourceTypePagedQuery.PageSize, totalCount);
        }

        /// <summary>
        /// 添加资源类型数据
        /// </summary>
        /// <param name="resourceTypeEntity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(ProcResourceTypeAddCommand addCommand)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, addCommand);
        }

        /// <summary>
        /// 更新资源类型维护数据
        /// </summary>
        /// <param name="updateCommand"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProcResourceTypeUpdateCommand updateCommand)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, updateCommand);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long[] idsArr)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteSql, new { Id = idsArr });
        }
    }

    public partial class ProcResourceRepository
    {
        const string GetByIdSql = "select * from proc_resource where Id =@Id ";
        const string Get = "select `proc_resource WHERE `Id` in @Id and ResTypeId >0 ";

        const string GetPagedInfoDataSqlTemplate = "SELECT a.Id,a.SiteCode,ResType,ResTypeName,a.Remark,a.CreateBy ,a.CreateOn,b.ResCode,b.ResName  FROM proc_resource_type a left join proc_resource b on a.Id =b.ResTypeId /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedInfoCountSqlTemplate = "SELECT count(*) FROM proc_resource_type a left join proc_resource b on a.Id =b.ResTypeId  /**where**/ ";

        const string GetPagedListSqlTemplate = "SELECT /**select**/ FROM proc_resource /**innerjoin**/ /**leftjoin**/ /**where**/ LIMIT @Offset,@Rows";
        const string GetPagedListCountSqlTemplate = "SELECT COUNT(*) FROM proc_resource /**where**/";

        const string InsertSql = "INSERT INTO `proc_resource`(`Id`, `SiteCode`, `ResCode`, `ResName`,`Status`,`ResTypeId, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (@Id, @SiteCode, @ResCode, @ResName,@Status,@ResTypeId,@Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted);";
        const string UpdateSql = "UPDATE `proc_resource` SET ResName = @ResName,ResTypeId = @ResTypeId,ResName = @ResName, Remark = @Remark, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn WHERE Id = @Id;";
        const string DeleteSql = "UPDATE `proc_resource` SET `IsDeleted` = 1 WHERE `Id` in @Id;";
    }
}
