using Dapper;
using Hymson.Infrastructure;
using Hymson.MES.Core.Domain.Process;
using Hymson.MES.Core.Domain.Quality;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Quality
{
    /// <summary>
    /// 不合格组仓储
    /// @author wangkeming
    /// @date 2023-02-11 04:45:25
    /// </summary>
    public partial class QualUnqualifiedGroupRepository : BaseRepository, IQualUnqualifiedGroupRepository
    {
        public QualUnqualifiedGroupRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions)
        {
        }

        #region 不合格代码组
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedInfo<QualUnqualifiedGroupEntity>> GetPagedInfoAsync(QualUnqualifiedGroupPagedQuery param)
        {
            var sqlBuilder = new SqlBuilder();
            var templateData = sqlBuilder.AddTemplate(GetPagedInfoDataSqlTemplate);
            var templateCount = sqlBuilder.AddTemplate(GetPagedInfoCountSqlTemplate);
            sqlBuilder.Where("IsDeleted = 0");
            sqlBuilder.Where("SiteId = @SiteId");
            sqlBuilder.Select("*");

            if (string.IsNullOrEmpty(param.Sorting))
            {
                sqlBuilder.OrderBy("UpdatedOn DESC");
            }
            else
            {
                sqlBuilder.OrderBy(param.Sorting);
            }
            if (!string.IsNullOrWhiteSpace(param.UnqualifiedGroup))
            {
                param.UnqualifiedGroup = $"%{param.UnqualifiedGroup}%";
                sqlBuilder.Where("UnqualifiedGroup like @UnqualifiedGroup");
            }
            if (!string.IsNullOrWhiteSpace(param.UnqualifiedGroupName))
            {
                param.UnqualifiedGroupName = $"%{param.UnqualifiedGroupName}%";
                sqlBuilder.Where("UnqualifiedGroupName like @UnqualifiedGroupName");
            }


            var offSet = (param.PageIndex - 1) * param.PageSize;
            sqlBuilder.AddParameters(new { OffSet = offSet });
            sqlBuilder.AddParameters(new { Rows = param.PageSize });
            sqlBuilder.AddParameters(param);

            using var conn = GetMESDbConnection();
            var qualUnqualifiedGroupEntitiesTask = conn.QueryAsync<QualUnqualifiedGroupEntity>(templateData.RawSql, templateData.Parameters);
            var totalCountTask = conn.ExecuteScalarAsync<int>(templateCount.RawSql, templateCount.Parameters);
            var qualUnqualifiedGroupEntities = await qualUnqualifiedGroupEntitiesTask;
            var totalCount = await totalCountTask;
            return new PagedInfo<QualUnqualifiedGroupEntity>(qualUnqualifiedGroupEntities, param.PageIndex, param.PageSize, totalCount);
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedGroupEntity>> GetListByProcedureIdAsync(QualUnqualifiedGroupQuery param)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("ug.*");
            sqlBuilder.Where("ug.IsDeleted=0");
            sqlBuilder.Where($"ug.SiteId =@SiteId");
            sqlBuilder.LeftJoin("qual_unqualified_group_procedure_relation pr on ug.Id =pr.UnqualifiedGroupId ");
            sqlBuilder.Where("pr.ProcedureId=@ProcedureId");

            using var conn = GetMESDbConnection();
            var qualUnqualifiedGroupEntities = await conn.QueryAsync<QualUnqualifiedGroupEntity>(template.RawSql, param);
            return qualUnqualifiedGroupEntities;
        }

        /// <summary>
        /// 查询List
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedGroupEntity>> GetListByMaterialGroupIddAsync(QualUnqualifiedGroupQuery param)
        {
            var sqlBuilder = new SqlBuilder();
            var template = sqlBuilder.AddTemplate(GetEntitiesSqlTemplate);
            sqlBuilder.Select("ug.*");
            sqlBuilder.Where("ug.IsDeleted=0");
            sqlBuilder.Where($"ug.SiteId =@SiteId");
            sqlBuilder.LeftJoin("proc_material_group_unqualified_group_relation pr on ug.Id =pr.UnqualifiedGroupId ");
            sqlBuilder.Where("pr.MaterialGroupId=@MaterialGroupId");

            using var conn = GetMESDbConnection();
            var qualUnqualifiedGroupEntities = await conn.QueryAsync<QualUnqualifiedGroupEntity>(template.RawSql, param);
            return qualUnqualifiedGroupEntities;
        }

        /// <summary>
        /// 根据ID获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<QualUnqualifiedGroupEntity> GetByIdAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualUnqualifiedGroupEntity>(GetByIdSql, new { Id = id });
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedGroupEntity>> GetByIdsAsync(long[] ids)
        {
            if (ids.Length <= 0)
            {
                return new List<QualUnqualifiedGroupEntity>();
            }

            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualUnqualifiedGroupEntity>(GetByIdsSql, new { ids = ids });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(QualUnqualifiedGroupEntity param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertSql, param);
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertRangAsync(List<QualUnqualifiedGroupEntity> param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, param);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(QualUnqualifiedGroupEntity param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdateSql, param);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateRangAsync(List<QualUnqualifiedGroupEntity> param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(UpdatesSql, param);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeleteRangAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据编码获取数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<QualUnqualifiedGroupEntity> GetByCodeAsync(QualUnqualifiedGroupByCodeQuery param)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryFirstOrDefaultAsync<QualUnqualifiedGroupEntity>(GetByCodeSql, param);
        }
        #endregion

        #region 不合格组关联不合格代码
        /// <summary>
        /// 插入不合格代码组关联不合格代码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertQualUnqualifiedCodeGroupRelationRangAsync(List<QualUnqualifiedCodeGroupRelation> param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertQualUnqualifiedCodeGroupRelationSql, param);
        }

        /// <summary>
        /// 删除不合格组关联不合格代码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> RealDelteQualUnqualifiedCodeGroupRelationAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DelteQualUnqualifiedCodeGroupRelationSql, new { UnqualifiedGroupId = id });
        }

        /// <summary>
        /// 删除不合格组关联不合格代码
        /// </summary>
        /// <param name="unqualifiedCodeId"></param>
        /// <returns></returns>
        public async Task<int> RealDelteQualUnqualifiedCodeGroupRelationByUnqualifiedIdAsync(long unqualifiedCodeId)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DelteQualUnqualifiedCodeGroupRelationByUnqualifiedIdSql, new { UnqualifiedCodeId = unqualifiedCodeId });
        }
        #endregion

        #region 不合格组关联工序
        /// <summary>
        /// 插入不合格代码组关联工序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> InsertQualUnqualifiedGroupProcedureRelationRangAsync(List<QualUnqualifiedGroupProcedureRelation> param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertQualUnqualifiedGroupProcedureRelationSql, param);
        }

        /// <summary>
        /// 删除不合格组关联工序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> RealDelteQualUnqualifiedGroupProcedureRelationAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(RealDelteQualUnqualifiedGroupProcedureRelationSql, new { UnqualifiedGroupId = id });
        }
        #endregion

        /// <summary>
        /// 获取不合格组关联不合格代码关系表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedGroupProcedureRelationView>> GetQualUnqualifiedCodeProcedureRelationAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualUnqualifiedGroupProcedureRelationView>(GetQualUnqualifiedCodeProcedureRelationSqlTemplate, new { Id = id });
        }

        /// <summary>
        /// 获取不合格组关联不合格代码关系表
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task<IEnumerable<QualUnqualifiedGroupCodeRelationView>> GetQualUnqualifiedCodeGroupRelationAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<QualUnqualifiedGroupCodeRelationView>(GetQualUnqualifiedCodeGroupRelationSqlTemplate, new { Id = id });
        }
    }

    public partial class QualUnqualifiedGroupRepository
    {
        const string GetPagedInfoDataSqlTemplate = @"SELECT /**select**/ FROM `qual_unqualified_group` /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/ LIMIT @Offset,@Rows ";
        const string GetPagedInfoCountSqlTemplate = "SELECT COUNT(*) FROM `qual_unqualified_group` /**where**/ ";
        const string GetEntitiesSqlTemplate = @"SELECT    /**select**/  FROM `qual_unqualified_group` ug  /**innerjoin**/ /**leftjoin**/ /**where**/ /**orderby**/  ";
        const string GetByIdSql = @"SELECT 
                               Id, SiteId, UnqualifiedGroup, UnqualifiedGroupName, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted
                                FROM `qual_unqualified_group`  WHERE Id = @Id  AND IsDeleted=0 ";
        const string GetByIdsSql = @"SELECT 
                               Id, SiteId, UnqualifiedGroup, UnqualifiedGroupName, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted
                                FROM `qual_unqualified_group`  WHERE Id in @ids  AND IsDeleted=0 ";
        const string GetByCodeSql = @"SELECT 
                              Id, SiteId, UnqualifiedGroup, UnqualifiedGroupName, Remark, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, IsDeleted
                                FROM `qual_unqualified_group`  WHERE UnqualifiedGroup = @Code AND SiteId = @Site  AND IsDeleted = 0";
        const string GetQualUnqualifiedCodeProcedureRelationSqlTemplate = @"SELECT  QUGP.Id,QUGP.UnqualifiedGroupId,QUGP.ProcedureId,PP.Code AS ProcedureCode,PP.Name AS UnqualifiedCodeName,QUGP.CreatedBy,QUGP.CreatedOn,QUGP.UpdatedBy,QUGP.UpdatedOn
                                                                            FROM qual_unqualified_group_procedure_relation  QUGP LEFT JOIN proc_procedure PP ON QUGP.ProcedureId=PP.Id AND PP.IsDeleted=0 WHERE  QUGP.UnqualifiedGroupId=@Id AND QUGP.IsDeleted = 0";
        const string GetQualUnqualifiedCodeGroupRelationSqlTemplate = @"SELECT  QUCGR.Id,QUCGR.UnqualifiedGroupId,QUCGR.UnqualifiedCodeId,QUC.UnqualifiedCode,QUC.UnqualifiedCodeName,QUCGR.CreatedBy,QUCGR.CreatedOn,QUCGR.UpdatedBy,QUCGR.UpdatedOn
                                                                        FROM qual_unqualified_code_group_relation QUCGR    LEFT JOIN   qual_unqualified_code QUC on QUC.Id=QUCGR.UnqualifiedCodeId AND QUC.IsDeleted=0
                                                                        WHERE QUCGR.UnqualifiedGroupId=@Id AND QUCGR.IsDeleted=0";
        const string InsertSql = "INSERT INTO `qual_unqualified_group`(  `Id`, `SiteId`, `UnqualifiedGroup`, `UnqualifiedGroupName`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @UnqualifiedGroup, @UnqualifiedGroupName, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertsSql = "INSERT INTO `qual_unqualified_group`(  `Id`, `SiteId`, `UnqualifiedGroup`, `UnqualifiedGroupName`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) VALUES (   @Id, @SiteId, @UnqualifiedGroup, @UnqualifiedGroupName, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted )  ";
        const string InsertQualUnqualifiedCodeGroupRelationSql = @"INSERT INTO `qual_unqualified_code_group_relation`(`Id`, `SiteId`, `UnqualifiedCodeId`, `UnqualifiedGroupId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) 
                                                                   VALUES (@Id, @SiteId, @UnqualifiedCodeId, @UnqualifiedGroupId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted)";
        const string InsertQualUnqualifiedGroupProcedureRelationSql = @"INSERT INTO `qual_unqualified_group_procedure_relation`(`Id`, `SiteId`, `UnqualifiedGroupId`, `ProcedureId`, `Remark`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`) 
                                                                        VALUES(@Id, @SiteId, @UnqualifiedGroupId, @ProcedureId, @Remark, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted) ";
        const string UpdateSql = "UPDATE `qual_unqualified_group`  SET    UnqualifiedGroupName = @UnqualifiedGroupName, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string UpdatesSql = "UPDATE `qual_unqualified_group` SET    UnqualifiedGroupName = @UnqualifiedGroupName, Remark = @Remark, CreatedBy = @CreatedBy, CreatedOn = @CreatedOn, UpdatedBy = @UpdatedBy, UpdatedOn = @UpdatedOn  WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `qual_unqualified_group` SET IsDeleted = Id, UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id in @ids AND IsDeleted=0 ";
        const string DelteQualUnqualifiedCodeGroupRelationSql = "DELETE  FROM qual_unqualified_code_group_relation WHERE  UnqualifiedGroupId = @UnqualifiedGroupId AND IsDeleted=0";
        const string DelteQualUnqualifiedCodeGroupRelationByUnqualifiedIdSql = "DELETE  FROM qual_unqualified_code_group_relation WHERE  UnqualifiedCodeId = @UnqualifiedCodeId AND IsDeleted=0";
        const string RealDelteQualUnqualifiedGroupProcedureRelationSql = "DELETE  FROM qual_unqualified_group_procedure_relation WHERE  UnqualifiedGroupId = @UnqualifiedGroupId AND IsDeleted=0";
    }
}
