﻿namespace Naos.Core.Domain.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Various options to specify the <see cref="IRepository{TEntity}"/> find operations
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFindOptions<T>
    {
        int? Skip { get; set; }

        int? Take { get; set; }

        OrderOption<T> Order { get; set; }

        IEnumerable<OrderOption<T>> Orders { get; set; }

        IEnumerable<Expression<Func<T, object>>> Includes { get; set; }

        bool HasOrders();
    }
}