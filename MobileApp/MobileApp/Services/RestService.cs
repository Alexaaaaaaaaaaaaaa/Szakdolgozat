using MobileApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public async Task<Item> GetItemAsync(int id) 
        {
            var item = new Item();
            //string url = "http://localhost:5046/fridgeApp/GetItem"+id.ToString();
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
        public async Task<bool> AddOrUpdateItemAsync(Item item)
        {
            if (item.Id == 0)
            {
                string json = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                //string url = "http://localhost:5046/fridgeApp/AddNewItems";
                string url = "http://localhost:5046/fridgeApp/";
                httpClient.BaseAddress = new Uri(url);
                HttpResponseMessage response = await httpClient.PostAsync("", content);
                if (response.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }
            }
            else
            {
                string json = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                //string url = "http://localhost:5046/fridgeApp/UpdateItems" + item.Id.ToString();
                string url = "http://localhost:5046/fridgeApp/" + item.Id.ToString();
                httpClient.BaseAddress = new Uri(url);
                HttpResponseMessage response = await httpClient.PostAsync("", content);
                if (response.IsSuccessStatusCode)
                {
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(true);
        }
        public async Task<bool > DeleteItemAsync(int id) 
        {
            if (id != 0)
            {
                //string url = "http://localhost:5046/fridgeApp/DeleteItems" + id.ToString();
                string url = "http://localhost:5046/fridgeApp/" + id.ToString();
                httpClient.BaseAddress = new Uri(url);
                HttpResponseMessage response = await httpClient.DeleteAsync("");
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
