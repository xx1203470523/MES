using Dapper;
using Hymson.MES.Core.Domain.Integrated;
using Hymson.MES.Data.Options;
using Hymson.MES.Data.Repositories.Common.Command;
using Microsoft.Extensions.Options;

namespace Hymson.MES.Data.Repositories.Integrated.InteJobClass
{
    /// <summary>
    /// 生产作业程序仓储
    /// </summary>
    public partial class InteJobClassRepository : BaseRepository, IInteJobClassRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionOptions"></param>
        public InteJobClassRepository(IOptions<ConnectionOptions> connectionOptions) : base(connectionOptions) { }


        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="inteJobClassEntitys"></param>
        /// <returns></returns>
        public async Task<int> InsertsAsync(List<InteJobClassEntity> inteJobClassEntitys)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(InsertsSql, inteJobClassEntitys);
        }

        /// <summary>
        /// 删除（软删除）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(long id)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeleteSql, new { Id = id });
        }

        /// <summary>
        /// 批量删除（软删除）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeletesAsync(DeleteCommand param)
        {
            using var conn = GetMESDbConnection();
            return await conn.ExecuteAsync(DeletesSql, param);
        }

        /// <summary>
        /// 根据IDs批量获取数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<InteJobClassEntity>> GetByIdsAsync(long[] ids)
        {
            using var conn = GetMESDbConnection();
            return await conn.QueryAsync<InteJobClassEntity>(GetByIdsSql, new { Ids = ids });
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class InteJobClassRepository
    {
        const string InsertsSql = "INSERT INTO `inte_job_class`(  `Id`, `Name`, `Namespace`, `Module`, `CreatedBy`, `CreatedOn`, `UpdatedBy`, `UpdatedOn`, `IsDeleted`, `SiteId`) VALUES (   @Id, @Name, @Namespace, @Module, @CreatedBy, @CreatedOn, @UpdatedBy, @UpdatedOn, @IsDeleted, @SiteId )  ";

        const string DeleteSql = "UPDATE `inte_job_class` SET IsDeleted = Id WHERE Id = @Id ";
        const string DeletesSql = "UPDATE `inte_job_class` SET IsDeleted = Id , UpdatedBy = @UserId, UpdatedOn = @DeleteOn WHERE Id IN @Ids";

        const string GetByIdsSql = @"SELECT * FROM `inte_job_class`  WHERE Id IN @Ids ";
    }
}
