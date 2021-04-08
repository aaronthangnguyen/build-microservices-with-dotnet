using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;

namespace Play.Catalog.Service.Repository
{
    public interface IItemsRepository
    {
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Item>> GetAllAsync();
        Task<Item> GetAsync(Guid id);
        Task PostAsync(Item entity);
        Task PutAsync(Item entity);
    }

}