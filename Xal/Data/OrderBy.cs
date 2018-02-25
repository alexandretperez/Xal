using System;
using System.Linq;
using System.Linq.Expressions;

namespace Xal.Data
{
    internal sealed class OrderBy<T, TProperty> : IOrderBy<T> where T : class
    {
        private readonly Expression<Func<T, TProperty>> _keySelector;
        private readonly bool _descending;

        public OrderBy(
            Expression<Func<T, TProperty>> keySelector,
            bool descending)
        {
            _keySelector = keySelector;
            _descending = descending;
        }

        public IOrderedQueryable<T> Run(IQueryable<T> items)
        {
            return _descending ? items.OrderByDescending(_keySelector) : items.OrderBy(_keySelector);
        }
    }
}