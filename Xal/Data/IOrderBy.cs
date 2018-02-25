using System.Linq;

namespace Xal.Data
{
    internal interface IOrderBy<T> where T : class
    {
        IOrderedQueryable<T> Run(IQueryable<T> items);
    }
}