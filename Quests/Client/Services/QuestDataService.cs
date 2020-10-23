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
using Microsoft.AspNetCore.Components;
using Quests.Client.Features;
using Quests.Shared.Entities.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;

namespace Quests.Client.Services
{
    public interface IQuestDataService
    {
        Task<PagingResponse<Quest>> Get(QuestParameters questParameters);
        Task<List<Quest>> GetAll();
        Task<Quest> Get(int id);
        Task<Quest> Add(Quest quest, string img);
        Task<Quest> Update(Quest quest, string img);
        Task<bool> Delete(int id);

        Task<List<QuestCategory>> GetAllQuestCategories();
        Task<QuestCategory> AddQuestCategory(QuestCategory questCategory);
        Task<bool> DeleteQuestCategory(int id);

    }
    public class QuestDataService : IQuestDataService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly HttpClient _http;
        private readonly IMessagesService _messagesService;
        private readonly SweetAlertService _sweetAlertService;
        private readonly NavigationManager NavigationManager;

        private readonly object _option = new
        {
            overlayColor = "#000000",
            state = "danger",
            message = "Идет загрузка данных..."
        };

        public QuestDataService(IJSRuntime jsRuntime, HttpClient http, IMessagesService messagesService, SweetAlertService sweetAlertService, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _http = http;
            _messagesService = messagesService;
            _sweetAlertService = sweetAlertService;
            NavigationManager = navigationManager;
        }

        public async Task<PagingResponse<Quest>> Get(QuestParameters questParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = questParameters.PageNumber.ToString(),
                ["searchTerm"] = questParameters.SearchTerm ?? "",
                ["orderBy"] = questParameters.OrderBy
            };

            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            var response = await _http.GetAsync(QueryHelpers.AddQueryString("api/Quests", queryStringParam));
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                await _messagesService.ShowError("Ошибка", content);

            }

            var pagingResponse = new PagingResponse<Quest>
            {
                Items = JsonSerializer.Deserialize<List<Quest>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }),
                MetaData = JsonSerializer.Deserialize<MetaData>(response.Headers.GetValues("X-Pagination").First(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            };
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");

            return pagingResponse;

        }

        public async Task<List<Quest>> GetAll()
        {
            var quests = new List<Quest>();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                quests = await _http.GetFromJsonAsync<List<Quest>>("api/Quests?all=true" );
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return quests;
        }

        public async Task<Quest> Get(int id)
        {
            var quest = new Quest();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                quest = await _http.GetFromJsonAsync<Quest>("api/Quests/" + id);
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return quest;
        }

        public async Task<Quest> Add(Quest quest, string img)
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
                NavigationManager.NavigateTo("/quest");
                return quest;
            }

            await _messagesService.ShowError("Error", "При добавлении квеста произошла ошибка");
            return quest;
        }

        public async Task<Quest> Update(Quest quest, string img)
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

            var response = await _http.PutAsJsonAsync<Quest>("api/Quests/" + quest.Id, quest);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Квест", "был удачно обновлен");
                NavigationManager.NavigateTo("/quest");
                return quest;
            }

            await _messagesService.ShowError("Error", "При обновлении квеста произошла ошибка");
            return quest;
        }

        public async Task<bool> Delete(int id)
        {
           var result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Вы уверены?",
                Text = "Вы не сможете восстановить этот квест!",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Да, удалить квест!",
                CancelButtonText = "Нет, отменить удаление"
            });
                
           if (!string.IsNullOrEmpty(result.Value))
           {
                    
              await _http.DeleteAsync("api/Quests/" + id);


              await _sweetAlertService.FireAsync(
                   "Удалено",
                   "Квест был удачно удален",
                   SweetAlertIcon.Success
               );
              return true;
           }
           else if (result.Dismiss == DismissReason.Cancel)
           {

              await _sweetAlertService.FireAsync(
                   "Отмена",
                   "Удаление квеста было отменено",
                   SweetAlertIcon.Error

               );
           }

           return false;

        }

        public async Task<List<QuestCategory>> GetAllQuestCategories()
        {
            var questCategories = new List<QuestCategory>();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                questCategories = await _http.GetFromJsonAsync<List<QuestCategory>>("api/QuestCategories" );
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return questCategories;
        }

        public async Task<QuestCategory> AddQuestCategory(QuestCategory questCategory)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            var response = await _http.PostAsJsonAsync<QuestCategory>("api/QuestCategories", questCategory);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Новая категория", "была удачно добавлена");
                return await response.Content.ReadFromJsonAsync<QuestCategory>();
            }

            await _messagesService.ShowError("Error", "При добавлении категории произошла ошибка");
            return questCategory;
        }

        public async Task<bool> DeleteQuestCategory(int id)
        {
            var res = await _http.DeleteAsync("api/QuestCategories/" + id);
            if (res.IsSuccessStatusCode)
            {
                await _sweetAlertService.FireAsync(
                    "Удалено",
                    "Категория была удалена",
                    SweetAlertIcon.Success
                );
                return true;
            }
            else
            {
                if(res.StatusCode == HttpStatusCode.Conflict)
                {
                    await _sweetAlertService.FireAsync(
                        "Не могу удалить категорию",
                        "В текущей категории есть квесты, переместите квесты и попробуйте ещё раз.",
                        SweetAlertIcon.Error
                    );
                }
                else
                {
                  await  _sweetAlertService.FireAsync(
                        "Ошибка при удалении",
                        "При удалении произошла ошибка, обратитесь к Администратору",
                        SweetAlertIcon.Error
                    );
                }
            }

            return false;

        }

        
    }
}
