using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MessagingService.Entities;
using MessagingService.Helpers;
using MessagingService.Services;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MessagingService.Core
{
    public class Repository<TModel> : IRepository<TModel>
        where TModel : MongoBaseModel
    {
        private readonly IMongoCollection<TModel> _collection;

        public Repository(IOptions<AppSettings> appSettings)
        {
            var modelName = typeof(TModel).Name;
            var collectionName = modelName.Replace("Model", string.Empty).Pluralize();

            var connectionStrings = appSettings.Value.ConnectionStrings;
            var client = new MongoClient(connectionStrings.Default);
            var database = client.GetDatabase(connectionStrings.DatabaseName);
            _collection = database.GetCollection<TModel>(collectionName);
            var x = database.GetCollection<TModel>(collectionName);
        }
        
        public async Task<TModel> Get(Expression<Func<TModel, bool>> filter)
        {
            var model = await _collection.FindAsync(filter);
            return await model.FirstOrDefaultAsync();
        }

        public async Task<IAsyncCursor<TModel>> GetAll(Expression<Func<TModel, bool>> filter,
            FindOptions<TModel> options = null)
        {
            if (filter == null)
            {
                return await _collection.FindAsync(Builders<TModel>.Filter.Empty, options);
            }

            return await _collection.FindAsync(filter, options);
        }

        public async Task<TModel> Create(TModel model)
        {
            await _collection.InsertOneAsync(model);
            return model;
        }

        public async Task Update(string id, TModel model)
        {
            var docId = new ObjectId(id);
            await _collection.ReplaceOneAsync(m => m.Id == docId, model);
        }

       
    }
}