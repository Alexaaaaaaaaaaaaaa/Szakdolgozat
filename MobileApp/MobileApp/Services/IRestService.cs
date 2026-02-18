using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MobileApp.Models;

namespace MobileApp.Services
{
    public interface IRestService
    {
        Task<Item> GetItemAsync(int id);
        Task<List<Item>> GetItemsAsync();
        Task<bool> AddItemAsync(Item item);
        Task<bool> UpdateItemAsync(int id, Item item);
        Task<bool> DeleteItemAsync(int id);
    }
}
