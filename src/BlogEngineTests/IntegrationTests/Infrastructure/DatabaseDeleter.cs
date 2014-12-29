namespace BlogEngineTests.IntegrationTests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    public class DatabaseDeleter
    {
        #region ignored tables

        private static readonly string[] IgnoredTables = new[]
        {
            "sysdiagrams",
            "States",
            "Users"
        };

        #endregion

        private static readonly Lazy<DatabaseDeleter> _instance = new Lazy<DatabaseDeleter>(() => new DatabaseDeleter());

        public static void Initialize(DbContext context)
        {
            if (!_initialized)
            {
                lock (_lockObj)
                {
                    if (string.IsNullOrWhiteSpace(_deleteSql))
                    {
                        BuildDeleteTables(context);

                        _initialized = true;
                    }
                }
            }
        }

        public static DatabaseDeleter Instance
        {
            get
            {
                var instance = _instance.Value;

                if (instance.WasInitialized) return instance;

                throw new InvalidOperationException("DatabaseDeleter must be initialized before it can be used.");
            }
        }

        private static string[] _tablesToDelete;
        private static string _deleteSql;

        private static object _lockObj = new object();
        private static bool _initialized { get; set; }

        private bool WasInitialized
        {
            get { return _initialized; }
        }

        [DebuggerDisplay("{PrimaryKeyTable} <= {ForeignKeyTable}")]
        private class Relationship
        {
            public string PrimaryKeyTable { get; private set; }
            public string ForeignKeyTable { get; private set; }

            public bool IsSelfReferencing
            {
                get { return PrimaryKeyTable == ForeignKeyTable; }
            }
        }

        private class ColumnStoreIndex
        {
            public string IndexName { get; private set; }
            public string TableName { get; private set; }
        }

        public virtual void DeleteAllData(DbContext dbContext)
        {
            using (var tx = dbContext.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                dbContext.Database.ExecuteSqlCommand(_deleteSql);
                tx.Commit();
            }
        }

        public static string[] GetTables()
        {
            return _tablesToDelete;
        }

        private static void BuildDeleteTables(DbContext dbContext)
        {
            var allTables = GetAllTables(dbContext);

            var allRelationships = GetRelationships(dbContext);

            _tablesToDelete = BuildTableList(allTables, allRelationships);

            _deleteSql = BuildTableSql(_tablesToDelete);

            EnsureColumnStoreIndexAreDisabled(dbContext);
        }

        private static void EnsureColumnStoreIndexAreDisabled(DbContext dbContext)
        {
            var indexes = GetColumnStoreIndexes(dbContext);

            if (indexes.Any())
            {

                var sql = new StringBuilder();
                foreach (var index in indexes)
                {
                    sql.Append(string.Format("ALTER INDEX [{0}] ON [{1}] DISABLE;", index.IndexName, index.TableName));
                }

                using (var tx = dbContext.Database.BeginTransaction(IsolationLevel.Serializable))
                {
                    dbContext.Database.ExecuteSqlCommand(sql.ToString());
                    tx.Commit();
                }
            }
        }

        private static string BuildTableSql(IEnumerable<string> tablesToDelete)
        {
            var builder = new StringBuilder();

            foreach (var tableName in tablesToDelete)
            {
                builder.Append(string.Format("delete from [{0}];", tableName));
            }
            return builder.ToString();
        }

        private static string[] BuildTableList(ICollection<string> allTables, Relationship[] allRelationships,
            List<string> tablesToDelete = null)
        {
            if (tablesToDelete == null)
            {
                tablesToDelete = new List<string>();
            }

            var referencedTables = allRelationships
                .Where(rel => !rel.IsSelfReferencing)
                .Select(rel => rel.PrimaryKeyTable)
                .Distinct()
                .ToList();

            var leafTables = allTables.Except(referencedTables).ToList();

            if (referencedTables.Count > 0 && leafTables.Count == 0)
            {
                throw new InvalidOperationException("There is a circular dependency between the DB tables and we can't safely build the list of tables to delete.");
            }

            tablesToDelete.AddRange(leafTables);

            if (referencedTables.Any())
            {
                var relationships = allRelationships.Where(x => !leafTables.Contains(x.ForeignKeyTable)).ToArray();
                var tables = allTables.Except(leafTables).ToArray();
                BuildTableList(tables, relationships, tablesToDelete);
            }

            return tablesToDelete.ToArray();
        }

        private static IList<ColumnStoreIndex> GetColumnStoreIndexes(DbContext dbContext)
        {
            return dbContext.Database.SqlQuery<ColumnStoreIndex>(@"SELECT 
                                                    TableName = t.name,
                                                    IndexName = ind.name     
                                            FROM 
                                                    sys.indexes ind
                                            INNER JOIN 
                                                    sys.tables t ON ind.object_id = t.object_id 
                                            WHERE
	                                                ind.type = 6
                                               AND  ind.is_disabled = 0
                                            ORDER BY 
                                                    t.name, ind.name").ToList();
        }

        private static Relationship[] GetRelationships(DbContext dbContext)
        {
            var otherquery = dbContext.Database.SqlQuery<Relationship>(@"select
                                                                so_pk.name as PrimaryKeyTable
                                                                ,   so_fk.name as ForeignKeyTable
                                                                from
                                                                sysforeignkeys sfk
	                                                                inner join sysobjects so_pk on sfk.rkeyid = so_pk.id
	                                                                inner join sysobjects so_fk on sfk.fkeyid = so_fk.id
                                                                order by
                                                                so_pk.name
                                                                ,   so_fk.name");

            return otherquery
                        .Where(x => !IgnoredTables.Contains(x.PrimaryKeyTable) && !IgnoredTables.Contains(x.ForeignKeyTable))
                        .ToArray();
        }

        private static IList<string> GetAllTables(DbContext dbContext)
        {
            var query =
                dbContext.Database.SqlQuery<string>(
                    "select t.name from sys.tables t INNER JOIN sys.schemas s ON t.schema_id = s.schema_id WHERE s.name = 'dbo'");

            return query.ToList().Except(IgnoredTables).ToList();
        }
    }
}