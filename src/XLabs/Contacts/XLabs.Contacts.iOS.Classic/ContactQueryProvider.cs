﻿using XLabs.Contacts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MonoTouch.AddressBook;
using XLabs.ContentProvider;

namespace XLabs.Contacts.iOS
{
    internal class ContactQueryProvider : IQueryProvider
    {
        internal ContactQueryProvider(ABAddressBook addressBook)
        {
            this.addressBook = addressBook;
        }

        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        object IQueryProvider.Execute(Expression expression)
        {
            IQueryable<Contact> q = GetContacts().AsQueryable();

            expression = ReplaceQueryable(expression, q);

            if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(IOrderedQueryable<>))
                return q.Provider.CreateQuery(expression);
            else
                return q.Provider.Execute(expression);
        }

        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new Query<TElement>(this, expression);
        }

        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return (TResult)((IQueryProvider)this).Execute(expression);
        }

        private readonly ABAddressBook addressBook;

        private IEnumerable<Contact> GetContacts()
        {
            return this.addressBook.GetPeople().Select(ContactHelper.GetContact);
        }

        private Expression ReplaceQueryable(Expression expression, object value)
        {
            MethodCallExpression mc = expression as MethodCallExpression;
            if (mc != null)
            {
                Expression[] args = mc.Arguments.ToArray();
                Expression narg = ReplaceQueryable(mc.Arguments[0], value);
                if (narg != args[0])
                {
                    args[0] = narg;
                    return Expression.Call(mc.Method, args);
                }
                else
                    return mc;
            }

            ConstantExpression c = expression as ConstantExpression;
            if (c != null && c.Type.GetInterfaces().Contains(typeof(IQueryable<Contact>)))
                return Expression.Constant(value);

            return expression;
        }
    }
}

