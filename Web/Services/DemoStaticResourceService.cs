using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BlazorDemo.Services {
    public interface IDemoStaticResourceService {
        string GetUrlWithVersion(string url);
    }

    public class DemoStaticResourceService : IDemoStaticResourceService {
        public DemoStaticResourceService(IHttpContextAccessor contextAccessor) {
            ContextAccessor = contextAccessor;
        }

        IHttpContextAccessor ContextAccessor { get; set; }

        public string GetUrlWithVersion(string url) {
            var context = ContextAccessor.HttpContext;
            if(context != null) {
                IFileVersionProvider fileVersionProvider = (IFileVersionProvider)context.RequestServices.GetService(typeof(IFileVersionProvider))!;
                url = fileVersionProvider!.AddFileVersionToPath(context.Request.PathBase, url);
            }
            return url;
        }
    }

}
