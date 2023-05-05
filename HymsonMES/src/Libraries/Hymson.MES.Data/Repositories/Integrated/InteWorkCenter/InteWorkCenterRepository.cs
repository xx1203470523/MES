using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Hymson.MES.Data.Repositories.Common.Query;
using Hymson.MES.Data.Repositories.Integrated.IIntegratedRepository;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.Query;
using Hymson.MES.Data.Repositories.Integrated.InteWorkCenter.View;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace Hymson.MES.Data.Repositories.Integrated.InteWorkCenter
{
    /// <summary>
    /// 工作中心表仓储
    /// @author admin
    /// @date 2023-02-22
    /// </summary>
    public partial class InteWorkCenterRepository : IInteWorkCenterRepository
    {
        private readonly ConnectionOptions _connectionOptions;

        public InteWorkCenterRepository(IOptions<ConnectionOptions> connectionOptions)
        {
            _connectionOptions = connectionOptions.Value;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<InteWorkCenterEntity>> GetPagedInfoAsync(InteWorkCenterPagedQuery param)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            if (string.IsNullOrEmpty(param.Sorting))
            {
                sqlBuilder.OrderBy("UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(param.Sorting);
            }
            sqlBuilder.Select("Id,SiteId,Code,Name,Type,Source,Status,IsMixLine,Remark,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted");

            if (param.SiteId.HasValue) { sqlBuilder.Where("SiteId = @SiteId"); }
            if (param.Type.HasValue) { sqlBuilder.Where("Type = @Type"); }
            if (param.Source.HasValue) { sqlBuilder.Where("Source = @Source"); }
            if (param.Status.HasValue) { sqlBuilder.Where("Status = @Status"); }
            if (!string.IsNullOrWhiteSpace(param.Code))
            {
                param.Code = $"%{param.Code}%";
                sqlBuilder.Where("Code like @Code");
            }
            if (!string.IsNullOrWhiteSpace(param.Name))
            {
                param.Name = $"%{param.Name}%";
                sqlBuilder.Where("Name like @Name");
            }
            if (!string.IsNullOrWhiteSpace(param.Remark))
            {
                param.Remark = $"%{param.Remark}%";
                sqlBuilder.Where("Remark like @Remark");
            }

            var offSet = (param.PageIndex - 1) * param.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = param.PageSize });
            sqlBuilder.AddParameters(param);

            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            var inteWorkCenterEntitiesTask = conn.QueryAsync<InteWorkCenterEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var inteWorkCenterEntities = await inteWorkCenterEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<InteWorkCenterEntity>(inteWorkCenterEntities, param.PageIndex, param.PageSize, totalCount);
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteWorkCenterEntity> GetByIdAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteWorkCenterEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteWorkCenterEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<InteWorkCenterEntity>(GetByIdsSql, new { ids });
        }

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<InteWorkCenterEntity> GetByCodeAsync(EntityByCodeQuery param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteWorkCenterEntity>(GetByCodeSql, param);
        }

        /// <summary>
        /// 根据资源ID获取数据
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<InteWorkCenterEntity> GetByResourceIdAsync(long resourceId)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteWorkCenterEntity>(GetByResourceId, new { resourceId });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(InteWorkCenterEntity param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, param);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertRangAsync(IEnumerable<InteWorkCenterEntity> param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertSql, param);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(InteWorkCenterEntity param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateSql, param);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangAsync(IEnumerable<InteWorkCenterEntity> param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(UpdateRangSql, param);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangAsync(DeleteCommand param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(DeleteRangSql, param);
        }

        #region 关联工作中心
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertInteWorkCenterRelationRangAsync(IEnumerable<InteWorkCenterRelation> param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InsertInteWorkCenterRelationRangSql, param);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> RealDelteInteWorkCenterRelationRangAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(RealDelteInteWorkCenterRelationSql, new { Id = id });
        }

        /// <summary>
        /// 获取工作中心关联
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteWorkCenterRelationView>> GetInteWorkCenterRelationAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<InteWorkCenterRelationView>(GetInteWorkCenterRelationSqlTemplate, new { Id = id });
        }

        /// <summary>
        /// 根据下级工作中心Id获取上级工作中心
        /// (只获取一级)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InteWorkCenterEntity> GetHigherInteWorkCenterAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryFirstOrDefaultAsync<InteWorkCenterEntity>(GetHigherInteWorkCenterSql, new { Id = id });
        }
        #endregion

        #region 关联资源
        /// <summary>
        /// 根据资源ID获取数据
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<long>> GetWorkCenterIdByResourceIdAsync(IEnumerable<long> resourceIds)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<long>(GetWorkCenterIdByResourceIdSql, new { resourceIds });
        }

        /// <summary>
        /// 查询产线下面的资源ID集合
        /// </summary>
        /// <param name="workCenterIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<long>> GetResourceIdsByWorkCenterIdAsync(long[] workCenterIds)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<long>(GetResourceIdsByWorkCenterIdSql, new { workCenterIds });
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertInteWorkCenterResourceRelationRangAsync(IEnumerable<InteWorkCenterResourceRelation> param)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(InteWorkCenterResourceRelationRangSql, param);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> RealDelteInteWorkCenterResourceRelationRangAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.ExecuteAsync(RealDelteInteWorkCenterResourceRelationSql, new { Id = id });
        }

        /// <summary>
        /// 获取工作中心关联
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteWorkCenterResourceRelationView>> GetInteWorkCenterResourceRelatioAsync(long id)
        {
            using var conn = new MySqlConnection(_connectionOptions.MESConnectionString);
            return await conn.QueryAsync<InteWorkCenterResourceRelationView>(GetInteWorkCenterResourceRelatioSqlTemplate, new { Id = id });
        }
        #endregion
    }

    /// <summary>
    /// 工作中心表SQL语句
    /// @author admin
    /// @date 2023-02-22
    public partial class InteWorkCenterRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `inte_work_center` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `inte_work_center` /**where**/ ";
        const string InsertSql = "INSERT INTO  `inte_work_center` ( Id,SiteId,Code,Name,Type,Source,Status,IsMixLine,Remark,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted) VALUES ( @Id,@SiteId,@Code,@Name,@Type,@Source,@Status,@IsMixLine,@Remark,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted) ";
        const string UpdateSql = "UPDATE `inte_work_center` SET  Name=@Name,Type=@Type,Source=@Source,Status=@Status,IsMixLine=@IsMixLine,Remark=@Remark,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id = @Id AND IsDeleted = @IsDeleted ";
        const string UpdateRangSql = "UPDATE `inte_work_center` SET Name=@Name,Type=@Type,Source=@Source,Status=@Status,IsMixLine=@IsMixLine,Remark=@Remark,UpdatedBy=@UpdatedBy,UpdatedOn=@UpdatedOn WHERE Id = @Id AND IsDeleted = @IsDeleted ";
        const string DeleteRangSql = "UPDATE `inte_work_center` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids AND IsDeleted=0";
        const string GetByIdSql = @"SELECT Id,SiteId,Code,Name,Type,Source,Status,IsMixLine,Remark,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted FROM `inte_work_center`  WHERE Id = @Id AND IsDeleted=0  ";
        const string GetByIdsSql = @"SELECT * FROM inte_work_center WHERE IsDeleted = 0 AND Id IN @ids ";
        const string GetByCodeSql = @"SELECT Id,SiteId,Code,Name,Type,Source,Status,IsMixLine,Remark,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted FROM `inte_work_center`  WHERE Code = @Code  AND SiteId=@Site AND IsDeleted=0 ";
        const string GetByResourceId = "SELECT IWC.* FROM inte_work_center_resource_relation IWCRR LEFT JOIN inte_work_center IWC ON IWCRR.WorkCenterId = IWC.Id WHERE IWC.IsDeleted = 0 AND IWCRR.ResourceId = @resourceId";

        const string InsertInteWorkCenterRelationRangSql = "INSERT INTO  `inte_work_center_relation` (  Id,WorkCenterId,SubWorkCenterId,Remark,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted,SiteId) VALUES ( @Id,@WorkCenterId,@SubWorkCenterId,@Remark,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted,@SiteId) ";
        const string RealDelteInteWorkCenterRelationSql = "DELETE  FROM `inte_work_center_relation` WHERE  WorkCenterId = @Id AND IsDeleted=0";
        const string GetInteWorkCenterRelationSqlTemplate = @"SELECT  IWCR.Id, IWCR.WorkCenterId, IWCR.SubWorkCenterId, IWCR.Remark, IWCR.CreatedBy, IWCR.CreatedOn, IWCR.UpdatedBy, IWCR.UpdatedOn, IWCR.IsDeleted, IWCR.SiteId ,IWC.`Code` as WorkCenterCode,IWC.`Name` as WorkCenterName
                                                               FROM inte_work_center_relation IWCR
                                                               LEFT JOIN inte_work_center IWC ON IWCR.SubWorkCenterId=IWC.Id AND IWC.IsDeleted=0 
                                                                WHERE IWCR.IsDeleted=0 AND IWCR.WorkCenterId= @Id";
        const string InteWorkCenterResourceRelationRangSql = "INSERT INTO  `inte_work_center_resource_relation` (  Id,WorkCenterId,ResourceId,Remark,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,IsDeleted,SiteId) VALUES ( @Id,@WorkCenterId,@ResourceId,@Remark,@CreatedBy,@CreatedOn,@UpdatedBy,@UpdatedOn,@IsDeleted,@SiteId) ";
        const string RealDelteInteWorkCenterResourceRelationSql = "DELETE  FROM `inte_work_center_resource_relation` WHERE  WorkCenterId = @Id AND IsDeleted=0";
        const string GetInteWorkCenterResourceRelatioSqlTemplate = @"SELECT  IWRR.Id, IWRR.WorkCenterId, IWRR.ResourceId, IWRR.Remark, IWRR.CreatedBy, IWRR.CreatedOn, IWRR.UpdatedBy, IWRR.UpdatedOn, IWRR.IsDeleted, IWRR.SiteId ,PR.`ResCode` as ResourceCode,PR.`ResName` as ResourceName
                                                                    FROM  inte_work_center_resource_relation IWRR 
                                                                     LEFT JOIN proc_resource PR ON IWRR.ResourceId=PR.Id AND PR.IsDeleted=0 
                                                                   WHERE IWRR.IsDeleted=0 AND  IWRR.WorkCenterId=@Id";
        const string GetHigherInteWorkCenterSql = @"select wc.*
                                                From  inte_work_center_relation  wcr 
                                                left join inte_work_center wc on wc.Id=wcr.WorkCenterId
                                                Where wcr.SubWorkCenterId = @Id ";
        const string GetWorkCenterIdByResourceIdSql = "SELECT WorkCenterId FROM inte_work_center_resource_relation WHERE IsDeleted = 0 AND ResourceId IN @resourceIds";
        const string GetResourceIdsByWorkCenterIdSql = "SELECT ResourceId FROM inte_work_center_resource_relation WHERE IsDeleted = 0 AND WorkCenterId IN @workCenterIds ";
    }
}