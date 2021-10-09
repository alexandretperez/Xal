using System.Linq;

namespace Xal.Data
{
    internal interface IOrderBy<T>
    {
        IOrderedQueryable<T> Run(IQueryable<T> items);
    }
}