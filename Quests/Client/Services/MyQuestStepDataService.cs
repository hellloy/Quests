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
using Quests.Shared.VM;

namespace Quests.Client.Services
{
    public interface IMyQuestStepDataService
    {
        Task Get(int id);
        Task<AnswerVM> Post(AnswerVM answerVm);
    }
    public class MyQuestStepDataService:IMyQuestStepDataService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _http;
        private readonly SweetAlertService _sweetAlertService;
        private readonly NavigationManager _navigationManager;
        private readonly object _option = new
        {
            overlayColor = "#000000",
            state = "danger",
            message = "Идет загрузка данных..."
        };

        public MyQuestStepDataService(IJSRuntime jsRuntime, HttpClient http, SweetAlertService sweetAlertService, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _http = http;
            _sweetAlertService = sweetAlertService;
            _navigationManager = navigationManager;
        }


        public async Task Get(int id)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            var response = await _http.GetAsync("api/MyQuestSteps/"+id );
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                var hint = await response.Content.ReadAsStringAsync();

                await _sweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Подсказка",
                    Text = hint,
                    Icon = SweetAlertIcon.Info,
                    ConfirmButtonText = "Ок",
                });
            }
        }

        public async Task<AnswerVM> Post(AnswerVM answerVm)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            var response = await _http.PostAsJsonAsync<AnswerVM>("api/MyQuestSteps/",answerVm);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AnswerVM>();
                await _sweetAlertService.FireAsync(new SweetAlertOptions
                {
                    Title = "Результат",
                    Text = result.Message,
                    Icon = result.Result ? SweetAlertIcon.Success : SweetAlertIcon.Error,
                    ConfirmButtonText = "Ок"
                });

                return result;
            }

            return null;
        }
    }
}
