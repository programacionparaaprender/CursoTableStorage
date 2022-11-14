﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary
{
    public interface INoSqlStorage<TEntity>
    {
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Get(string rowKey, string partitioKey);
        Task Delete(string rowKey, string partitioKey);
        Task<TEntity> Update(TEntity entity);
        IQueryable<TEntity> All();
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query);
    }
}
