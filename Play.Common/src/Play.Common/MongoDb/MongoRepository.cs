using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Common;

namespace Play.Common.MongoDb
{

    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private const string _connectionString = "mongodb://localhost:27017";
        private const string _dbName = "catalog";
        private readonly IMongoCollection<T> _dbCollection;
        private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase db, string collectionName)
        {
            _dbCollection = db.GetCollection<T>(collectionName);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).ToListAsync();

        }

        public async Task<T> GetAsync(Guid id)
        {
            // var filter = _filterBuilder.Eq(entity => entity.Id, id);
            var filter = _createFilterDefinitionById(id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task PostAsync(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity)); // Single-line nullcheck
            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task PutAsync(T entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));
            // var filter = _filterBuilder.Eq(existingEntity => existingEntity.Id, entity.Id);
            var filter = _createFilterDefinitionById(entity.Id);
            await _dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            // var filter = _filterBuilder.Eq(entity => entity.Id, id);
            var filter = _createFilterDefinitionById(id);
            await _dbCollection.DeleteOneAsync(filter);
        }

        private FilterDefinition<T> _createFilterDefinitionById(Guid id) =>
            _filterBuilder.Eq(entity => entity.Id, id);
    }

}