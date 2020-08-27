using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Quests.Client.Services
{

    public interface IMessagesService
    {
        Task ShowError(string title, string message);
        Task ShowSuccess(string title, string message);
        Task ShowWarning(string title, string message);
        Task ShowInfo(string title, string message);
    }
    public class MessagesService : IMessagesService
    {
        private readonly IJSRuntime _jsRuntime;

        public MessagesService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ShowError(string title, string message)
        {
            await _jsRuntime.InvokeVoidAsync("toastr.error", message, title);
        }

        public async Task ShowSuccess(string title, string message)
        {
            await _jsRuntime.InvokeVoidAsync("toastr.success", message, title);
        }

        public async Task ShowWarning(string title, string message)
        {
            await _jsRuntime.InvokeVoidAsync("toastr.warning", message, title);
        }

        public async Task ShowInfo(string title, string message)
        {
            await _jsRuntime.InvokeVoidAsync("toastr.info", message, title);
        }
    }
}
