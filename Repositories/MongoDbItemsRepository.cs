using System;
using System.Collections.Generic;
using Catalog.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Repositories
{
    public class MongoDbItemsRepository : IItemsRepository
    {
        private const string databaseName = "catalog";
        private const string collectionName = "items"; 
        private readonly IMongoCollection<Item> itemsCollection;
        private readonly FilterDefinitionBuilder<Item> filterBuilder = Builders<Item>.Filter;

        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase db = mongoClient.GetDatabase(databaseName);
            itemsCollection = db.GetCollection<Item>(collectionName);
        }
        public void CreateItemAsync(Item item)
        {
            itemsCollection.InsertOne(item);
        }

        public void DeleteItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item=>item.Id, id);
            itemsCollection.DeleteOne(filter);
        }

        public Item GetItemAsync(Guid id)
        {
            var filter = filterBuilder.Eq(item=>item.Id, id);
            return itemsCollection.Find(filter).SingleOrDefault();
        }

        public IEnumerable<Item> GetItemsAsync()
        {
            return itemsCollection.Find(new BsonDocument()).ToList();
        }

        public void UpdateItemAsync(Item item)
        {
            var filter = filterBuilder.Eq(existingItem=>existingItem.Id, item.Id);
            itemsCollection.ReplaceOne(filter,item);
        }
    }
}