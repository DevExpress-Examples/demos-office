using System.Globalization;

namespace BlazorDemo.OfficeFileApi.DemoShell {
    public class DemoTheme {
        const string BsNativeDarkModePostfix = "-dark";
        public string Name { get; }
        public string Title { get { return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Name.Replace("-", " ")); } }
        public string IconCssClass { get { return Name.ToLower(); } }
        public bool IsBootstrapNative { get; }
        public string BootstrapThemeMode => IsBootstrapNative && Name.Contains(BsNativeDarkModePostfix) ? "dark" : "light";
        public string GetCssClass(bool isActive) => isActive ? "active" : "text-body";
        public string ThemePath => IsBootstrapNative ? Name.Replace(BsNativeDarkModePostfix, string.Empty) : Name;
        public DemoTheme(string name, bool isBootstrapNative) {
            Name = name;
            IsBootstrapNative = isBootstrapNative;
        }
    }
}
