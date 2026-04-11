using Microsoft.JSInterop;

namespace BlazorDemo.OfficeFileApi.Utils {
    internal static class JsModuleHelper {
        public static async ValueTask SafeDisposeAsync(IJSObjectReference module) {
            if(module is null)
                return;
            try {
                await module.DisposeAsync();
            } catch(JSDisconnectedException) {
                // Circuit already gone
            } catch(ObjectDisposedException) {
                // JS runtime already disposed
            }
        }
    }
}
