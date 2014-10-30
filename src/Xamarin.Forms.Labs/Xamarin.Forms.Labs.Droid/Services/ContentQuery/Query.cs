using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Xamarin.Forms.Labs.Droid.Services.ContentQuery
{
    internal class Query<T>
        : IOrderedQueryable<T>
    {
        public Query(IQueryProvider provider)
        {
            this.provider = provider;
            this.expression = Expression.Constant(this);
        }

        public Query(IQueryProvider provider, Expression expression)
        {
            this.provider = provider;
            this.expression = expression;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var enumerable = ((IEnumerable<T>)this.provider.Execute(this.expression));
            return (enumerable ?? Enumerable.Empty<T>()).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression
        {
            get { return this.expression; }
        }

        public IQueryProvider Provider
        {
            get { return this.provider; }
        }

        private readonly Expression expression;
        private readonly IQueryProvider provider;
    }
}

