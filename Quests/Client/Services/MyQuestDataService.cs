using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.JSInterop;
using Quests.Shared.Entities.Models;
using Quests.Shared.VM;

namespace Quests.Client.Services
{
    public interface IMyQuestDataService
    {
        
        Task<List<MyQuest>> Get();
        Task<MyQuestVm> Get(int id);
        Task<MyQuest> Add(Quest quest);
        Task<MyQuest> Update(MyQuest myQuest);
        Task Delete(int id);

    }
    public class MyQuestDataService: IMyQuestDataService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _http;
        private readonly IMessagesService _messagesService;
        private readonly SweetAlertService _sweetAlertService;

        private readonly object _option = new
        {
            overlayColor = "#000000",
            state = "danger",
            message = "Идет загрузка данных..."
        };


        public MyQuestDataService(IJSRuntime jsRuntime, HttpClient http, IMessagesService messagesService, SweetAlertService sweetAlertService)
        {
            _jsRuntime = jsRuntime;
            _http = http;
            _messagesService = messagesService;
            _sweetAlertService = sweetAlertService;
        }

        public async Task<List<MyQuest>> Get()
        {
            var myQuests = new List<MyQuest>();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                myQuests = await _http.GetFromJsonAsync<List<MyQuest>>("api/MyQuests" );
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return myQuests;
        }

        public async Task<MyQuestVm> Get(int id)
        {
            var myQuest = new MyQuestVm();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                myQuest = await _http.GetFromJsonAsync<MyQuestVm>("api/MyQuests/" + id);
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return myQuest;
        }

        public async Task<MyQuest> Add(Quest quest)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            var response = await _http.PostAsJsonAsync("api/MyQuests/",quest);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Квест", "был удачно добавлен");
                return await response.Content.ReadFromJsonAsync<MyQuest>();
            }

            await _messagesService.ShowError("Error", "При добавлении квеста произошла ошибка");
            return null;
        }

        public async Task<MyQuest> Update(MyQuest myQuest)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            var response = await _http.PutAsJsonAsync<MyQuest>("api/MyQuests/" + myQuest.Id, myQuest);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Квест", "был удачно обновлен");
                return myQuest;
            }

            await _messagesService.ShowError("Error", "При обновлении квеста произошла ошибка");
            return myQuest;
        }

        public async Task Delete(int id)
        {
            await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Вы уверены?",
                Text = "Вы не сможете восстановить этот квест!",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Да, удалить квест!",
                CancelButtonText = "Нет, отменить удаление"
            }).ContinueWith(swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (!string.IsNullOrEmpty(result.Value))
                {
                    
                    _http.DeleteAsync("api/MyQuests/" + id);


                    _sweetAlertService.FireAsync(
                        "Удалено",
                        "Квест был удачно удален",
                        SweetAlertIcon.Success
                    );

                }
                else if (result.Dismiss == DismissReason.Cancel)
                {

                    _sweetAlertService.FireAsync(
                        "Отмена",
                        "Удаление квеста было отменено",
                        SweetAlertIcon.Error

                    );
                }
            });
        }
    }
}
