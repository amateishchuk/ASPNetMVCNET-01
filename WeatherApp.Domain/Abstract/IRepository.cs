﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.Domain.Abstract
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Insert(T item);

        Task InsertAsync(T item);
        void Update(T item);
        void Delete(T item);
        T Get(Func<T, bool> predicate);
        int Count { get; }

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
    }
}
