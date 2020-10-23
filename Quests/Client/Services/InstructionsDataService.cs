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
    public interface IInstructionsDataService
    {
        Task<List<Instruction>> Get();
        Task<Instruction> Get(int id);
        Task<Instruction> Add(Instruction instruction, string img);
        Task<Instruction> Update(Instruction instruction, string img);
        Task<bool> Delete(int id);
    }
    public class InstructionsDataService:IInstructionsDataService
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

        public InstructionsDataService(IJSRuntime jsRuntime, HttpClient http, IMessagesService messagesService, SweetAlertService sweetAlertService, NavigationManager navigationManager)
        {
            _jsRuntime = jsRuntime;
            _http = http;
            _messagesService = messagesService;
            _sweetAlertService = sweetAlertService;
            _navigationManager = navigationManager;
        }
        public async Task<List<Instruction>> Get()
        {
            var instructions = new List<Instruction>();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                instructions = await _http.GetFromJsonAsync<List<Instruction>>("api/Instructions");
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return instructions;
        }
        public async Task<Instruction> Get(int id)
        {
            var instruction = new Instruction();
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);
            try
            {
                instruction = await _http.GetFromJsonAsync<Instruction>("api/Instructions/" + id);
            }
            catch (Exception e)
            {
                await _messagesService.ShowError("Error", e.Message);
            }

            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            return instruction;
        }

        public async Task<Instruction> Add(Instruction instruction, string img)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            if (img != "")
            {
                var responseImg = await _http.PostAsJsonAsync<string>("api/Uploads", img);
                if (responseImg.IsSuccessStatusCode)
                {
                    instruction.Img = await responseImg.Content.ReadAsStringAsync();
                }
                else
                {
                    await _messagesService.ShowWarning("Внимание", "Что-то пошло не так, при загрузке изображения. Попробуйте другой файл");
                }
            }


            var response = await _http.PostAsJsonAsync<Instruction>("api/Instructions", instruction);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Новая инструкция", "был удачно добавлена");
                _navigationManager.NavigateTo("/instructions");
                return instruction;
            }

            await _messagesService.ShowError("Error", "При добавлении новости произошла ошибка");
            return instruction;
        }

        public async Task<Instruction> Update(Instruction instruction, string img)
        {
            await _jsRuntime.InvokeVoidAsync("KTApp.blockPage", _option);

            if (img != "")
            {
                var responseImg = await _http.PostAsJsonAsync<string>("api/Uploads", img);
                if (responseImg.IsSuccessStatusCode)
                {
                    instruction.Img = await responseImg.Content.ReadAsStringAsync();
                }
                else
                {
                    await _messagesService.ShowWarning("Внимание", "Что-то пошло не так, при загрузке изображения. Попробуйте другой файл");
                }
            }

            var response = await _http.PutAsJsonAsync<Instruction>("api/Instructions/" + instruction.Id, instruction);
            await _jsRuntime.InvokeVoidAsync("KTApp.unblockPage");
            if (response.IsSuccessStatusCode)
            {
                await _messagesService.ShowSuccess("Инструкция", "была удачно обновлена");
                _navigationManager.NavigateTo("/instructions");
                return instruction;
            }

            await _messagesService.ShowError("Error", "При обновлении инструкции произошла ошибка");
            return instruction;
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Вы уверены?",
                Text = "Вы не сможете восстановить эту инструкцию!",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Да, удалить инструкцию!",
                CancelButtonText = "Нет, отменить удаление"
            });
                
            if (!string.IsNullOrEmpty(result.Value))
            {
                    
                await _http.DeleteAsync("api/Instructions/" + id);


                await _sweetAlertService.FireAsync(
                    "Удалено",
                    "Инструкция была удачно удаленна",
                    SweetAlertIcon.Success
                );
                return true;
            }
            else if (result.Dismiss == DismissReason.Cancel)
            {

                await _sweetAlertService.FireAsync(
                    "Отмена",
                    "Удаление инструкции было отменено",
                    SweetAlertIcon.Error

                );
            }

            return false;

        }
    }
}
