using System.Collections.Concurrent;
using BlazorDemo.OfficeFileApi.DemoShell;

namespace BlazorDemo.Services {
    public class DemoThemeService {
        public static bool EnableNewBlazorThemes = true;
        DemoTheme _activeTheme;

        public const string ThemeCookieKey = "DXBZCurrentTheme";
        const string DefaultThemeName = "default";

        public DemoThemeService() {
            ResourcesReadyState = new ConcurrentDictionary<string, TaskCompletionSource<bool>>();
        }

        public ConcurrentDictionary<string, TaskCompletionSource<bool>> ResourcesReadyState { get; }
        public DemoTheme ActiveTheme => _activeTheme;
        public DemoTheme DefaultTheme {
            get { return new DemoTheme(DefaultThemeName, true); }
        }
        public string GetBootstrapThemeCssUrl(DemoTheme theme) {
            if(!EnableNewBlazorThemes || theme.IsBootstrapNative) {
#pragma warning disable DX0025 // theme.ThemePath here is built-in theme names like "purple" or "office-white"
                return $"css/switcher-resources/themes/{theme.ThemePath}/bootstrap.min.css";
#pragma warning restore DX0025 // theme.ThemePath here is built-in theme names like "purple" or "office-white"
            }
            return null;
        }
        public void SetActiveThemeByName(string themeName) {
            _activeTheme = DefaultTheme;
        }
    }
}
