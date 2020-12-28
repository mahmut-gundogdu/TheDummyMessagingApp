using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MessagingService.Entities;
using MessagingService.Services;
using MongoDB.Driver;

namespace MessagingService.Core
{
    public interface IRepository<TModel> where TModel : MongoBaseModel
    {
 
        Task<TModel> Create(TModel model);
        Task Update(string id, TModel model);
        Task<TModel> Get(Expression<Func<TModel, bool>> filter);
        Task<IAsyncCursor<TModel>> GetAll(Expression<Func<TModel, bool>> filter, FindOptions<TModel> options = null);
        
    }
}