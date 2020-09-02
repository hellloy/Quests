using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Quests.Shared;

namespace Quests.Client.Services
{
    public interface IQuestDataService
    {
        Task<List<Quest>> Get();
        Task<Quest> Get(int id);
        Task<Quest> Add(Quest quest,string img);
        Task<Quest> Update(Quest quest,string img);
        Task<Quest> Delete(int id);
    }
    public class QuestDataService : IQuestDataService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _http;
        private readonly IMessagesService _messagesService;

        private readonly object _option = new
        {
            overlayColor = "#000000",
            state = "danger",
            message = "Идет загрузка данных..."
        };

        public QuestDataService(IJSRuntime jsRuntime, HttpClient http, IMessagesService messagesService)
        {
            _jsRuntime = jsRuntime;
            _http = http;
            _messagesService = messagesService;
        }

        public async Task<List<Quest>> Get()
        {
            var quests = new List<Quest>();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                quests = await _http.GetFromJsonAsync<List<Quest>>("api/Quests");
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Ошибка", e.Message);
            }
           
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return quests.ToList();
        }

        public async Task<Quest> Get(int id)
        {
            var quest = new Quest();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                quest = await _http.GetFromJsonAsync<Quest>("api/Quests/"+id);
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }
           
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return quest;
        }

        public async Task<Quest> Add(Quest quest,string img)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            if (img != "")
            {
                var responseImg = await _http.PostAsJsonAsync<string>("api/Uploads", img);
                if (responseImg.IsSuccessStatusCode)
                {
                    quest.Img = await responseImg.Content.ReadAsStringAsync();
                }
                else
                {
                    await _messagesService.ShowWarning("Внимание", "Что-то пошло не так, при загрузке изображения. Попробуйте другой файл");
                }
            }


            var response = await _http.PostAsJsonAsync<Quest>("api/Quests", quest);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Новый квест", "был удачно добавлен");
                return await response.Content.ReadFromJsonAsync<Quest>();
            }

            await _messagesService.ShowError("Error", "При добавлении квеста произошла ошибка");
            return quest;
        }

        public async Task<Quest> Update(Quest quest,string img)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option, img);

            if (img != "")
            {
                var responseImg = await _http.PostAsJsonAsync<string>("api/Uploads", img);
                if (responseImg.IsSuccessStatusCode)
                {
                    quest.Img = await responseImg.Content.ReadAsStringAsync();
                }
                else
                {
                    await _messagesService.ShowWarning("Внимание", "Что-то пошло не так, при загрузке изображения. Попробуйте другой файл");
                }
            }
            
            var response = await _http.PutAsJsonAsync<Quest>("api/Quests/"+quest.Id, quest);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Квест", "был удачно обновлен");
                return quest;
            }

            await _messagesService.ShowError("Error", "При обновлении квеста произошла ошибка");
            return quest;
        }

        public async Task<Quest> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
