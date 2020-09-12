using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.JSInterop;
using Quests.Shared.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Quests.Client.Features;
using Quests.Shared.Entities.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;

namespace Quests.Client.Services
{
    public interface IQuestStepDataService
    {
        Task<PagingResponse<QuestStep>> Get(QuestParameters questStepParameters);
        Task<List<QuestStep>> GetAll();
        Task<List<QuestStep>> GetFromQuest(int questId);
        Task<QuestStep> Get(int id);
        Task<QuestStep> Add(QuestStep questStep, string img);
        Task<QuestStep> Update(QuestStep questStep, string img);
        Task Delete(int id);


    }
    public class QuestStepDataService : IQuestStepDataService
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

        public QuestStepDataService(IJSRuntime jsRuntime, HttpClient http, IMessagesService messagesService, SweetAlertService sweetAlertService)
        {
            _jsRuntime = jsRuntime;
            _http = http;
            _messagesService = messagesService;
            _sweetAlertService = sweetAlertService;
        }

        public async Task<PagingResponse<QuestStep>> Get(QuestParameters questStepParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = questStepParameters.PageNumber.ToString(),
                ["searchTerm"] = questStepParameters.SearchTerm ?? "",
                ["orderBy"] = questStepParameters.OrderBy
            };

            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            var response = await _http.GetAsync(QueryHelpers.AddQueryString("api/QuestSteps", queryStringParam));
            var content = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
            {
                await _messagesService.ShowError("Ошибка", content);

            }

            var pagingResponse = new PagingResponse<QuestStep>
            {
                Items = JsonSerializer.Deserialize<List<QuestStep>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            };
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");

            return pagingResponse;

        }

        public async Task<List<QuestStep>> GetAll()
        {
            var questSteps = new List<QuestStep>();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                questSteps = await _http.GetFromJsonAsync<List<QuestStep>>("api/QuestSteps?all=true" );
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return questSteps;
        }

        
        public async Task<List<QuestStep>> GetFromQuest(int questId)
        {
            var questSteps = new List<QuestStep>();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                questSteps = await _http.GetFromJsonAsync<List<QuestStep>>("api/QuestSteps?questId="+questId);
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return questSteps;
        }

        public async Task<QuestStep> Get(int id)
        {
            var questStep = new QuestStep();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                questStep = await _http.GetFromJsonAsync<QuestStep>("api/QuestSteps/" + id);
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return questStep;
        }

        public async Task<QuestStep> Add(QuestStep questStep, string img)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            if (img != "")
            {
                var responseImg = await _http.PostAsJsonAsync<string>("api/Uploads", img);
                if (responseImg.IsSuccessStatusCode)
                {
                    questStep.Img = await responseImg.Content.ReadAsStringAsync();
                }
                else
                {
                    await _messagesService.ShowWarning("Внимание", "Что-то пошло не так, при загрузке изображения. Попробуйте другой файл");
                }
            }


            var response = await _http.PostAsJsonAsync<QuestStep>("api/QuestSteps", questStep);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Новый квест", "был удачно добавлен");
                return await response.Content.ReadFromJsonAsync<QuestStep>();
            }

            await _messagesService.ShowError("Error", "При добавлении квеста произошла ошибка");
            return questStep;
        }

        public async Task<QuestStep> Update(QuestStep questStep, string img)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            if (img != "")
            {
                var responseImg = await _http.PostAsJsonAsync<string>("api/Uploads", img);
                if (responseImg.IsSuccessStatusCode)
                {
                    questStep.Img = await responseImg.Content.ReadAsStringAsync();
                }
                else
                {
                    await _messagesService.ShowWarning("Внимание", "Что-то пошло не так, при загрузке изображения. Попробуйте другой файл");
                }
            }

            var response = await _http.PutAsJsonAsync<QuestStep>("api/QuestSteps/" + questStep.Id, questStep);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Этап", "был удачно обновлен");
                return questStep;
            }

            await _messagesService.ShowError("Error", "При обновлении квеста произошла ошибка");
            return questStep;
        }

        public async Task Delete(int id)
        {
            await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Вы уверены?",
                Text = "Вы не сможете восстановить этот этап!",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Да, удалить этап!",
                CancelButtonText = "Нет, отменить удаление"
            }).ContinueWith(swalTask =>
            {
                SweetAlertResult result = swalTask.Result;
                if (!string.IsNullOrEmpty(result.Value))
                {
                    
                    _http.DeleteAsync("api/QuestSteps/" + id);


                    _sweetAlertService.FireAsync(
                        "Удалено",
                        "Этап был удачно удален",
                        SweetAlertIcon.Success
                    );

                }
                else if (result.Dismiss == DismissReason.Cancel)
                {

                    _sweetAlertService.FireAsync(
                        "Отмена",
                        "Удаление этапа было отменено",
                        SweetAlertIcon.Error

                    );
                }
            });
            

        }

    }
}
