﻿using AzureStorageLibrary.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Services
{
    public class TableStorage<TEntity> : INoSqlStorage<TEntity> where TEntity:TableEntity,new ()
    {
        private readonly CloudTableClient _cloudTableClient;
        private readonly CloudTable _table;
        public TableStorage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse
            (ConnectionStrings.AzureStorageConnectionString);

            _cloudTableClient = storageAccount.CreateCloudTableClient();
            _table = _cloudTableClient.GetTableReference(typeof(TEntity).Name);
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            var operation = TableOperation.InsertOrMerge(entity);
            var execute = await _table.ExecuteAsync(operation);
            return execute.Result as TEntity;
        }

        public IQueryable<TEntity> All()
        {
            return _table.CreateQuery<TEntity>().AsQueryable();
        }

        public async Task Delete(string rowKey, string partitioKey)
        {
            var entity = await Get(rowKey, partitioKey);
            var operation = TableOperation.Delete(entity);
            await _table.ExecuteAsync(operation);
        }

        public async Task<TEntity> Get(string rowKey, string partitioKey)
        {
            var operation = TableOperation.Retrieve<TEntity>(partitioKey, rowKey);
            var execute = await _table.ExecuteAsync(operation);
            return execute.Result as TEntity;
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            return _table.CreateQuery<TEntity>().Where(query);
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var operation = TableOperation.Replace(entity);
            var execute = await _table.ExecuteAsync(operation);
            return execute.Result as TEntity;
        }
    }

}
