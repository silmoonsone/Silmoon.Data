using Silmoon.Collections;

/* 项目“Silmoon.Data (netstandard2.0)”的未合并的更改
在此之前:
using System;
在此之后:
using Silmoon.Data;
using Silmoon.Data;
using Silmoon.Data.SqlServer;
using System;
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Silmoon.Data
{
    public class SqlExecuteResult
    {
        public SqlExecuteResult()
        {

        }
        public SqlExecuteResult(int ResponseRows, string ExecuteSqlString)
        {
            this.ResponseRows = ResponseRows;
            this.ExecuteSqlString = ExecuteSqlString;
        }
        public int ResponseRows { get; set; }
        public string ExecuteSqlString { get; set; }
    }
    public class SqlExecuteResults<T> : SqlExecuteResult
    {
        public SqlExecuteResults()
        {

        }
        public SqlExecuteResults(int ResponseRows, string ExecuteSqlString, (T, NameObjectCollection<object>[]) Result) : base(ResponseRows, ExecuteSqlString)
        {
            this.Result = Result.Item1;
            DataCollections = Result.Item2;
        }
        public T Result { get; set; }
        public NameObjectCollection<object>[] DataCollections { get; set; }
    }
    public class SqlExecuteResult<T> : SqlExecuteResult
    {
        public SqlExecuteResult()
        {

        }
        public SqlExecuteResult(int ResponseRows, string ExecuteSqlString, (T, NameObjectCollection<object>) Result) : base(ResponseRows, ExecuteSqlString)
        {
            this.Result = Result.Item1;
            DataCollection = Result.Item2;
        }
        public T Result { get; set; }
        public NameObjectCollection<object> DataCollection { get; set; }
    }
}
