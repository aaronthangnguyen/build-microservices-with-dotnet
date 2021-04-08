using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repository
{

    public class ItemsRepository : IItemsRepository
    {
        private const string _connectionString = "mongodb://localhost:27017";
        private const string _dbName = "catalog";
        private const string _collectionName = "items";
        private readonly IMongoCollection<Item> _dbCollection;
        private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;

        public ItemsRepository(IMongoDatabase db)
        {
            _dbCollection = db.GetCollection<Item>(_collectionName);
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            // var filter = _filterBuilder.Eq(entity => entity.Id, id);
            var filter = _createFilterDefinitionById(id);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task PostAsync(Item entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity)); // Single-line nullcheck
            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task PutAsync(Item entity)
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

        private FilterDefinition<Item> _createFilterDefinitionById(Guid id) =>
            _filterBuilder.Eq(entity => entity.Id, id);
    }

}