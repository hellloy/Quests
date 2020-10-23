using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Quests.Shared.Entities.Models;

namespace Quests.Client.Services
{
    public interface INewsDataService
    {
        Task<List<NewsRecord>> Get();
        Task<NewsRecord> Get(int id);
        Task<NewsRecord> Add(NewsRecord newsRecord, string img);
        Task<NewsRecord> Update(NewsRecord newsRecord, string img);
        Task<bool> Delete(int id);
    }
    public class NewsDataService:INewsDataService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _http;
        private readonly IMessagesService _messagesService;
        private readonly SweetAlertService _sweetAlertService;
        private readonly NavigationManager _navigationManager;

        private readonly object _option = new
        {
            overlayColor = "#000000",
            state = "danger",
            message = "Идет загрузка данных..."
        };

        public NewsDataService(IJSRuntime jsRuntime, HttpClient http, IMessagesService messagesService, SweetAlertService sweetAlertService, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _http = http;
            _messagesService = messagesService;
            _sweetAlertService = sweetAlertService;
            _navigationManager = navigationManager;
        }

        public async Task<List<NewsRecord>> Get()
        {
            var newsRecords = new List<NewsRecord>();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                newsRecords = await _http.GetFromJsonAsync<List<NewsRecord>>("api/NewsRecords");
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return newsRecords;
        }
        public async Task<NewsRecord> Get(int id)
        {
            var newsRecord = new NewsRecord();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                newsRecord = await _http.GetFromJsonAsync<NewsRecord>("api/NewsRecords/" + id);
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return newsRecord;
        }

        public async Task<NewsRecord> Add(NewsRecord newsRecord, string img)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            if (img != "")
            {
                var responseImg = await _http.PostAsJsonAsync<string>("api/Uploads", img);
                if (responseImg.IsSuccessStatusCode)
                {
                    newsRecord.Img = await responseImg.Content.ReadAsStringAsync();
                }
                else
                {
                    await _messagesService.ShowWarning("Внимание", "Что-то пошло не так, при загрузке изображения. Попробуйте другой файл");
                }
            }


            var response = await _http.PostAsJsonAsync<NewsRecord>("api/NewsRecords", newsRecord);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Новая новость", "был удачно добавлена");
                _navigationManager.NavigateTo("/news");
                return newsRecord;
            }

            await _messagesService.ShowError("Error", "При добавлении новости произошла ошибка");
            return newsRecord;
        }

        public async Task<NewsRecord> Update(NewsRecord newsRecord, string img)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            if (img != "")
            {
                var responseImg = await _http.PostAsJsonAsync<string>("api/Uploads", img);
                if (responseImg.IsSuccessStatusCode)
                {
                    newsRecord.Img = await responseImg.Content.ReadAsStringAsync();
                }
                else
                {
                    await _messagesService.ShowWarning("Внимание", "Что-то пошло не так, при загрузке изображения. Попробуйте другой файл");
                }
            }

            var response = await _http.PutAsJsonAsync<NewsRecord>("api/NewsRecords/" + newsRecord.Id, newsRecord);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Новость", "была удачно обновлена");
                _navigationManager.NavigateTo("/news");
                return newsRecord;
            }

            await _messagesService.ShowError("Error", "При обновлении новости произошла ошибка");
            return newsRecord;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Вы уверены?",
                Text = "Вы не сможете восстановить эту новость!",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Да, удалить новость!",
                CancelButtonText = "Нет, отменить удаление"
            });
                
            if (!string.IsNullOrEmpty(result.Value))
            {
                    
                await _http.DeleteAsync("api/NewsRecords/" + id);


                await _sweetAlertService.FireAsync(
                    "Удалено",
                    "Новость была удачно удаленна",
                    SweetAlertIcon.Success
                );
                return true;
            }
            else if (result.Dismiss == DismissReason.Cancel)
            {

                await _sweetAlertService.FireAsync(
                    "Отмена",
                    "Удаление новости было отменено",
                    SweetAlertIcon.Error

                );
            }

            return false;

        }
    }
}
