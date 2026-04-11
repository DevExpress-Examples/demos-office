using System.Web;
using Microsoft.AspNetCore.Components;

namespace BlazorDemo.OfficeFileApi {
    public static class DemoRenderUtils {
        const string ExcludedPageSectionsQueryParameter = "excludedPageSections";

        public static bool PreventRenderDemoSection(string uri, string demoSectionId) {
            var queryString = (new Uri(uri)).Query;
            var queryCollection = HttpUtility.ParseQueryString(queryString);
            var excludedDemos = queryCollection.Get(ExcludedPageSectionsQueryParameter);
            if(!string.IsNullOrEmpty(excludedDemos) && excludedDemos.Split(',').Contains(demoSectionId)) {
                return true;
            }

            return false;
        }

        public static string CombineCssClasses(params string[] value) => CombineCssClasses((IEnumerable<string>)value);
        public static string CombineCssClasses(IEnumerable<string> value) {
            var cssClass = string.Join(" ", value.Where(v => !string.IsNullOrWhiteSpace(v))).Trim();
            return string.IsNullOrWhiteSpace(cssClass) ? null : cssClass;
        }
    }

    public static class SignDemoUtils {
        public static string GetSignDemoPassword(IConfiguration config) {
            return config["SignatureDemoSettings:Password"];
        }
    }

    static class NavigationManagerExtensions {
        public static string GetCurrentPageName(this NavigationManager navigationManager) =>
            navigationManager.GetPageNameByLocation(navigationManager.Uri);

        public static string GetPageNameByLocation(this NavigationManager navigationManager, string location) {
            var uriFragment = navigationManager.ToAbsoluteUri(location).Fragment;
            if(!string.IsNullOrEmpty(uriFragment))
                location = location.Replace(uriFragment, "");
            var pageName = navigationManager.ToBaseRelativePath(location);
            if(pageName.EndsWith('/'))
                pageName = pageName.Substring(0, pageName.LastIndexOf('/'));
            return pageName;
        }

        public static string ResolveUrl(this NavigationManager navigationManager, string url) {
            if(url.StartsWith("~/")) {
                string baseUrl = navigationManager.BaseUri;
                string absoluteUrl = baseUrl + url.Substring(2);
                url = navigationManager.ToBaseRelativePath(absoluteUrl);
                url = navigationManager.ToAbsoluteUri(url).PathAndQuery;
            }
            return url;
        }
    }
}
