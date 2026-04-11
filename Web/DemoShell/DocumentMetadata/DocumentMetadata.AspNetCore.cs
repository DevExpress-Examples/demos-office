using Microsoft.AspNetCore.Razor.TagHelpers;

namespace DevExpress.Blazor.DocumentMetadata {
    public static partial class DocumentMetadataExtensions {

        public static void AddDocumentMetadata(this IServiceCollection services, Action<IServiceProvider, IDocumentMetadataCollection> initialize) {
            services.AddMvc();
            services.AddSingleton(new DocumentMetadataSetup(initialize));
            services.AddTransient<ITagHelperComponent, DocumentMetadataTagHelperComponent>();
            services.AddDocumentMetadata();
        }
    }
}
