using MobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Services
{
    public class RestService : IRestService
    {
        HttpClient httpClient;
        public RestService()
        {
            httpClient = new HttpClient();
        }
        public async Task<List<Item>> GetItemsAsync()
        {
            var itemsList = new List<Item>();
            string url = "https://hyperbaric-unseismic-alleen.ngrok-free.dev/fridgeapp/";
            if(httpClient.BaseAddress == null) //enélkül: This instance has already started one or more requests.
                                               //Properties can only be modified before sending the first request.
                                               //(nem tudom lefrissíteni az oldalt)
                httpClient.BaseAddress = new Uri(url);
            HttpResponseMessage response = await httpClient.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                itemsList = JsonConvert.DeserializeObject<List<Item>>(content);
            }
            return await Task.FromResult(itemsList);
        }
        public async Task<Item> GetItemAsync(int id) //ItemDetailViewModel használja!
        {
            var item = new Item();
            string url = "https://hyperbaric-unseismic-alleen.ngrok-free.dev/fridgeapp/" + id.ToString();
            httpClient.BaseAddress = new Uri(url);
            HttpResponseMessage response = await httpClient.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                string content = response.Content.ReadAsStringAsync().Result;
                item = JsonConvert.DeserializeObject<Item>(content);
            }
            return await Task.FromResult(item);
        }
        public async Task<bool> AddItemAsync(Item item)
        {
            if (item.Id == 0)
            {
                return await Task.FromResult(false);
            }
            else
            {
                string json = JsonConvert.SerializeObject(item);
                var replacedJson = json.Replace("Id", "id").Replace("Food", "food")
                    .Replace("Quantity", "quantity")
                    .Replace("QuantityMeasure", "quantityMeasure")
                    .Replace("Date", "date")
                    .Replace("IsOpened", "isOpened")
                    .Replace("T00:00:00", "");
                StringContent content = new StringContent(replacedJson, Encoding.UTF8, "application/json");
                //string itemcontent = content.ReadAsStringAsync().Result; Teszteléshez!!!
                string url = "https://hyperbaric-unseismic-alleen.ngrok-free.dev/fridgeapp/";
                if (httpClient.BaseAddress == null)
                    httpClient.BaseAddress = new Uri(url);
                HttpResponseMessage response = await httpClient.PostAsync("", content);
                if (response.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }
                else
                    return await Task.FromResult(false);
            }
        }
        public async Task<bool> UpdateItemAsync(int id) //még meg kell írni!
        {
            return await Task.FromResult(false);
        }
        public async Task<bool > DeleteItemAsync(int id) 
        {
            if (id != 0)
            {
                string url = "https://hyperbaric-unseismic-alleen.ngrok-free.dev/fridgeapp/" + id.ToString();
                HttpClient httpClient2 = new HttpClient();
                httpClient2.BaseAddress = new Uri(url);
                HttpResponseMessage response = await httpClient2.DeleteAsync("");
                if (response.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }
                else
                {
                    return await Task.FromResult(false);
                }
            }
            else
            {
                return await Task.FromResult(false);
            }
        }
    }
}
