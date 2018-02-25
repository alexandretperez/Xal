using System;
using System.Linq;

namespace Xal.Data
{
    public class QueryReadyEventArgs<T> : EventArgs where T : class
    {
        public QueryReadyEventArgs(IQueryable<T> rawData, QueryResult<T> result)
        {
            RawData = rawData;
            Result = result;
        }

        public IQueryable<T> RawData { get; }
        public QueryResult<T> Result { get; }
    }
}