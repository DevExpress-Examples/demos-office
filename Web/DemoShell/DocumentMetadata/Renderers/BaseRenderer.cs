using BlazorDemo.OfficeFileApi;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace DevExpress.Blazor.DocumentMetadata
{
    public readonly partial struct Renderer {
        public static Renderer BaseHref(string url) {
            return  new Renderer(RendererFlag.BaseHref | RendererFlag.UniqueByType, url);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "ASP0006:Do not use non-literal sequence numbers", Justification = "<Pending>")]
        public int BaseHrefRender(RenderTreeBuilder renderTreeBuilder, int seq, NavigationManager navigationManager) {
            renderTreeBuilder.OpenElement(seq + 0, "base");
            renderTreeBuilder.AddAttribute(seq + 1, "href", navigationManager.ResolveUrl(_mainAttributeValue));
            renderTreeBuilder.CloseElement();
            return seq + 2;
        }
    }
}
