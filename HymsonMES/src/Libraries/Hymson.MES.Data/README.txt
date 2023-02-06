数据访问层 仓储层
数据访问ORM工具使用 Dapper https://github.com/DapperLib/Dapper
复杂查询使用 Dapper.SqlBuilder
SQL语句常量放置在单独的文件中和具体的仓储用部分类做隔离
SQL常量后缀Sql,SQL模板常量后缀SqlTemplate 不要嫌名字长，易读最重要，编译后速度没有影响