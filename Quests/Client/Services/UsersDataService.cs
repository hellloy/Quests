using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.JSInterop;
using Quests.Shared.VM;

namespace Quests.Client.Services
{
    public interface IUsersDataService
    {
        Task<UsersVMQuests> Get();
        Task<UserVm> GetUserInfo();
        Task<UserVm> Update(UserVm userVm, string img);
        Task<bool> Delete(string id);
    }

    public class UsersDataService : IUsersDataService
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

        public UsersDataService(IJSRuntime jsRuntime, HttpClient http, IMessagesService messagesService,
            SweetAlertService sweetAlertService)
        {
            _jsRuntime = jsRuntime;
            _http = http;
            _messagesService = messagesService;
            _sweetAlertService = sweetAlertService;
        }

        public async Task<UsersVMQuests> Get()
        {
            var usersVMQuests = new UsersVMQuests();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                usersVMQuests = await _http.GetFromJsonAsync<UsersVMQuests>("api/users");
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return usersVMQuests;
        }

        public async Task<UserVm> GetUserInfo()
        {
            var user = new UserVm();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                user = await _http.GetFromJsonAsync<UserVm>("api/users/0");
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return user;
        }

        public async Task<UserVm> Update(UserVm userVm, string img)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            if (img != "")
            {
                var responseImg = await _http.PostAsJsonAsync<string>("api/Uploads", img);
                if (responseImg.IsSuccessStatusCode)
                {
                    userVm.Img = await responseImg.Content.ReadAsStringAsync();
                }
                else
                {
                    await _messagesService.ShowWarning("Внимание",
                        "Что-то пошло не так, при загрузке изображения. Попробуйте другой файл");
                }
            }

            var response = await _http.PutAsJsonAsync<UserVm>("api/users/" + userVm.Id, userVm);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Квест", "был удачно обновлен");
                return userVm;
            }

            await _messagesService.ShowError("Error", "При обновлении квеста произошла ошибка");
            return userVm;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Вы уверены?",
                Text = "Вы не сможете восстановить этого пользователя!",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Да, удалить пользователя!",
                CancelButtonText = "Нет, отменить удаление"
            });

            if (!string.IsNullOrEmpty(result.Value))
            {
                await _http.DeleteAsync("api/users/" + id);
                await _sweetAlertService.FireAsync(
                    "Удалено",
                    "Пользователь был удачно удален",
                    SweetAlertIcon.Success
                );
                return true;
            }
            else if (result.Dismiss == DismissReason.Cancel)
            {
                await _sweetAlertService.FireAsync(
                    "Отмена",
                    "Удаление пользователя было отменено",
                    SweetAlertIcon.Error
                );
                
            }
            return false;
        }
    }
}
