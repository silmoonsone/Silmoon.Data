using Microsoft.Data.SqlClient;
using Silmoon.Data.QueryModel;
using Silmoon.Extension;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Silmoon.Runtime;

namespace Silmoon.Data.SqlServer
{
    public class SqlServerExecuter : IDisposable
    {
        SqlServerOperate SqlServerOperate { get; set; }
        public SqlConnection Connection { get; }

        public SqlServerExecuter(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
            SqlServerOperate = new SqlServerOperate(Connection);
        }
        public SqlExecuteResult AddObject<T>(string tableName, T obj)
        {
            var fieldInfos = obj.GetPropertyValueInfoDictionary("id");

            string sql = $"INSERT INTO [{SqlServerHelper.SafeSqlWord(tableName)}] (";
            foreach (var item in fieldInfos)
            {
                sql += "[" + item.Value.Name + "], ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += ") VALUES (";
            foreach (var item in fieldInfos)
            {
                sql += "@" + item.Value.Name + ", ";
            }
            sql = sql.Substring(0, sql.Length - 2);
            sql += ")";
            var cmd = SqlServerOperate.GetDataCommand(sql);
            cmd.AddParameters(fieldInfos);
            int i = cmd.ExecuteNonQuery();
            return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
        }
        public void AddObjects<T>(string tableName, T[] obj) => obj.Each(x => AddObject(tableName, x));

        public SqlExecuteResult<T> GetObject<T>(string tableName, object whereObject, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);


            makeWhereString(ref sql, ref tableName, ref fieldInfos);
            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);


            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return new SqlExecuteResult<T>(reader.RecordsAffected, sql, default);
                    var obj = reader.DeserializeObject(new T(), options.ExcludedField);
                    return new SqlExecuteResult<T>(reader.RecordsAffected, sql, obj);
                }
            }
        }
        public SqlExecuteResult<T> GetObject<T>(string tableName, ExpandoObject whereObject, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);


            makeWhereString(ref sql, ref tableName, ref fieldInfos);
            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);


            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return new SqlExecuteResult<T>(reader.RecordsAffected, sql, default);
                    var obj = reader.DeserializeObject(new T(), options.ExcludedField);
                    return new SqlExecuteResult<T>(reader.RecordsAffected, sql, obj);
                }
            }
        }
        public SqlExecuteResult<T> GetObjectWithWhere<T>(string tableName, string whereString, object whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);

            if (!string.IsNullOrEmpty(whereString))
            {
                sql += " WHERE " + whereString;
            }

            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read()) return new SqlExecuteResult<T>(reader.RecordsAffected, sql, default);
                    var obj = reader.DeserializeObject(new T(), options.ExcludedField);
                    return new SqlExecuteResult<T>(reader.RecordsAffected, sql, obj);
                }
            }
        }

        public SqlExecuteResults<T[]> GetObjects<T>(string tableName, object whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);


            makeWhereString(ref sql, ref tableName, ref fieldInfos);
            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                using (var reader = cmd.ExecuteReader())
                {
                    //if (!reader.Read()) return default;
                    var obj = reader.DeserializeObjects<T>(options.ExcludedField);
                    return new SqlExecuteResults<T[]>(reader.RecordsAffected, sql, obj);
                }
            }
        }
        public SqlExecuteResults<T[]> GetObjects<T>(string tableName, ExpandoObject whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);


            makeWhereString(ref sql, ref tableName, ref fieldInfos);
            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                using (var reader = cmd.ExecuteReader())
                {
                    //if (!reader.Read()) return default;
                    var obj = reader.DeserializeObjects<T>(options.ExcludedField);
                    return new SqlExecuteResults<T[]>(reader.RecordsAffected, sql, obj);
                }
            }
        }
        public SqlExecuteResults<T[]> GetObjectsWithWhere<T>(string tableName, string whereString = null, object whereObject = null, SqlQueryOptions options = null) where T : new()
        {
            if (options == null) options = SqlQueryOptions.Create();
            if ((options.Count.HasValue || options.Offset.HasValue) && (options.Sorts == null || options.Sorts.Count() == 0)) throw new ArgumentException("指定分页参数的时候不能缺少Sorts指定。", "SqlQueryOptions::Sorts");
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();

            string sql = $"SELECT";
            if (!options.Offset.HasValue && options.Count.HasValue)
                sql += $" TOP {options.Count} {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";
            else sql += $" {makeSelectFieldString(fieldInfos, tableName, ref options)} FROM [{tableName}]";

            if (options.Join != null) makeOnString(ref sql, ref tableName, options.Join);


            if (!string.IsNullOrEmpty(whereString))
            {
                sql += " WHERE " + whereString;
            }

            makeOrderBy(ref sql, ref tableName, ref options);
            makeOffset(ref sql, ref options);

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                using (var reader = cmd.ExecuteReader())
                {
                    //if (!reader.Read()) return default;
                    var obj = reader.DeserializeObjects<T>(options.ExcludedField);
                    return new SqlExecuteResults<T[]>(reader.RecordsAffected, sql, obj);
                }
            }
        }


        public SqlExecuteResult SetObjects<T>(string tableName, T obj, object whereObject, params string[] updateObjectFieldNames) => SetObjectsInternal(tableName, obj, null, whereObject, updateObjectFieldNames);
        public SqlExecuteResult SetObjects<T>(string tableName, T obj, ExpandoObject whereObject, params string[] updateObjectFieldNames) => SetObjectsInternal(tableName, obj, null, whereObject, updateObjectFieldNames);
        public SqlExecuteResult SetObjects<T>(string tableName, T obj, string whereString, object whereObject = null, params string[] updateObjectFieldNames) => SetObjectsInternal(tableName, obj, whereString, whereObject, updateObjectFieldNames);

        public SqlExecuteResult SetObjects<T>(string tableName, T obj, object whereObject, params Expression<Func<T, object>>[] updateExpressions) => SetObjectsInternal(tableName, obj, null, whereObject, updateExpressions);
        public SqlExecuteResult SetObjects<T>(string tableName, T obj, ExpandoObject whereObject, params Expression<Func<T, object>>[] updateExpressions) => SetObjectsInternal(tableName, obj, null, whereObject, updateExpressions);
        public SqlExecuteResult SetObjects<T>(string tableName, T obj, string whereString, object whereObject = null, params Expression<Func<T, object>>[] updateExpressions) => SetObjectsInternal(tableName, obj, whereString, whereObject, updateExpressions);


        SqlExecuteResult SetObjectsInternal(string tableName, object obj, string whereString, object whereObject, params string[] updateObjectFieldNames)
        {
            string sql = $"UPDATE [{tableName}] SET ";
            string[] setNames = updateObjectFieldNames;
            var setFieldInfos = obj.GetPropertyValueInfoDictionary("id");

            if (setNames.IsNullOrEmpty()) setNames = setFieldInfos.Select(x => x.Key).ToArray();

            foreach (var item in setNames)
            {
                sql += $"[{item}] = @{item}, ";
            }
            sql = sql.Substring(0, sql.Length - 2);


            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();

            if (whereString.IsNullOrEmpty())
                makeWhereString(ref sql, ref tableName, ref fieldInfos);
            else sql += " WHERE " + whereString;

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(obj.GetPropertyValueInfoDictionary("id"), setNames);
                cmd.AddParameters(fieldInfos);

                int returnLine = cmd.ExecuteNonQuery();
                return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = returnLine };
            }
        }
        SqlExecuteResult SetObjectsInternal<T>(string tableName, T obj, string whereString, object whereObject, params Expression<Func<T, object>>[] updateExpressions)
        {
            string sql = $"UPDATE [{tableName}] SET ";

            var updateFieldNames = updateExpressions.Select(expr =>
            {
                var memberExpression = expr.Body as MemberExpression;
                if (memberExpression == null && expr.Body is UnaryExpression unaryExpression) memberExpression = unaryExpression.Operand as MemberExpression;
                if (memberExpression == null) throw new ArgumentException("无效的表达式。只支持属性表达式。");
                return memberExpression.Member.Name;
            }).ToArray();

            foreach (var item in updateFieldNames)
            {
                sql += $"[{item}] = @{item}, ";
            }
            sql = sql.Substring(0, sql.Length - 2);

            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();

            if (whereString.IsNullOrEmpty())
                makeWhereString(ref sql, ref tableName, ref fieldInfos);
            else sql += " WHERE " + whereString;

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(obj.GetPropertyValueInfoDictionary("id"), updateFieldNames);
                cmd.AddParameters(fieldInfos);

                int returnLine = cmd.ExecuteNonQuery();
                return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = returnLine };
            }
        }

        public SqlExecuteResult DeleteObjects(string tableName, object whereObject)
        {
            string sql = $"DELETE [{tableName}]";
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();
            makeWhereString(ref sql, ref tableName, ref fieldInfos);

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                int i = cmd.ExecuteNonQuery();
                return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
            }
        }
        public SqlExecuteResult DeleteObjects(string tableName, ExpandoObject whereObject)
        {
            string sql = $"DELETE [{tableName}]";
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();
            makeWhereString(ref sql, ref tableName, ref fieldInfos);

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                int i = cmd.ExecuteNonQuery();
                return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
            }
        }
        public SqlExecuteResult DeleteObjects(string tableName, string whereString, object whereObject = null)
        {
            string sql = $"DELETE [{tableName}]";
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();
            if (!string.IsNullOrEmpty(whereString))
            {
                sql += " WHERE " + whereString;
            }

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                int i = cmd.ExecuteNonQuery();
                return new SqlExecuteResult() { ExecuteSqlString = sql, ResponseRows = i };
            }
        }

        public int Count(string tableName, object whereObject)
        {
            string sql = $"SELECT COUNT(*) FROM [{tableName}]";
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();
            makeWhereString(ref sql, ref tableName, ref fieldInfos);

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
        public int Count(string tableName, ExpandoObject whereObject)
        {
            string sql = $"SELECT COUNT(*) FROM [{tableName}]";
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();
            makeWhereString(ref sql, ref tableName, ref fieldInfos);

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
        public int Count(string tableName, string whereString, object whereObject = null)
        {
            string sql = $"SELECT COUNT(*) FROM [{tableName}]";
            var fieldInfos = whereObject.GetPropertyValueInfoDictionary();
            if (!string.IsNullOrEmpty(whereString)) sql += " WHERE " + whereString;

            using (var cmd = SqlServerOperate.GetDataCommand(sql))
            {
                cmd.AddParameters(fieldInfos);
                var result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        public SqlExecuteResult<bool> CreateTable<T>(string tableName)
        {
            var isExistResult = IsTableExists(tableName);
            if (isExistResult.Result) return new SqlExecuteResult<bool>() { Result = false, ResponseRows = isExistResult.ResponseRows, ExecuteSqlString = isExistResult.ExecuteSqlString };
            var props = typeof(T).GetProperties();

            string sql = $"CREATE TABLE [{SqlServerHelper.SafeSqlWord(tableName)}]\r\n";
            sql += $"(\r\n";
            sql += $"[id] int NOT NULL IDENTITY (1, 1),\r\n";
            foreach (var item in props)
            {
                if (item.Name.ToLower() == "id") continue;
                var type = item.PropertyType;
                if (type.IsEnum)
                {
                    sql += $"\t[{item.Name}] int,\r\n";
                }
                else if (type.Name == "DateTime")
                {
                    sql += $"\t{item.Name} datetime NULL,\r\n";
                }
                else if (type.Name == "String")
                {
                    sql += $"\t[{item.Name}] nvarchar(MAX) NULL,\r\n";
                }
                else if (type.Name == "Boolean")
                {
                    sql += $"\t{item.Name} bit,\r\n";
                }
                else if (type.Name == "Int16" || type.Name == "UInt16")
                {
                    sql += $"\t[{item.Name}] smallint,\r\n";
                }
                else if (type.Name == "Int32" || type.Name == "UInt32")
                {
                    sql += $"\t[{item.Name}] int,\r\n";
                }
                else if (type.Name == "Int64" || type.Name == "UInt64")
                {
                    sql += $"\t[{item.Name}] bigint,\r\n";
                }
                else if (type.Name == "Decimal")
                {
                    sql += $"\t[{item.Name}] decimal(18, 4) NULL,\r\n";
                }
                else if (type.Name == "Guid")
                {
                    sql += $"\t[{item.Name}] uniqueidentifier NULL,\r\n";
                }
                else if (type.Name == "ObjectId")
                {
                    sql += $"\t[{item.Name}] nvarchar(24),\r\n";
                }
                else if (type.Name == "Byte[]")
                {
                    sql += $"\t[{item.Name}] VARBINARY(5120),\r\n";
                }
                else if (type.Name == "Int32[]")
                {
                    sql += $"\t[{item.Name}] nvarchar(MAX) NULL,\r\n";
                }
                else if (type.Name == "String[]")
                {
                    sql += $"\t[{item.Name}] nvarchar(MAX) NULL,\r\n";
                }
                else if (type.Name == "JObject")
                {
                    sql += $"\t[{item.Name}] nvarchar(MAX) NULL,\r\n";
                }
                else
                {
                    sql += $"\t[{item.Name}] nvarchar(MAX) NULL,\r\n";

                    //throw new Exception("未知的类型处理（" + type.Name + "）");
                }
            }
            sql += ")  ON [PRIMARY]\r\n";
            //sql += "TEXTIMAGE_ON [PRIMARY]\r\n";
            var result = SqlServerOperate.ExecuteNonQuery(sql);
            return new SqlExecuteResult<bool>() { Result = true, ExecuteSqlString = sql, ResponseRows = result };
        }
        public SqlExecuteResult<bool> IsTableExists(string tableName)
        {
            var sql = $"SELECT TOP 1 COUNT(*) FROM [SYSOBJECTS] WHERE id = OBJECT_ID(N'{tableName}')";
            var result = SqlServerOperate.GetRecordCount(sql);
            return new SqlExecuteResult<bool>() { Result = result != 0, ExecuteSqlString = sql, ResponseRows = result };
        }


        public SqlTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted) => SqlServerOperate.BeginTransaction(isolationLevel);
        public void CommitTransaction(SqlTransaction sqlTransaction) => SqlServerOperate.CommitTransaction();
        public void RollbackTransaction(SqlTransaction sqlTransaction) => SqlServerOperate.RollbackTransaction();

        private string makeSelectFieldString(Dictionary<string, PropertyValueInfo> fieldInfos, string tableName, ref SqlQueryOptions options)
        {
            if (options.FieldOption == SelectFieldOption.All)
            {
                return "*";
            }
            else if (options.FieldOption == SelectFieldOption.SpecifiedObject)
            {
                string s = string.Empty;
                foreach (var item in fieldInfos)
                {
                    if (options.ExcludedField?.Contains(item.Value.Name) ?? false) continue;
                    s += "[" + tableName + "].[" + item + "], ";
                }
                return s.Substring(0, s.Length - 2);
            }
            else return "*";
        }
        private void makeOnString(ref string sql, ref string tableName, JoinOption onOption)
        {
            sql += $" {onOption.On.ToString().ToUpper()} JOIN [{onOption.TableName}] ON (";
            foreach (var item in onOption.FieldNames)
            {
                sql += $"[{tableName}].[{item}] = [{onOption.TableName}].[{item}] AND ";
            }
            sql = sql.Substring(0, sql.Length - 5);
            sql += ")";
        }
        private void makeWhereString(ref string sql, ref string tableName, ref Dictionary<string, PropertyValueInfo> fieldInfos, bool addWhereStr = true)
        {
            if (fieldInfos.Count != 0)
            {
                if (addWhereStr) sql += " WHERE ";
                foreach (var item in fieldInfos)
                {
                    if (item.Value.Value == null)
                        sql += $"[{tableName}].[{item.Value.Name}] IS NULL AND ";
                    else
                        sql += $"[{tableName}].[{item.Value.Name}] = @{item.Value.Name} AND ";
                }
                sql = sql.Substring(0, sql.Length - 5);
            }
        }
        private void makeOrderBy(ref string sql, ref string tableName, ref SqlQueryOptions options)
        {
            if (options.Sorts != null && options.Sorts.Count() != 0)
            {
                sql += " ORDER BY";
                foreach (var item in options.Sorts)
                {
                    sql += $" [{tableName}].[{item.Name}]";
                    if (item.Method == QueryModel.SortMethod.Asc)
                        sql += " ASC,";
                    else sql += " DESC,";
                }
                sql = sql.Substring(0, sql.Length - 1);
            }
        }
        private void makeOffset(ref string sql, ref SqlQueryOptions options)
        {
            if (options.Offset.HasValue)
            {
                sql += $" OFFSET {options.Offset} ROWS";
                if (options.Count.HasValue)
                    sql += $" FETCH NEXT {options.Count} ROWS ONLY";
            }
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
